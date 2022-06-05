using System;

namespace ET
{
    //保存玩家登陆账号的处理者
    [ActorMessageHandler]
    public class A2L_LoginAccountRecordRequestHandler : AMActorRpcHandler<Scene,A2L_LoginAccountRecord,L2A_LoginAccountRecord>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRecord request, L2A_LoginAccountRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCentreLock,accountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExistAccount(accountId))
                {
                    reply();
                    return;
                }
                
                //登陆中心服存有玩家账号信息,就踢该次玩家下线
                //先得到该玩家连接的gate网关服务器
                //让gate网关服务器发送下线消息
                int zone = scene.GetComponent<LoginInfoRecordComponent>().GetZone(accountId);
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone,accountId);
                var g2lDisconnectGateUnit = (G2L_DisconnectGateUnit) await MessageHelper.CallActor(gateConfig.InstanceId,new L2G_DisconnectGateUnit() { AccountId = accountId });

                response.Error = g2lDisconnectGateUnit.Error;
                reply();
            }
        }
    }
}