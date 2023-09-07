using Reformat.Framework.Temp.SSE.Domain;

namespace Reformat.Framework.Temp.JWT.interfaces;

public interface IUserService
{
    /// <summary>
    /// 用于消息服务的用户列表
    /// </summary>
    
    public IReadOnlyDictionary<long, SseUser> GetUserMap();
    public List<Guid> GetSseIdByUserId(long userId, bool isMobile = false);
    public long GetUserIdBySseId(Guid SseId,bool isMobile = false);
    public Guid AddSseUser(long userId,bool isMobile = false);
    public long RemoveSseUser(Guid guid,bool isMobile = false);
    

    public IUser GetUserByToken(string token);

    public Task<IUser> GetUserById(long id);
    
    public Task<List<IUser>> GetUserByIds(long[] ids = null);

    //public Task<List<long>> GetAllUserIds();
}