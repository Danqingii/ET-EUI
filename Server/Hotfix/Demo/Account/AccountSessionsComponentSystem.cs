namespace ET
{
    [FriendClass(typeof(AccountSessionsComponent))]
    public class AccountSessionsComponentDestroySystem:DestroySystem<AccountSessionsComponent>
    {
        public override void Destroy(AccountSessionsComponent self)
        {
            self.AccountSessionDictionary.Clear();
        }
    }

    [FriendClass(typeof(AccountSessionsComponent))]
    public static class AccountSessionsComponentSystem
    {
        public static void AddSessionInstanceId(this AccountSessionsComponent self,long accountId,long sessionInstanceId)
        {
            if (!self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary.Add(accountId,sessionInstanceId);
            }
        }
        
        public static void RemoveSessionInstanceId(this AccountSessionsComponent self,long accountId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary.Remove(accountId);
            }
        }

        public static long GetSessionInstanceId(this AccountSessionsComponent self,long accountId)
        {
            long sessionInstanceId = 0;
            self.AccountSessionDictionary.TryGetValue(accountId, out sessionInstanceId);
            return sessionInstanceId;
        }
    }
}