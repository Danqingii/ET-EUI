using System;

namespace ET
{
    [ActorMessageHandler]
    public class A2R_GetRealmKeyHandler : AMActorRpcHandler<Scene,A2R_GetRealmKey,R2A_GetRealmKey>
    {
        protected override async ETTask Run(Scene scene, A2R_GetRealmKey request, R2A_GetRealmKey response, Action reply)
        {
            if (scene.DomainScene().SceneType != SceneType.Realm)
            {
                Log.Error($"请求Scene错误,当前Scene为{scene.SceneType}");
                response.Error = ErrorCode.ERR_RequestSceneTypeError;
                reply();
                return;
            }

            string key = TimeHelper.ServerNow().ToString() + RandomHelper.RandInt64().ToString();
            scene.GetComponent<TokenComponent>().RemoveToken(request.AccountId);
            scene.GetComponent<TokenComponent>().AddToken(request.AccountId,key);
            response.RealmKey = key;
            reply();

            await ETTask.CompletedTask;
        }
    }
}