using System;

namespace ET
{
    /// <summary>
    /// 断开跟玩家通讯的Gate网关服
    /// </summary>
    [ActorMessageHandler]
    public class L2G_DisconnectGateUnitHandler : AMActorRpcHandler<Scene,L2G_DisconnectGateUnit,G2L_DisconnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GateLoginLock,accountId.GetHashCode()))
            {
                //其实这个player就是玩家在gate服务器上面的映射
                Player gateUnit = scene.GetComponent<PlayerComponent>().Get(accountId);

                //如果gate服务器不存在该账号映射 就不用踢玩家下线了
                if (gateUnit == null)
                {
                    reply();
                    return;
                }
                
                scene.GetComponent<PlayerComponent>().Remove(accountId);
                gateUnit.Dispose();
                //TODO
            }

            reply();
        }
    }
}