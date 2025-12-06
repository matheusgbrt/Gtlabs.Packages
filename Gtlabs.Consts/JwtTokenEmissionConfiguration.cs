namespace Gtlabs.Consts;

public class JwtTokenEmissionConfiguration
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}