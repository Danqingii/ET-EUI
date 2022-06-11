using System;
using System.Collections.Generic;

namespace ET
{
    [MessageHandler]
    [FriendClass(typeof(RoleInfo))]
    public class C2A_CreateRoleHandler : AMRpcHandler<C2A_CreateRole,A2C_CreateRole>
    {
        protected override async ETTask Run(Session session, C2A_CreateRole request, A2C_CreateRole response, Action reply)
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

            if (string.IsNullOrEmpty(request.Name))
            {
                response.Error = ErrorCode.ERR_RoleNameIsNull;
                reply();
                return;
            }

            if (request.Name.Length < 2 || request.Name.Length > 8)
            {
                response.Error = ErrorCode.ERR_RoleNameIsNorm;
                reply();
                return;
            }
            
            //TODO其实还需要判断是否名字敏感词

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRole,request.AccountId.GetHashCode()))
                {
                    List<RoleInfo> roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<RoleInfo>(d => d.Name == request.Name && d.ServerId == request.ServerId);

                    
                    //数据库已经存在该角色了 无法创建
                    if (roleInfos !=  null && roleInfos.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleExist;
                        reply();
                        return;
                    }

                    RoleInfo newRoleInfo = session.AddChildWithId<RoleInfo>(IdGenerater.Instance.GenerateUnitId(request.ServerId));
                    newRoleInfo.Name = request.Name;
                    newRoleInfo.State = (int)RoleInfoState.Normal;
                    newRoleInfo.ServerId = request.ServerId;
                    newRoleInfo.AccountId = request.AccountId;
                    newRoleInfo.CreateTime = TimeHelper.ServerNow();
                    newRoleInfo.LastLoginTime = 0;

                    await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(newRoleInfo);

                    response.RoleInfoProto = newRoleInfo.ToMessage();
                
                
                    reply();
                
                    //这个实例传递之后要销户掉了
                    newRoleInfo?.Dispose();
                }
            }
        }
    }
}