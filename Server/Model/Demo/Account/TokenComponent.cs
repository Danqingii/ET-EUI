using System.Collections.Generic;

namespace ET
{
    //令牌组件
    //保存令牌
    [ComponentOf(typeof(Scene))]
    public class TokenComponent : Entity,IAwake,IDestroy
    {
        // key = account.Id  value = token
        public Dictionary<long, string> TokenDictionary = new Dictionary<long, string>();
    }
}