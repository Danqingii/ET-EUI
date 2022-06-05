namespace ET
{
    public enum AccountType
    {
        General,
        Blocked,
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