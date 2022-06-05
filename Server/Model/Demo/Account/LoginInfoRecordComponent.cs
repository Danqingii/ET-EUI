using System.Collections.Generic;

namespace ET
{
    //登陆信息记录 组件
    //主要用于登陆中心服记录玩家登陆信息
    [ComponentOf(typeof(Scene))]
    public class LoginInfoRecordComponent : Entity,IAwake,IDestroy
    {
        //key = account.Id  value = zone
        public Dictionary<long, int> AccountLoginInfoDict = new Dictionary<long, int>();
    }
}