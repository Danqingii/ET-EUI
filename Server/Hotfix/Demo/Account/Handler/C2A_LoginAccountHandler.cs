using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET
{
    //登陆账号处理者
    //玩家登陆注意事项
    //1.同一玩家可能一直重复点击请求登陆
    //2.不同玩家可能在不同客户端同时发送登陆请求
    //3.如果已经有玩家已经登陆的情况
    
    [MessageHandler]
    [FriendClass(typeof(Account))]
    public class C2A_LoginAccountHandler : AMRpcHandler<C2A_LoginAccount,A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求Scene错误,当前Scene为{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            
            //移除session短连接组件
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            
            //防止玩家一直按请求按钮
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeat;
                reply();
                session?.Disconnect().Coroutine(); 
                return;
            }

            if (request.AccountName == string.Empty || request.Password == string.Empty)
            {
                response.Error = ErrorCode.ERR_LoginNameOrPwNull;
                reply();
                session?.Disconnect().Coroutine(); 
                return;
            }

            if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_LoginAccountNameRule;
                reply();
                session?.Disconnect().Coroutine(); 
                return;
            }

            if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))
            {
                response.Error = ErrorCode.ERR_LoginPassWordRule;
                reply();
                session?.Disconnect().Coroutine(); 
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                //账号登陆携程锁,防止玩家不同地同时登陆,造成2个Account信息
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    List<Account> accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<Account>(d => d.AccountName.Equals(request.AccountName.Trim()));

                    Account account = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        if (account.AccountType == (int) AccountType.Blocked)
                        {
                            response.Error = ErrorCode.ERR_AccountBlocked;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        if (!request.Password.Trim().Equals(account.PassWord))
                        {
                            response.Error = ErrorCode.ERR_LoginPassWordError;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName;
                        account.PassWord = request.Password;
                        account.CreateTimer = TimeHelper.ServerNow();
                        account.AccountType = (int) AccountType.General;
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
                    }

                    //保存当前玩家的账号在登陆中心服
                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCentre");
                    long loginCentreInstacneId = loginCenterConfig.InstanceId;
                    var l2ALoginAccountRecord = (L2A_LoginAccountRecord) await MessageHelper.CallActor(loginCentreInstacneId, new A2L_LoginAccountRecord() {AccountId = account.Id});
                    if (l2ALoginAccountRecord.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = l2ALoginAccountRecord.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        account?.Dispose();
                        return;
                    }
                    
                    //尝试得到上一次的账号连接的session  如果该账号存在于账号通讯管理中 代表该玩家被顶号了,通知当前玩家下线
                    long accountSessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().GetSessionInstanceId(account.Id);
                    Session otherSession = Game.EventSystem.Get(accountSessionInstanceId) as Session;
                    if (otherSession != null)
                    {
                        //通知当前在线的玩家 下线
                        otherSession.Send(new A2C_AccountDisconnect(){Error = 0});
                        otherSession.Disconnect().Coroutine();
                        
                        Log.Debug($"{account.Id}T玩家下线了");
                        //把该玩家从账号通讯管理中移除
                        session.DomainScene().GetComponent<AccountSessionsComponent>().RemoveSessionInstanceId(accountSessionInstanceId);
                    }
                    
                    Log.Debug($"{account.Id}保存玩家账号");
                    //保存后续玩家的session连接
                    session.DomainScene().GetComponent<AccountSessionsComponent>().AddSessionInstanceId(account.Id,session.InstanceId);
                   
                    //账号检测闲置时间组件 10分钟后会激活
                    session.AddComponent<AccountCheckOutTimeComponent, long>(account.Id);
                    
                    //把令牌保存到账号服务器的令牌组件上
                    string token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.DomainScene().GetComponent<TokenComponent>().RemoveToken(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().AddToken(account.Id, token);

                    response.Token = token;
                    response.AccountId = account.Id;
                    
                    account?.Dispose();
                    reply();
                }
            }
        }
    }
}