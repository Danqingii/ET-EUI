using System;

namespace ET
{
    [ActorMessageHandler]
    public class G2L_AddLoginRecordHandler : AMActorRpcHandler<Scene,G2L_AddLoginRecord,L2G_AddLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_AddLoginRecord request, L2G_AddLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCentreLock,accountId.GetHashCode()))
            {
                scene.GetComponent<LoginInfoRecordComponent>().RemoveAccount(accountId);
                scene.GetComponent<LoginInfoRecordComponent>().AddAccount(accountId,request.ServerId);
            }
            reply();
        }
    }
}