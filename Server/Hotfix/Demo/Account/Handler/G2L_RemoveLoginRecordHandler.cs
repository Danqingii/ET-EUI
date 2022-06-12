using System;

namespace ET
{
    [ActorMessageHandler]
    public class G2L_RemoveLoginRecordHandler : AMActorRpcHandler<Scene,G2L_RemoveLoginRecord,L2G_RemoveLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_RemoveLoginRecord request, L2G_RemoveLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCentreLock,accountId.GetHashCode()))
            {
                long zone = scene.GetComponent<LoginInfoRecordComponent>().GetZone(accountId);
                if (request.ServerId == zone)
                {
                    scene.GetComponent<LoginInfoRecordComponent>().RemoveAccount(accountId);
                }
            }
            reply();
        }
    }
}