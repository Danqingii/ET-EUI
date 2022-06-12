namespace ET
{
    /// <summary>
    /// 踢玩家下线 准备时间
    /// </summary>
    [ComponentOf(typeof(Player))]
    public class PlayerOffLineOutTimeComponent : Entity,IAwake,IDestroy
    {
        public long Timer;
    }
}