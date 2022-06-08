namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        
        // 110000以下的错误请看ErrorCore.cs
        
        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
        
        public const int ERR_NetwordError = 200002; //网络错误
        public const int ERR_LoginNameOrPwNull = 200003;       //登陆账号或者密码为空
        public const int ERR_LoginAccountNameRule = 200004;    //登陆账号不符合规则
        public const int ERR_LoginPassWordRule = 200005;       //登陆密码不符合规则
        public const int ERR_AccountBlocked = 200006;          //账号已经被封禁
        public const int ERR_LoginPassWordError = 200007;      //登陆密码错误
        public const int ERR_RequestRepeat = 200008;           //重复请求
        public const int ERR_TokenError = 200009;              //令牌请求错误

        public const int ERR_RoleNameIsNull = 200010;    //角色名字为空
        public const int ERR_RoleNameIsNorm = 200011;    //角色名字长度不规范
        public const int ERR_RoleExist = 200012;         //角色已经存在了
    }
}