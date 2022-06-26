namespace ET
{
    public class UnitCacheDestroySystem : DestroySystem<UnitCache>
    {
        public override void Destroy(UnitCache self)
        {
            foreach (var entity in self.CacheComponentDictionary.Values)
            {
                entity.Dispose();
            }
            self.CacheComponentDictionary.Clear();
            self.Key = null;
        }
    }

    [FriendClass(typeof(UnitCache))]
    public static class UnitCacheSystem
    {
        public static void AddOrUpdate(this UnitCache self,Entity entity)
        {
            if (entity == null || entity.IsDisposed)
            {
                return;
            }

            if (!self.CacheComponentDictionary.TryGetValue(entity.Id,out Entity oldEntity))
            {
                if (entity != oldEntity)
                {
                    oldEntity.Dispose();
                }

                self.CacheComponentDictionary.Remove(entity.Id);
            }
            
            self.CacheComponentDictionary.Add(entity.Id,entity);
        }

        public static async ETTask<Entity> Get(this UnitCache self,long unitId)
        {
            Entity entity = null;
            if (!self.CacheComponentDictionary.TryGetValue(unitId,out entity))
            {
                entity = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<Entity>(unitId,self.Key);
                if (entity != null)
                {
                    self.AddOrUpdate(entity);
                }
            }
            return entity;
        }

        public static void Delete(this UnitCache self,long unitId)
        {
            if (self.CacheComponentDictionary.TryGetValue(unitId, out Entity entity))
            {
                entity.Dispose();
                self.CacheComponentDictionary.Remove(unitId);
            }
        }
    }
}