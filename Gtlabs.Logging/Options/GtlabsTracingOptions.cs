namespace Gtlabs.Logging.Options;

public class GtlabsTracingOptions
{
    public string ServiceName { get; set; } = string.Empty;
    public Uri? OtlpEndpoint { get; set; }
    public bool ExportToOtlp { get; set; } = true;
}
