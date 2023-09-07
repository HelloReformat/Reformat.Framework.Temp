using Lib.AspNetCore.ServerSentEvents;
using Newtonsoft.Json;
using Reformat.Framework.Core.IOC;
using Reformat.Framework.Temp.SSE.Constant;
using Reformat.Framework.Temp.SSE.Core;
using Reformat.Framework.Temp.SSE.Domain;

namespace Reformat.Framework.Temp.SSE;

public static class SseHelper
{
    private static IServerSendEvent ServerSendEvent;

    static SseHelper()
    {
        ServerSendEvent = ServiceLocator.GetService<IServerSendEvent>();
    }
    
    /// <summary>
    /// 获取在线用户Id列表
    /// </summary>
    /// <returns></returns>
    public static List<long> GetOnlineUserIds()
    {
        List<long> userIds = new List<long>();
        foreach (var client in ServerSendEvent.GetClients())
        {
            userIds.Add(ServerSendEvent.GetUserService().GetUserIdBySseId(client.Id));
        }
        return userIds.Distinct().ToList();
    }
    
    /// <summary>
    /// 根据UserId判断用户是否在线
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static Boolean IsOnline(long userId)
    {
        var userMap = GetUserMap();
        if (!userMap.ContainsKey(userId)) return false;
        return ServerSendEvent.GetClients().Any(O=>O.Id.Equals(userMap[userId]));
    }
    
    /// <summary>
    /// 获取用户Id与SseId的映射
    /// </summary>
    /// <returns></returns>
    public static IReadOnlyDictionary<long, SseUser> GetUserMap()
    {
        return ServerSendEvent.GetUserService().GetUserMap();
    }
    
    /// <summary>
    /// 对指定用户发送消息
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="noticeType">消息类型</param>
    /// <param name="data">传递的数据</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<string> SendAsyncByUserId<T>(long userId,NoticeTag noticeTag,T data)
    {
        // TODO:需要替换成支持分布式的雪花算法
        // TODO:替换成GRPC的json解析器增加性能
        string id = Guid.NewGuid().ToString();
        string json = JsonConvert.SerializeObject(data);

        ServerSentEvent serverSentEvent = new ServerSentEvent()
        {
            Id = id,
            Type = noticeTag.ToString(),
            Data = new List<string>(){json},
        };
        await ServerSendEvent.SendAsyncById(userId,serverSentEvent);
        return id;
    }
    
    /// <summary>
    /// 向全员发送消息
    /// </summary>
    /// <param name="noticeType">消息类型</param>
    /// <param name="data">传递的数据</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<string> SendAll<T>(NoticeTag noticeTag,T data)
    {
        string id = Guid.NewGuid().ToString();
        string json = JsonConvert.SerializeObject(data);

        ServerSentEvent serverSentEvent = new ServerSentEvent()
        {
            Id = id,
            Type = noticeTag.ToString(),
            Data = new List<string>(){json},
        };
        await ServerSendEvent.SendEventAsync(serverSentEvent);
        return id;
    }
}