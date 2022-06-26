using System;

namespace ET
{
    /// <summary>
    /// 玩家网关下线
    /// </summary>
    [ActorMessageHandler]
    [FriendClass(typeof(Player))]
    [FriendClass(typeof(SessionPlayerComponent))]
    public class L2G_DisconnectGateUnitHandler : AMActorRpcHandler<Scene,L2G_DisconnectGateUnit,G2L_DisconnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate,accountId.GetHashCode()))
            {
                //gate映射
                Player player = scene.GetComponent<PlayerComponent>().Get(accountId);

                //如果gate服务器不存在该账号映射 就不用踢玩家下线了
                if (player == null)
                {
                    reply();
                    return;
                }
                
                //存在有player映射
                scene.GetComponent<PlayerComponent>().Remove(accountId);
                Session gateSession = Game.EventSystem.Get(player.SessionInstanceId) as Session;
                if (gateSession != null && !gateSession.IsDisposed)
                {
                    //!记录一下 现在是在顶号操作 防止下线有两种结果 
                    if (gateSession.GetComponent<SessionPlayerComponent>() != null)
                    {
                        gateSession.GetComponent<SessionPlayerComponent>().IsLoginAgain = true;
                    }
                    
                    
                    //重点! 
                    //这个gateSession身上其实有SessionPlayerComponent组件 触发销毁生命周期时 会踢玩家下线
                    gateSession.Send(new A2C_AccountDisconnect() {Error = ErrorCode.ERR_OtherAccountLogin });
                    gateSession?.Disconnect().Coroutine();
                }

                player.SessionInstanceId = 0;
                player.AddComponent<PlayerOffLineOutTimeComponent>();
            }

            reply();
        }
    }
}