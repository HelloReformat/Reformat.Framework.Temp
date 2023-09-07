# SSE (Server-Send Events) 开发指南

## 版本说明

**20230626：**
1. 命名优化
2. 启动优化

**20230608：**

1. 完成基础通信机制
2. 完成心跳检测机制
3. 完成断线重连机制
4. 完成用户关系映射
5. 完成用户在线监控



## 一、基本介绍

1. SSE 为 单向通信机制，允许服务器在P/S计算模型中向客户端推送消息
2. 该功能基于 `Lib.AspNetCore.ServerSentEvents` 开源组件，进行的二次开发及封装
3. 当前尚未实现消息队列，短期内将会通过本地缓存实现 P&B 功能，后续可接入 消息队列中间件或者 消息服务平台



## 二、后端示例

**启动配置**`（ Program.cs）`

```C#
// 完成服务注入
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServerSentEvent();
...
// 配置连接地址
var app = builder.Build();
app.AddServerSentEvent("/notifications");
```

**使用示例**

```C#
[ApiController]
[Route("[controller]/[action]")]
public class NoticeController : BaseController
{

    public NoticeController(IocScoped iocService) : base(iocService)
    {
    }
    
    [HttpGet]
    public bool IsOnline(long userId)
    { 
        return NoticeHelper.IsOnline(userId);
    }
    
    [HttpGet]
    public List<long> OnlineUserIds()
    {
        return NoticeHelper.GetOnlineUserIds();
    }

    [HttpGet]
    public string UserMap()
    {
        IReadOnlyDictionary<long,Guid> readOnlyDictionary = NoticeHelper.GetUserMap();
        StringBuilder sb = new StringBuilder();
        readOnlyDictionary.ToList().ForEach(i =>
        {
            sb.AppendLine($"UserId:{i.Key.ToString()} : SseId:{i.Value.ToString()}");
        });
        return sb.ToString();
    }

    [HttpGet]
    public async Task<string> SendAsyncByUserId(long userId,string msg)
    {
        var msgId = await NoticeHelper.SendAsyncByUserId(userId,NoticeType.ALERT,msg);
        return $"消息ID：{msgId}";
    }
    
    [HttpGet]
    public async Task<string> SendAll(string msg)
    { 
        var msgId = await NoticeHelper.SendAll(NoticeType.ALERT,msg);
        return $"消息ID：{msgId}";
    }
}
```

**注意事项**

+ 该功能强依赖于`IUserService`，程序必须对该接口进行实现，且注入IOC容器
+ 所有SSE过程中的调用的实例均为单例，若有多管道隔离需求，需重新进行定制开发
+ 不要对 `NoticeService` 及相关实现类（eg：LocalNotificationsService）直接进行操作，请使用**辅助工具类** `（NoticeHelper）`
+ 本模块的设计已完成 ”用户&客户端映射“ 功能进行解耦并下放至 `IUserService`的具体实现，可针对不同项目进行定制
+ 不要推送


## 三、前端示例

```js
// 创建持久化链接，TX格式为 event-stream
var source = new EventSource('/notifications');

source.addEventListener('ALERT', function(event) {
    // 相关业务操作....
    alert(event.lastEventId) 
    alert(event.data);
});
```

**Warining：**

+ 当前程序仅支持单页应用且不支持多端登录链接，即一个用户只能在一个客户端进行登录
+ 实例代码 中 的 “ALERT” 为消息类型，由后端制定。当前提供  `ALERT` `SYSTEM`  `BUSSINESS`三种类型。
+ `event.lastEventId` 为 消息ID 可像后端请求做 “已读” 销毁
+  `event.data` 为具体的消息内容，格式为JSON类型，可根据业务需求进行定制
+ 若存在多端需求，请联系后端进行改造



## 四、调试指南

![1.png](https://s2.loli.net/2023/06/08/FZdPMDztgrqxy8R.png)

![Snipaste_2023-06-08_20-18-38.png](https://s2.loli.net/2023/06/08/iUlykZP6neQIATa.png)

## 五、扩展应用

+ 超时提醒
+ 二维码时效性
+ 支付码防窃
+ 门户千人千面
+ 动态开屏广告
+ 在线统计，后台踢人
+ .............