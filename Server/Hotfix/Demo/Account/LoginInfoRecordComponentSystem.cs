namespace ET
{
    public class LoginInfoRecordComponentDestroySystem : DestroySystem<LoginInfoRecordComponent>
    {
        public override void Destroy(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDict.Clear();
        }
    }

    [FriendClass(typeof(LoginInfoRecordComponent))]
    public static class LoginInfoRecordComponentSystem
    {
        public static void AddAccount(this LoginInfoRecordComponent self,long accountId,int zone)
        {
            if (self.AccountLoginInfoDict.ContainsKey(accountId))
            {
                self.AccountLoginInfoDict[accountId] = zone;
                return;
            }
            self.AccountLoginInfoDict.Add(accountId,zone);
        }
        
        public static void RemoveAccount(this LoginInfoRecordComponent self,long accountId)
        {
            if (self.AccountLoginInfoDict.ContainsKey(accountId))
            {
                self.AccountLoginInfoDict.Remove(accountId);
            }
        }

        public static int GetZone(this LoginInfoRecordComponent self,long accountId)
        {
            int zone = -1;
            self.AccountLoginInfoDict.TryGetValue(accountId, out zone);
            return zone;
        }

        public static bool IsExistAccount(this LoginInfoRecordComponent self,long accountId)
        {
            return self.AccountLoginInfoDict.ContainsKey(accountId);
        }
    }
}