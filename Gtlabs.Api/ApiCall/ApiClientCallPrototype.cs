namespace Gtlabs.Api.ApiCall;

public class ApiClientCallPrototype
{
    public string Url { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public HttpMethod Method { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public object? Body { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    public Type? ErrorDtoType { get; set; }
    
}