using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 令牌组件 --注意现在每个令牌都要10分钟过期时间
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class TokenComponent : Entity,IAwake,IDestroy
    {
        // key=>account.Id  value=>token
        public Dictionary<long, string> TokenDict = new Dictionary<long, string>();
    }
}