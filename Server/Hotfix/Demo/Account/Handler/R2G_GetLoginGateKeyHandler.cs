using System;

namespace ET
{
    [ActorMessageHandler]
    public class R2G_GetLoginGateKeyHandler : AMActorRpcHandler<Scene,R2G_GetLoginGateKey,G2R_GetLoginGateKey>
    {
        protected override async ETTask Run(Scene scene, R2G_GetLoginGateKey request, G2R_GetLoginGateKey response, Action reply)
        {
            if (scene.DomainScene().SceneType != SceneType.Gate)
            {
                Log.Error($"请求Scene错误,当前Scene为{scene.SceneType}");
                response.Error = ErrorCode.ERR_RequestSceneTypeError;
                reply();
                return;
            }

            string key = TimeHelper.ServerNow().ToString() + RandomHelper.RandInt64().ToString();
            scene.GetComponent<GateSessionKeyComponent>().Remove(request.AccountId);
            scene.GetComponent<GateSessionKeyComponent>().Add(request.AccountId,key);
            response.GateKey = key;
            reply();

            await ETTask.CompletedTask;
        }
    }
}