using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;
using Reformat.Framework.Temp.JWT.interfaces;
using Reformat.Framework.Temp.SSE.Core;
using Reformat.Framework.Temp.SSE.Interfaces;
using StackExchange.Redis;

namespace Reformat.Framework.Temp.SSE.Services;

public class SseRedisImpl : IServerSendEvent,INotificationsService
{
    private const string CONNECTION_MULTIPLEXER_CONFIGURATION_KEY = "RedisConnectionMultiplexerConfiguration";
    private const string NOTIFICATIONS_CHANNEL = "NOTIFICATIONS";
    private const string ALERTS_CHANNEL = "ALERTS";
    private ConnectionMultiplexer _redis;
    
    public SseRedisImpl(IOptions<ServerSentEventsServiceOptions<ServerSentEventsService>> Options, IConfiguration Configuration) : base(Options)
    {
        _redis = ConnectionMultiplexer.Connect(Configuration.GetValue<String>(CONNECTION_MULTIPLEXER_CONFIGURATION_KEY));
        ISubscriber subscriber = _redis.GetSubscriber();
        subscriber.Subscribe(NOTIFICATIONS_CHANNEL, async (channel, message) => { await SendNotificationAsync(message, false); });
        subscriber.Subscribe(ALERTS_CHANNEL, async (channel, message) => { await SendNotificationAsync(message, true); });
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
        // ISubscriber subscriber = _redis.GetSubscriber();
        // return subscriber.PublishAsync(alert ? ALERTS_CHANNEL : NOTIFICATIONS_CHANNEL, JsonConvert.SerializeObject(data));
    }

    public Task SendNotificationAsync(object notification, bool alert)
    {
        throw new NotImplementedException();
    }
}
