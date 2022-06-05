namespace ET
{
    //账号检测外置时间组件 
    //案例:如果玩家登陆成功了,但是在大厅一直停留,不进入Map服务器 10分钟后 把该玩家踢下线
    //关机 / 杀死进程 这些都可能触发该组件
    [ComponentOf(typeof(Session))]
    public class AccountCheckOutTimeComponent : Entity,IAwake<long>,IDestroy
    {
        public long AccountId;
        public long Timer;
    }
}