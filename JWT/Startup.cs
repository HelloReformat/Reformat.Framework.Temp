using Reformat.Framework.Temp.JWT.interfaces;

namespace Reformat.Framework.Temp.JWT;

/// <summary>
/// 启动配置
/// </summary>
public static class Startup
{
    public static void AddCustomUser<T>(this WebApplicationBuilder builder) where T : class, IUserService
    {
        builder.Services.AddSingleton<IUserService,T>();
    }
}
