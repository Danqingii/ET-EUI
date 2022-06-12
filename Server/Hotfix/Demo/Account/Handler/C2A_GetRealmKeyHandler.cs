using System;

namespace ET
{
    [MessageHandler]
    public class C2A_GetRealmKeyHandler : AMRpcHandler<C2A_GetRealmKey,A2C_GetRealmKey>
    {
        protected override async ETTask Run(Session session, C2A_GetRealmKey request, A2C_GetRealmKey response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求Scene错误,当前Scene为{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            //重复请求创建角色
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeat;
                reply();
                return;
            }
            
            string token = session.DomainScene().GetComponent<TokenComponent>().GetToken(request.AccountId);

            //令牌验证失败了
            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount,request.AccountId.GetHashCode()))
                {
                    //取模Realm
                    StartSceneConfig realmSceneConfig =  RealmGateAddressHelper.GetRealm(request.ServerId);

                    //通知Realm分配一个进入Realm的令牌
                    R2A_GetRealmKey r2AGetRealmKey = (R2A_GetRealmKey) await MessageHelper.CallActor(realmSceneConfig.InstanceId, new A2R_GetRealmKey() {AccountId = request.AccountId});

                    //如果请求Realm不通过 为啥要断开session
                    if (r2AGetRealmKey.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = r2AGetRealmKey.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        return;
                    }

                    response.RealmKey = r2AGetRealmKey.RealmKey;
                    response.RealmAddress = realmSceneConfig.OuterIPPort.ToString();
                    reply();
                    
                    //因为当前是Realm所有可以直接断开
                    session?.Disconnect().Coroutine();
                }
            }
            
            await ETTask.CompletedTask;
        }
    }
}