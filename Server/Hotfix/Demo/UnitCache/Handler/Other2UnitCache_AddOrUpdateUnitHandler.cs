using System;

namespace ET
{
    [ActorMessageHandler]
    public class Other2UnitCache_AddOrUpdateUnitHandler : AMActorRpcHandler<Scene,Other2UnitCache_AddOrUpdateUnit,UnitCache2Other_AddOrUpdateUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response, Action reply)
        {
            UpdateUnitCacheAsync(scene, request, response).Coroutine();
            reply();
            await ETTask.CompletedTask;
        }

        private static async ETTask UpdateUnitCacheAsync(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            using (ListComponent<Entity> entityList = ListComponent<Entity>.Create())
            {
                for (int i = 0; i < request.EntityTypes.Count; i++)
                {
                    Type type = Game.EventSystem.GetType(request.EntityTypes[i]);
                    Entity entity = MongoHelper.FromBson(type, request.EntityBytes[i]) as Entity;
                    entityList.Add(entity);
                }
                await unitCacheComponent.AddOrUpdate(request.UnitId, entityList);
            }
        }
    }
}