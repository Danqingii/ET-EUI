using System.Collections.Generic;

namespace ET
{
    public interface IUnitCache
    {

    }

    [ComponentOf(typeof(UnitCacheComponent))]
    public class UnitCache : Entity,IAwake,IDestroy
    {
        public string Key;

        public Dictionary<long, Entity> CacheComponentDictionary = new Dictionary<long, Entity>();
    }
}