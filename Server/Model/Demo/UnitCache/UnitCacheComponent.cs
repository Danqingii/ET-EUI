using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 数据缓存服组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    [ChildType(typeof(UnitCache))]
    public class UnitCacheComponent : Entity,IAwake,IDestroy
    {
        public Dictionary<string, UnitCache> UnitCaches = new Dictionary<string, UnitCache>();

        public List<string> UnityCacheKeyList = new List<string>();
    }
}