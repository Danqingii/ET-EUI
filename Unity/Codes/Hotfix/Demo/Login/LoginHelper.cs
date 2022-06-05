using System;
using UnityEngine;

namespace ET
{
    [FriendClass(typeof(AccountInfoComponent))]
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
    }
}