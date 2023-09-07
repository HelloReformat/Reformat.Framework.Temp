using System.ComponentModel;

namespace Reformat.Framework.Temp.SSE.Constant;

public enum NoticeTag
{
    /// <summary>
    /// 弹出提醒
    /// </summary>
    [Description("弹出提醒")]
    ALERT,
    /// <summary>
    /// 系统提醒
    /// </summary>
    [Description("系统提醒")]
    ALARM,
    /// <summary>
    /// 业务提醒
    /// </summary>
    [Description("业务提醒")]
    BUSSINESS,
    /// <summary>
    /// 平台提醒
    /// </summary>
    [Description("平台提醒")]
    PLATFORM,
    /// <summary>
    /// 历史消息
    /// </summary>
    [Description("历史消息")]
    HISTORY
}