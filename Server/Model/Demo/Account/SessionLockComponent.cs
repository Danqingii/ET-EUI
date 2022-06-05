namespace ET
{
    //防止玩家登陆时请求多条信息
    [ComponentOf(typeof(Session))]
    public class SessionLockComponent : Entity,IAwake
    {
        
    }
}