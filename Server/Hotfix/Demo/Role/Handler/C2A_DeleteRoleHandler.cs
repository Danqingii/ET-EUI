using System;
using System.Collections.Generic;

namespace ET
{
    [MessageHandler]
    [FriendClass(typeof(RoleInfo))]
    public class C2A_DeleteRoleHandler : AMRpcHandler<C2A_DeleteRole,A2C_DeleteRole>
    {
        protected override async ETTask Run(Session session, C2A_DeleteRole request, A2C_DeleteRole response, Action reply)
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
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRole, request.AccountId.GetHashCode()))
                {
                    List<RoleInfo> roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<RoleInfo>(d=> d.AccountId == request.AccountId && d.ServerId == request.ServerId && d.Id == request.RoleInfoId);

                    //需要删除的角色未查询成功
                    if (roleInfos == null || roleInfos.Count <= 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNoExist;
                        reply();
                        return;
                    }

                    RoleInfo roleInfo = roleInfos[0];
                    session.AddChild(roleInfo);
                    
                    roleInfo.State = (int)RoleInfoState.Freeze;
                    await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(roleInfo);
                    response.DeleteRoleInfoId = roleInfo.Id;
                    roleInfo?.Dispose();
                    
                    reply();
                }
            }
        }
    }
}