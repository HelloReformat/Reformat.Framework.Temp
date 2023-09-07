using Lib.AspNetCore.ServerSentEvents;
using Reformat.Framework.Core.Common.Extensions.Collections;
using Reformat.Framework.Core.IOC.Attributes;
using Reformat.Framework.Core.IOC.Services;
using Reformat.Framework.Temp.JWT.interfaces;

namespace Reformat.Framework.Temp.SSE.Config;

/// <summary>
/// 客户端ID提供者
/// 用于制定用户隔离策略
/// </summary>
public class ClientIdProvider : IServerSentEventsClientIdProvider
{
    [Autowired]
    protected IUserService userService;
    
    public ClientIdProvider(IocSingle iocSingle)
    {
        iocSingle.Autowired(this);
    }

    /// <summary>
    /// 请求连接时，设置客户端ID
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Guid AcquireClientId(HttpContext context)
    {

        context.Request.Query.TryGetValue("token", out var token);
#if DEBUG
        if (token.IsNullOrEmpty())
        {
            token = "dev";
        }
#endif
        if (token.Count == 0)
        {
            throw new Exception("token不能为空");
        }
        IUser user = userService.GetUserByToken(token);
        long userId = user != null ? user.Id : -1;
        Guid clientId = userService.AddSseUser(userId);
        string info = $"用户:{userId} 客户端SSE：{clientId} 开始连接";
        Console.WriteLine(info);
        return clientId;
    }

    /// <summary>
    /// 断开连接时的操作
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="context"></param>
    public void ReleaseClientId(Guid clientId, HttpContext context)
    {
        var token = context.Request.Headers["token"].FirstOrDefault("-1");
        long userId = userService.RemoveSseUser(clientId);
        string info = $"用户:{userId} 客户端SSE：{clientId} 断开连接";
        Console.WriteLine(info);
    }
}