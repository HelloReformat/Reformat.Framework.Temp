using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;
using Reformat.Framework.Temp.JWT.interfaces;

namespace Reformat.Framework.Temp.SSE.Core;

/// <summary>
/// SSE服务基类（不可省略）
/// 注入时使用：services.AddServerSentEvents<NotionsService, LocalNotificationsService>（）
/// </summary>
public abstract class IServerSendEvent :  ServerSentEventsService, IServerSentEventsService
{
    protected IServerSendEvent(IOptions<ServerSentEventsServiceOptions<ServerSentEventsService>> Options) : base(Options)
    {
    }

    public abstract IUserService GetUserService();
    public abstract void Subscribe(Guid token, int timeoutSecs = 300);
    public abstract Task Publish(Guid token, object data);
    public abstract Task SendAsyncById(long id, ServerSentEvent data);
}
