using System;

namespace ET
{
    [MessageHandler]
    public class C2R_LoginRealmHandler : AMRpcHandler<C2R_LoginRealm,R2C_LoginRealm>
    {
        protected override async ETTask Run(Session session, C2R_LoginRealm request, R2C_LoginRealm response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Realm)
            {
                Log.Error($"请求Scene错误,当前Scene为{session.DomainScene().SceneType}");
                response.Error = ErrorCode.ERR_RequestSceneTypeError;
                reply();
                return;
            }

            //重复请求登陆Realm
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeat;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            string token = session.DomainScene().GetComponent<TokenComponent>().GetToken(request.AccountId);

            //令牌错误
            if (string.IsNullOrEmpty(token) || token != request.RealmKey)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }
            
            //令牌通过,删除掉,因为后续玩家不会在连接realm
            session.DomainScene().GetComponent<TokenComponent>().RemoveToken(request.AccountId);

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginRealm,request.AccountId.GetHashCode()))
                {
                    //取模固定分配一个Gate
                    StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainScene().Zone, request.AccountId);

                    //通知gate服拿一个登陆令牌
                    G2R_GetLoginGateKey g2RGetLoginGateKey =
                            (G2R_GetLoginGateKey) await MessageHelper.CallActor(config.InstanceId, new R2G_GetLoginGateKey() { AccountId = request.AccountId});

                    if (g2RGetLoginGateKey.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = g2RGetLoginGateKey.Error;
                        reply();
                        return;
                    }
                    
                    //把登陆令牌跟gate连接地址返回客户端
                    response.GateKey = g2RGetLoginGateKey.GateKey;
                    response.GateAddress = config.OuterIPPort.ToString();

                    reply();
                    session?.Disconnect().Coroutine();
                }
            }
        }
    }
}