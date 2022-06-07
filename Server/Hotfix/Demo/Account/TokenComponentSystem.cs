namespace ET
{
    public class TokenComponentDestroySystem:DestroySystem<TokenComponent>
    {
        public override void Destroy(TokenComponent self)
        {
            self.TokenDict.Clear();
        }
    }

    [FriendClass(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {
        //在添加令牌的时候 10分钟后会自动移除令牌
        public static void AddToken(this TokenComponent self,long accountId,string token)
        {
            self.TokenDict.Add(accountId,token);
            self.TokenMaintainTimer(accountId,token).Coroutine();
        }
        
        public static void RemoveToken(this TokenComponent self,long accountId)
        {
            if (self.TokenDict.ContainsKey(accountId))
            {
                self.TokenDict.Remove(accountId);
            }
        }

        public static string GetToken(this TokenComponent self,long accountId)
        {
            string token = string.Empty;
            self.TokenDict.TryGetValue(accountId, out token);
            return token;
        }

        //10分钟之后移除令牌
        private static async ETTask TokenMaintainTimer(this TokenComponent self,long key,string token)
        {
            await TimerComponent.Instance.WaitAsync(600000);

            string onlineToken = self.GetToken(key);
            
            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == token)
            {
                self.RemoveToken(key);
            }
        }
    }
}