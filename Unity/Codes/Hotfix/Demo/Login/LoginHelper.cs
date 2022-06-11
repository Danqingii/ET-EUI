using System;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof(AccountInfoComponent))]
    [FriendClass(typeof(ServerInfosComponent))]
    [FriendClass(typeof(RoleInfosComponent))]
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            Session accountSession = null;
            A2C_LoginAccount a2CLoginAccount = null;
            
            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringConversionMD5(password);
                a2CLoginAccount = (A2C_LoginAccount) await accountSession.Call(new C2A_LoginAccount() { AccountName = account, Password = password });
            }
            catch (Exception e)
            {
                accountSession?.Dispose();
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetwordError;
            }
            

            if (a2CLoginAccount.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"请求登陆,错误码{a2CLoginAccount.Error}");
                accountSession?.Dispose();
                return a2CLoginAccount.Error;
            }

            //保存跟账号服务器的通讯
            zoneScene.AddComponent<SessionComponent>().Session = accountSession;
            
            //给通讯器添加心跳检测组件
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();
            
            //保存服务器返回的登陆信息
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2CLoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountId = a2CLoginAccount.AccountId;
            
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> GetServerInfos(Scene zoneScene)
        {
            A2C_GetServerInfos a2CGetServerInfos = null;
            
            try
            {
                a2CGetServerInfos = (A2C_GetServerInfos) await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfos()
                {
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetwordError;
            }
            
            if (a2CGetServerInfos.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2CGetServerInfos.Error.ToString());
                return a2CGetServerInfos.Error;
            }

            foreach (var serverInfoProto in a2CGetServerInfos.ServerInfoList)
            {
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfosComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(serverInfoProto);
                zoneScene.GetComponent<ServerInfosComponent>().Add(serverInfo);
            }
            
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> CreateRole(Scene zoneScene,string name)
        {
            A2C_CreateRole a2CCreateRole = null;

            try
            {
                a2CCreateRole = (A2C_CreateRole) await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_CreateRole()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    Name = name,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetwordError;
            }

            if (a2CCreateRole.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"请求创建角色,错误码{a2CCreateRole.Error}");
                return a2CCreateRole.Error;
            }

            RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
            newRoleInfo.FromMessage(a2CCreateRole.RoleInfoProto);
            zoneScene.GetComponent<RoleInfosComponent>().AddRoleInfo(newRoleInfo);
            
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> GetRoles(Scene zoneScene)
        {
            A2C_GetRoles a2CGetRoles = null;

            try
            {
                a2CGetRoles = (A2C_GetRoles) await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRoles()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetwordError;
            }

            if (a2CGetRoles.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"请求获取全部角色信息,错误码{a2CGetRoles.Error}");
                return a2CGetRoles.Error;
            }

            zoneScene.GetComponent<RoleInfosComponent>().ClearRoleInfo();
            foreach (var roleInfoProto in a2CGetRoles.RoleInfoList)
            {
                RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
                newRoleInfo.FromMessage(roleInfoProto);
                zoneScene.GetComponent<RoleInfosComponent>().AddRoleInfo(newRoleInfo);
            }

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> DelectRole(Scene zoneScene)
        {
            A2C_DeleteRole a2CDeleteRole = null;

            try
            {
                a2CDeleteRole = (A2C_DeleteRole) await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_DeleteRole()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    RoleInfoId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetwordError;
            }

            if (a2CDeleteRole.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"请求删除角色,错误码{a2CDeleteRole.Error}");
                return a2CDeleteRole.Error;
            }

            int index = zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.FindIndex(d=> d.Id == a2CDeleteRole.DeleteRoleInfoId);
            zoneScene.GetComponent<RoleInfosComponent>().RemoveRoleInfo(index);
            
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> GetRelamekey(Scene zoneScene)
        {
            A2C_GetRealmKey a2CGetRealmKey = null;

            try
            {
                a2CGetRealmKey = (A2C_GetRealmKey) await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRealmKey()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetwordError;
            }

            if (a2CGetRealmKey.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"请求RealmKey,错误码{a2CGetRealmKey.Error}");
                return a2CGetRealmKey.Error;
            }

            zoneScene.GetComponent<AccountInfoComponent>().RealmKey = a2CGetRealmKey.RealmKey;
            zoneScene.GetComponent<AccountInfoComponent>().RealmAddress = a2CGetRealmKey.RealmAddress;
            
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> EnterGame(Scene zoneScene)
        {
            string realmAddress = zoneScene.GetComponent<AccountInfoComponent>().RealmAddress;
            
            //1.连接Realm,获取分配的Gate
            Session realmSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(realmAddress));

            R2C_LoginRealm r2CLoginRealm = null;
            
            try
            {
                r2CLoginRealm = (R2C_LoginRealm) await realmSession.Call(new C2R_LoginRealm()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    RealmKey = zoneScene.GetComponent<AccountInfoComponent>().RealmKey
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                realmSession?.Dispose();
                return ErrorCode.ERR_NetwordError;
            }
            realmSession?.Dispose();

            if (r2CLoginRealm.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"请求Realm获取Gate,错误码{r2CLoginRealm.Error}");
                return r2CLoginRealm.Error;
            }

            //2.连接Gate
            Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2CLoginRealm.GateAddress));

            try
            {

            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                gateSession?.Dispose();
                return ErrorCode.ERR_NetwordError;
            }

            
            return ErrorCode.ERR_Success;
        }
    }
}