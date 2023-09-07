using System.Collections.Concurrent;

namespace Reformat.Framework.Temp.SSE.Domain;

public class SseUser
{
    public ConcurrentBag<Guid> PcIds { get; set; } = new ConcurrentBag<Guid>();
    public ConcurrentBag<Guid> MobileIds { get; set; } = new ConcurrentBag<Guid>();
}