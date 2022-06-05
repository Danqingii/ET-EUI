using System;

namespace ET
{
    [Timer(TimerType.AccountSessionCheckOutTime)]
    public class AccountSessionCheckOunTime : ATimer<AccountCheckOutTimeComponent>
    {
        public override void Run(AccountCheckOutTimeComponent self)
        {
            try
            {
                //当触发该方法,代表600000毫秒已经过去了,让该玩家下线
                self.DeleteSession();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }

    public class AccountCheckOutTimeComponentAwakeSystem : AwakeSystem<AccountCheckOutTimeComponent,long>
    {
        public override void Awake(AccountCheckOutTimeComponent self, long accountId)
        {
            self.AccountId = accountId;
            TimerComponent.Instance.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 600000,TimerType.AccountSessionCheckOutTime,self);
        }
    }

    public class AccountCheckOutTimeComponentDestroySystem: DestroySystem<AccountCheckOutTimeComponent>
    {
        public override void Destroy(AccountCheckOutTimeComponent self)
        {
            self.AccountId = 0;
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    [FriendClass(typeof(AccountCheckOutTimeComponent))]
    public static class AccountCheckOutTimeComponentSystem
    {
        public static void DeleteSession(this AccountCheckOutTimeComponent self)
        {
            Session session = self.GetParent<Session>();

            //删除在账号通讯管理跟该玩家保持通讯的session
            long accountSessionInstaceId = session.DomainScene().GetComponent<AccountSessionsComponent>().GetSessionInstanceId(self.AccountId);
            if (session.InstanceId == accountSessionInstaceId)
            {
                session.DomainScene().GetComponent<AccountSessionsComponent>().RemoveSessionInstanceId(self.AccountId);
            }
            
            //如果该通讯不为空  通知该玩家下线
            session?.Send(new A2C_AccountDisconnect(){Error = 1});
            session?.Disconnect().Coroutine();
        }
    }
}