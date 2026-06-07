using System.Diagnostics;
using Rebus.Pipeline;

namespace Gtlabs.ServiceBus.ServiceBus.Tracing;

internal static class RebusTracePropagation
{
    public const string ActivitySourceName = "Gtlabs.ServiceBus";

    private const string TraceParentHeaderName = "traceparent";
    private const string TraceStateHeaderName = "tracestate";
    private const string ActivityContextKey = "Gtlabs.ServiceBus.Activity";

    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public static void BeforeMessageSent(
        Dictionary<string, string> headers,
        object message,
        OutgoingStepContext context)
    {
        var activity = ActivitySource.StartActivity(
            $"Rebus send {message.GetType().Name}",
            ActivityKind.Producer);

        activity?.SetTag("messaging.system", "rebus");
        activity?.SetTag("messaging.operation", "send");
        activity?.SetTag("messaging.message.type", message.GetType().FullName);

        var propagationActivity = activity ?? Activity.Current;
        if (propagationActivity is not null)
        {
            headers[TraceParentHeaderName] = propagationActivity.Id!;

            if (!string.IsNullOrWhiteSpace(propagationActivity.TraceStateString))
            {
                headers[TraceStateHeaderName] = propagationActivity.TraceStateString;
            }
        }

        if (activity is not null)
        {
            context.Save(ActivityContextKey, activity);
        }
    }

    public static void AfterMessageSent(OutgoingStepContext context)
    {
        var activity = TryLoadActivity(context);
        activity?.Stop();
        activity?.Dispose();
    }

    public static void BeforeMessageHandled(
        Dictionary<string, string> headers,
        object message,
        IncomingStepContext context)
    {
        var activity = TryExtractParentContext(headers, out var parentContext)
            ? ActivitySource.StartActivity(
                $"Rebus process {message.GetType().Name}",
                ActivityKind.Consumer,
                parentContext)
            : ActivitySource.StartActivity(
                $"Rebus process {message.GetType().Name}",
                ActivityKind.Consumer);

        activity?.SetTag("messaging.system", "rebus");
        activity?.SetTag("messaging.operation", "process");
        activity?.SetTag("messaging.message.type", message.GetType().FullName);

        if (activity is not null)
        {
            context.Save(ActivityContextKey, activity);
        }
    }

    public static void AfterMessageHandled(IncomingStepContext context)
    {
        var activity = TryLoadActivity(context);
        activity?.Stop();
        activity?.Dispose();
    }

    private static Activity? TryLoadActivity(StepContext context)
    {
        try
        {
            return context.Load<Activity>(ActivityContextKey);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    private static bool TryExtractParentContext(
        IReadOnlyDictionary<string, string> headers,
        out ActivityContext parentContext)
    {
        parentContext = default;

        if (!headers.TryGetValue(TraceParentHeaderName, out var traceParent))
        {
            return false;
        }

        headers.TryGetValue(TraceStateHeaderName, out var traceState);
        return ActivityContext.TryParse(traceParent, traceState, out parentContext);
    }
}
