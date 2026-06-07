using System.Net.Http.Json;
using System.Text.Json;
using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Consts;
using Gtlabs.Redis.Authentication.Entities;
using Gtlabs.Redis.Authentication.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Gtlabs.Api.ApiCall.Authentication;

public class CachedAppTokenProvider : IAppTokenProvider
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly IAuthCacheService _authCacheService;
    private readonly IAmbientData _ambientData;
    private readonly IOptions<AppTokenHydrationOptions> _options;

    public CachedAppTokenProvider(
        HttpClient httpClient,
        IMemoryCache memoryCache,
        IAuthCacheService authCacheService,
        IAmbientData ambientData,
        IOptions<AppTokenHydrationOptions> options)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
        _authCacheService = authCacheService;
        _ambientData = ambientData;
        _options = options;
    }

    public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        var appId = _ambientData.GetAppId();
        var cacheKey = GetMemoryCacheKey(appId);

        if (_memoryCache.TryGetValue(cacheKey, out string? memoryToken) && !string.IsNullOrWhiteSpace(memoryToken))
        {
            return memoryToken;
        }

        var redisToken = await _authCacheService.GetCachedServiceToken(appId);
        if (!string.IsNullOrWhiteSpace(redisToken))
        {
            SetMemoryToken(cacheKey, redisToken);
            return redisToken;
        }

        if (ShouldSkipAuthorizationServiceSelfRequest(appId))
        {
            return string.Empty;
        }

        var token = await RequestTokenAsync(appId, cancellationToken);

        SetMemoryToken(cacheKey, token);
        await _authCacheService.SetCachedServiceToken(
            new CachedServiceJwt(appId) { Token = token },
            _options.Value.RedisCacheDuration);

        return token;
    }

    private async Task<string> RequestTokenAsync(string appId, CancellationToken cancellationToken)
    {
        var options = _options.Value;
        var gatewayUrl = _ambientData.GetGatewayUrl();

        if (string.IsNullOrWhiteSpace(options.AuthorizationServiceName))
        {
            throw new InvalidOperationException("Authorization service name was not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.TokenEndpoint))
        {
            throw new InvalidOperationException("Authorization token endpoint was not configured.");
        }

        if (string.IsNullOrWhiteSpace(gatewayUrl))
        {
            throw new InvalidOperationException("Gateway URL is required to request an app token.");
        }

        var request = new HttpRequestMessage(
            options.TokenRequestMethod,
            BuildTokenEndpointUrl(gatewayUrl, options.TokenEndpoint));

        request.Headers.TryAddWithoutValidation(HeaderFields.ServiceName, options.AuthorizationServiceName);
        request.Headers.TryAddWithoutValidation(HeaderFields.ServiceKey, _ambientData.GetServiceToken());
        request.Headers.TryAddWithoutValidation(HeaderFields.Timestamp, DateTime.UtcNow.ToString("o"));

        var correlationId = _ambientData.GetCorrelationId();
        if (correlationId.HasValue)
        {
            request.Headers.TryAddWithoutValidation(HeaderFields.CorrelationId, correlationId.Value.ToString());
        }

        if (RequiresBody(options.TokenRequestMethod))
        {
            request.Content = JsonContent.Create(new AppTokenRequest(appId));
        }

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Failed to request app token. HTTP {(int)response.StatusCode}: {responseBody}");
        }

        var token = ExtractToken(responseBody);
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("Authorization service returned an empty app token.");
        }

        return token;
    }

    private void SetMemoryToken(string cacheKey, string token)
    {
        _memoryCache.Set(cacheKey, token, _options.Value.MemoryCacheDuration);
    }

    private bool ShouldSkipAuthorizationServiceSelfRequest(string appId)
        => _options.Value.SkipTokenRequestWhenCurrentAppIsAuthorizationService &&
           string.Equals(appId, _options.Value.AuthorizationServiceName, StringComparison.OrdinalIgnoreCase);

    private static string GetMemoryCacheKey(string appId)
        => $"service-jwt:{appId}";

    private static string BuildTokenEndpointUrl(string gatewayUrl, string tokenEndpoint)
        => $"{gatewayUrl.TrimEnd('/')}/{tokenEndpoint.TrimStart('/')}";

    private static bool RequiresBody(HttpMethod method)
        => method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH";

    private static string ExtractToken(string responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            return string.Empty;
        }

        JsonElement root;

        try
        {
            using var document = JsonDocument.Parse(responseBody);
            root = document.RootElement.Clone();
        }
        catch (JsonException)
        {
            return responseBody.Trim().Trim('"');
        }

        if (root.ValueKind == JsonValueKind.String)
        {
            return root.GetString() ?? string.Empty;
        }

        if (root.ValueKind == JsonValueKind.Object)
        {
            if (TryGetTokenProperty(root, "token", out var token) ||
                TryGetTokenProperty(root, "accessToken", out token) ||
                TryGetTokenProperty(root, "access_token", out token))
            {
                return token;
            }
        }

        return responseBody.Trim().Trim('"');
    }

    private static bool TryGetTokenProperty(JsonElement root, string propertyName, out string token)
    {
        token = string.Empty;

        if (!root.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        token = property.GetString() ?? string.Empty;
        return !string.IsNullOrWhiteSpace(token);
    }

    private sealed record AppTokenRequest(string AppId);
}
