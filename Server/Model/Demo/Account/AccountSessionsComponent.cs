using System.Collections.Generic;

namespace ET
{
    //账号通讯管理
    //存储客户端连接账号服务器时的通讯Session
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent : Entity,IAwake,IDestroy
    {
        //key=>acccount.Id  value=>session.instanceId
        public Dictionary<long, long> AccountSessionDictionary = new Dictionary<long, long>();
    }
}