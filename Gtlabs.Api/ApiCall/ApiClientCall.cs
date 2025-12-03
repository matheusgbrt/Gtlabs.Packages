using System.Text;
using System.Text.Json;

namespace Gtlabs.Api.ApiCall;

public class ApiClientCall : IApiClientCall
{
    private readonly HttpClient _client;

    public ApiClientCall()
    {
        _client = new HttpClient();
    }

    public async Task<ApiResponseEnvelope> ExecuteAsync(ApiClientCallPrototype proto)
    {
        var response = await ExecuteInternalAsync(proto, safe: false);

        if (!response.Success)
            throw new HttpRequestException(response.Error);

        return response;
    }

    public Task<ApiResponseEnvelope> ExecuteSafeAsync(ApiClientCallPrototype proto)
        => ExecuteInternalAsync(proto, safe: true);


    private async Task<ApiResponseEnvelope> ExecuteInternalAsync(
        ApiClientCallPrototype proto,
        bool safe)
    {
        var envelope = new ApiResponseEnvelope
        {
            ErrorBodyType = proto.ErrorDtoType
        };

        try
        {
            _client.Timeout = proto.Timeout;

            var request = new HttpRequestMessage(proto.Method, proto.Url);

            foreach (var header in proto.Headers)
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (proto.Body != null)
            {
                string json = JsonSerializer.Serialize(proto.Body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var httpResponse = await _client.SendAsync(request);

            envelope.StatusCode = httpResponse.StatusCode;
            envelope.RawBody = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                envelope.Success = false;
                envelope.Error = $"HTTP {(int)httpResponse.StatusCode}: {envelope.RawBody}";

                if (proto.ErrorDtoType != null && !string.IsNullOrWhiteSpace(envelope.RawBody))
                {
                    envelope.ErrorDto = JsonSerializer.Deserialize(
                        envelope.RawBody,
                        proto.ErrorDtoType
                    );
                }

                return envelope;
            }

            envelope.Success = true;
            return envelope;
        }
        catch (Exception ex)
        {
            if (safe)
            {
                envelope.Success = false;
                envelope.Error = ex.Message;
                return envelope;
            }
            throw;
        }
    }
}