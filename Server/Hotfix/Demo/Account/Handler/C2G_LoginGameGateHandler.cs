using System;

namespace ET
{
    [MessageHandler]
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(Player))]
    [FriendClass(typeof(SessionStateComponent))]
    public class C2G_LoginGameGateHandler : AMRpcHandler<C2G_LoginGameGate,G2C_LoginGameGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGameGate request, G2C_LoginGameGate response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Gate)
            {
                Log.Error($"请求Scene错误,当前Scene为{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            
            //重复请求登陆gate
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeat;
                reply();
                return;
            }
            
            string token = session.DomainScene().GetComponent<GateSessionKeyComponent>().Get(request.AccountId);

            //令牌验证失败了
            if (token == null || token != request.GateKey)
            {
                response.Error = ErrorCode.ERR_TokenError;
                response.Message = "Gate Key验证失败";
                reply();
                session?.Disconnect().Coroutine();
                return;
            }
            
            session.DomainScene().GetComponent<GateSessionKeyComponent>().Remove(request.AccountId);

            long instanceId = session.InstanceId;
            using (session.AddComponent<SessionLockingComponent>())
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate,request.AccountId.GetHashCode()))
            {
                if (session.InstanceId != instanceId)
                {
                    return;
                }
                
                //通知登陆中心服 记录本次登陆的服务器zone
                StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
                L2G_AddLoginRecord l2GAddLoginRecord = (L2G_AddLoginRecord) await MessageHelper.CallActor(loginCenterConfig.InstanceId, new G2L_AddLoginRecord()
                {
                    AccountId = request.AccountId,
                    ServerId = session.DomainZone(),
                });

                if (l2GAddLoginRecord.Error != ErrorCode.ERR_Success)
                {
                    response.Error = l2GAddLoginRecord.Error;
                    reply();
                    session?.Disconnect().Coroutine();
                    return;
                }

                //改变session状态
                SessionStateComponent sessionStateComponent = session.GetComponent<SessionStateComponent>();
                if (sessionStateComponent == null)
                {
                    sessionStateComponent = session.AddComponent<SessionStateComponent>();
                }
                sessionStateComponent.State = SessionState.Normal;
                
                
                //TODO 还没怎么看懂
                //尝试得到玩家gate映射
                Player player = session.DomainScene().GetComponent<PlayerComponent>().Get(request.AccountId);

                if (player == null)
                {
                    //映射不存在 添加一个新的GateUnit
                    player = session.DomainScene().GetComponent<PlayerComponent>().AddChildWithId<Player, long, long>(request.RoleId,request.AccountId,request.RoleId);
                    player.PlayerState = PlayerState.Gate;
                    session.DomainScene().GetComponent<PlayerComponent>().Add(player);
                    session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                }
                else
                {
                    player.RemoveComponent<PlayerOffLineOutTimeComponent>();
                }

                session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
                session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
                session.GetComponent<SessionPlayerComponent>().AccountId = request.AccountId;
                player.SessionInstanceId = session.InstanceId;
            }
            reply();
        }
    }
}