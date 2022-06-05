namespace ET
{
    //账号信息组件负责保存客户端账号登陆信息
    [ComponentOf(typeof(Scene))]
    public class AccountInfoComponent : Entity,IAwake,IDestroy
    {
        public long AccountId;

        public string Token;
    }
}