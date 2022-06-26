using System;

namespace ET
{
    public static class UnitCacheHelper
    {
        /// <summary>保存或者更新玩家缓存</summary>
        public static async ETTask AddOrUpdateUnitCache<T>(this T self) where T : Entity,IUnitCache
        {
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit(){UnitId = self.Id};
            message.EntityTypes.Add(typeof(T).FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(self));
            await MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(self.Id).InstanceId, message);
        }

        /// <summary>获取玩家缓存</summary>
        public static async ETTask<Unit> GetUnitCache(Scene scene, long unitId)
        {
            long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;
            Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit(){UnitId = unitId};
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, message);
            if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EntityList.Count <= 0)
            {
                return null;
            }

            int indexOf = queryUnit.ComponentNameList.IndexOf(nameof (Unit));
            Unit unit = queryUnit.EntityList[indexOf] as Unit;
            if (unit == null)
            {
                return null;
            }

            scene.AddChild(unit);
            foreach (Entity entity in queryUnit.EntityList)
            {
                if (entity == null || !(entity is Unit))
                {
                    continue;
                }
                unit.AddComponent(entity);
            }
            return unit;
        } 

        /// <summary>获取玩家组件缓存</summary>
        public static async ETTask<T> GetUnitComponentCache<T>(long unitId) where T : Entity,IUnitCache
        {
            Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit(){UnitId = unitId};
            message.ComponentNameList.Add(typeof(T).FullName);
            long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, message);
            if (queryUnit.Error == ErrorCode.ERR_Success && queryUnit.EntityList.Count > 0)
            {
                return queryUnit.EntityList[0] as T;
            }
            return null;
        }

        /// <summary>删除玩家缓存</summary>
        public static async ETTask DeleteUnitCache(long unitId)
        {
            Other2UnitCache_DeleteUnit message = new Other2UnitCache_DeleteUnit() { UnitId = unitId };
            await MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId, message);
        }

        /// <summary>保存Unit以及Unit身上组件到缓存服以及数据库中</summary>
        public static void AddOrUpdateUnitAllCache(Unit unit)
        {
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit(){UnitId = unit.Id};
            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(unit));
            
            foreach ((Type type,Entity entity) in unit.Components)
            {
                //如果类型不是继承自IUnitCache
                if (!typeof(IUnitCache).IsAssignableFrom(type))
                {
                    continue;
                }
                message.EntityTypes.Add(type.FullName);
                message.EntityBytes.Add(MongoHelper.ToBson(entity));
            }
            
            MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unit.Id).InstanceId, message).Coroutine();
        }
    }
}