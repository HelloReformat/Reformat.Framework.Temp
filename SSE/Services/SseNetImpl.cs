using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;
using Reformat.Framework.Core.IOC.Services;
using Reformat.Framework.Temp.JWT.interfaces;
using Reformat.Framework.Temp.SSE.Core;
using Reformat.Framework.Temp.SSE.Interfaces;

namespace Reformat.Framework.Temp.SSE.Services;

/// <summary>
/// 用于第三方和分布式 消息平台的通知
/// </summary>
public class SseNetImpl : IServerSendEvent,INotificationsService
{

    public SseNetImpl(IOptions<ServerSentEventsServiceOptions<ServerSentEventsService>> Options, IocSingle iocSingle) : base(Options)
    {
    }

    public override IUserService GetUserService()
    {
        throw new NotImplementedException();
    }

    public override void Subscribe(Guid token, int timeoutSecs = 300)
    {
        throw new NotImplementedException();
    }

    public override Task Publish(Guid token, object data)
    {
        throw new NotImplementedException();
    }

    public override Task SendAsyncById(long id, ServerSentEvent data)
    {
        throw new NotImplementedException();
    }
}