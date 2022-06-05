namespace ET
{
    public class TokenComponentDestroySystem:DestroySystem<TokenComponent>
    {
        public override void Destroy(TokenComponent self)
        {
            self.TokenDictionary.Clear();
        }
    }

    [FriendClass(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {
        //在添加令牌的时候 10分钟后会自动移除令牌
        public static void AddToken(this TokenComponent self,long key,string token)
        {
            self.TokenDictionary.Add(key,token);
            self.TokenMaintainTimer(key,token).Coroutine();
        }
        
        public static void RemoveToken(this TokenComponent self,long key)
        {
            if (self.TokenDictionary.ContainsKey(key))
            {
                self.TokenDictionary.Remove(key);
            }
        }

        public static string GetToken(this TokenComponent self,long key)
        {
            string token = string.Empty;
            self.TokenDictionary.TryGetValue(key, out token);
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