using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Reformat.Framework.Core.IOC.Attributes;
using Reformat.Framework.Core.IOC.Services;
using Reformat.Framework.Temp.JWT.interfaces;
using Reformat.Framework.Temp.SSE.Core;
using Reformat.Framework.Temp.SSE.Interfaces;

namespace Reformat.Framework.Temp.SSE.Services;

public class SseLocalImpl : IServerSendEvent,INotificationsService
{
    [Autowired] private IMemoryCache Cache;

    [Autowired] 
    private IUserService userService; 
    
    public SseLocalImpl(IOptions<ServerSentEventsServiceOptions<ServerSentEventsService>> Options,IocSingle iocSingle) : base(Options)
    {
        iocSingle.Autowired(this);
    }

    public override IUserService GetUserService() => userService;

    public override void Subscribe(Guid token, int timeoutSecs = 300)
    {
        throw new NotImplementedException();
    }

    public override Task Publish(Guid token, object data)
    {
        throw new NotImplementedException();
    }

    public override async Task SendAsyncById(long id,ServerSentEvent data)
    {
        List<Guid> clentIds = userService.GetSseIdByUserId(id);
        if (clentIds.Count == 0)
        {
            Console.WriteLine($"未能找到用户Id:{id}对应的SSE通信ID");
            throw new Exception("未能找到用户Id对应的SSE通信ID");
        }
        
        IEnumerable<IServerSentEventsClient> clients = this.GetClients().Where(O=> clentIds.Contains(O.Id));
        foreach (var client in clients)
        {
            Console.WriteLine($"向用户Id:{id} SSE通信ID:{client}发送消息");
            await client.SendEventAsync(data);
        }
    }
}
