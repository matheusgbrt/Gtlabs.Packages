using Gtlabs.AmbientData.Interfaces;
using Gtlabs.Authentication.Providers;
using Gtlabs.Authentication.Services;
using Gtlabs.Consts.Authentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gtlabs.Authentication.BackgroundJobs;

public class JwtCacheUpdater  : BackgroundService
{
    private readonly ILogger<JwtCacheUpdater> _logger;
    private readonly IAppAuthorizationProvider _appAuthorizationProvider;
    private readonly IAmbientData _ambientData;
    private readonly IAppAuthService _appAuthService;
    private readonly IJwtEmissorService _jwtEmissorService;
    private readonly IOptions<AuthenticationHeaderOptions> _authHeaderOptions;

    public JwtCacheUpdater(ILogger<JwtCacheUpdater> logger, 
        IJwtEmissorService jwtEmissorService, 
        IAppAuthorizationProvider appAuthorizationProvider, 
        IAmbientData ambientData, 
        IAppAuthService appAuthService,
        IOptions<AuthenticationHeaderOptions> authHeaderOptions)
    {
        _logger = logger;
        _jwtEmissorService = jwtEmissorService;
        _appAuthorizationProvider = appAuthorizationProvider;
        _ambientData = ambientData;
        _appAuthService = appAuthService;
        _authHeaderOptions = authHeaderOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(50));

        if (!_authHeaderOptions.Value.RegisterBackgroundService)
            return;
        while (!stoppingToken.IsCancellationRequested)
        {
            await RefreshToken();

            if (!await timer.WaitForNextTickAsync(stoppingToken))
                break;
        }
    }
    
    private async Task RefreshToken()
    {
        var appId = _ambientData.GetAppId();
        var apiResponse = await _appAuthorizationProvider.GetAppPermissionAsync(appId);
        var permissions = apiResponse.MapTo<AppPermission>();
        var token = _jwtEmissorService.GenerateAppPermissionToken(permissions);
        await _appAuthService.SetAppToken(token);
    }
}