using Lib.AspNetCore.ServerSentEvents;
using Reformat.Framework.Temp.SSE.Config;
using Reformat.Framework.Temp.SSE.Core;
using Reformat.Framework.Temp.SSE.enums;
using Reformat.Framework.Temp.SSE.Services;

namespace Reformat.Framework.Temp.SSE.Extensions;

/// <summary>
/// 启动配置
/// </summary>
public static class ServicesExtensions
{
    private static SseImplType _sseImplType;
    
    private const string NOTIFICATIONS_SERVICE_TYPE_CONFIGURATION_KEY = "NotificationsService";
    private const string NOTIFICATIONS_SERVICE_TYPE_LOCAL = "Local";
    private const string NOTIFICATIONS_SERVICE_TYPE_REDIS = "Redis";

    /// <summary>
    /// 不做配置，默认使用本地（Local）
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static IServiceCollection AddServerSentEvent(this IServiceCollection services,SseImplType sseType = SseImplType.LOCAL)
    {
        services.AddServerSentEvents();
        services.AddServerSentEventsClientIdProvider<ClientIdProvider>();
        services.AddSingleton<IHostedService, HeartbeatService>();
        services.AddInMemoryServerSentEventsNoReconnectClientsIdsStore();

        _sseImplType = sseType;
        switch (sseType)
        {
            case SseImplType.LOCAL:
            {
                services.AddServerSentEvents<IServerSendEvent, SseLocalImpl>(options =>
                {
                    options.ReconnectInterval = 5000;
                });
                break;   
            }
            case SseImplType.REDIS:
            {
                services.AddServerSentEvents<IServerSendEvent, SseRedisImpl>(options =>
                {
                    options.ReconnectInterval = 5000;
                });
                break;   
            }
            case SseImplType.NET:
            {
                services.AddServerSentEvents<IServerSendEvent, SseNetImpl>(options =>
                {
                    options.ReconnectInterval = 5000;
                });
                break;
            }
        }

        // 根据配置文件选择不同的策略
        // string notificationsServiceType = configuration.GetValue(NOTIFICATIONS_SERVICE_TYPE_CONFIGURATION_KEY, NOTIFICATIONS_SERVICE_TYPE_LOCAL);
        //
        // if (notificationsServiceType.Equals(NOTIFICATIONS_SERVICE_TYPE_LOCAL, StringComparison.InvariantCultureIgnoreCase))
        // {
        //     services.AddTransient<INotificationsService, LocalNotificationsService>();
        // }
        // else if (notificationsServiceType.Equals(NOTIFICATIONS_SERVICE_TYPE_REDIS, StringComparison.InvariantCultureIgnoreCase))
        // {
        //     services.AddSingleton<INotificationsService, RedisNotificationsService>();
        // }
        // else
        // {
        //     throw new NotSupportedException($"Not supported {nameof(INotificationsService)} type.");
        // }
        return services;
    }

    public static void AddServerSentEvent(this WebApplication app,string accessPath)
    {
        switch (_sseImplType)
        {
            case SseImplType.LOCAL:
            {
                app.MapServerSentEvents<SseLocalImpl>(accessPath);
                break;   
            }
            case SseImplType.REDIS:
            {
                app.MapServerSentEvents<SseRedisImpl>(accessPath);
                break;   
            }
            case SseImplType.NET:
            {
                app.MapServerSentEvents<SseNetImpl>(accessPath);
                break;
            }
        }
    }
}
