namespace ET
{
    [FriendClass(typeof(AccountInfoComponent))]
    public class AccountInfoComponentDestroySystem : DestroySystem<AccountInfoComponent>
    {
        public override void Destroy(AccountInfoComponent self)
        {
            self.AccountId = 0;
            self.Token = string.Empty;
        }
    }
    
    public static class AccountInfoComponentSystem
    {
        
    }
}