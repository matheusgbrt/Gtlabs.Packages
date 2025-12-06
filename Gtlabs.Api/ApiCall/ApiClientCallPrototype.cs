using Gtlabs.Api.ApiCall.Tokens;

namespace Gtlabs.Api.ApiCall;

public class ApiClientCallPrototype
{
    public string Url { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public HttpMethod Method { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public object? Body { get; set; }
    private CallTimeout _timeoutEnum;
    public CallTimeout TimeoutEnum
    {
        get => _timeoutEnum;
        set
        {
            _timeoutEnum = value;
            Timeout = TimeSpan.FromSeconds((int)value);
        }
    }

    public TimeSpan Timeout { get; private set; }
    public Type? ErrorDtoType { get; set; }
    
}