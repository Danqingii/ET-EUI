namespace ET
{
    public enum AccountType
    {
        General,   //一般玩家
        Blocked,   //账号封禁
    }
    
    //账号实体
    public class Account : Entity,IAwake
    {
        public string AccountName;

        public string PassWord;

        public long CreateTimer;

        public int AccountType;
    }
}