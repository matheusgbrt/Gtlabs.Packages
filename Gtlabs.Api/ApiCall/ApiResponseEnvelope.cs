using System.Net;
using System.Text.Json;

namespace Gtlabs.Api.ApiCall;

public class ApiResponseEnvelope
{
    public object? ErrorDto { get; internal set; }
    public bool Success { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string RawBody { get; set; } = string.Empty;
    public string? Error { get; set; }
    internal Type? ErrorBodyType { get; set; }

    public TResult MapTo<TResult>()
    {
        return JsonSerializer.Deserialize<TResult>(RawBody)!;
    }
    public List<T> MapToList<T>()
    {
        return JsonSerializer.Deserialize<List<T>>(RawBody)!;
    }
    
    public T MapErrorTo<T>()
    {
        if (ErrorDto is T typed)
            return typed;

        return JsonSerializer.Deserialize<T>(RawBody)!;
    }
}