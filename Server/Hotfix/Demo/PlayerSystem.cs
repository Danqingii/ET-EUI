namespace ET
{
    [FriendClass(typeof(Player))]
    public static class PlayerSystem
    {
        [ObjectSystem]
        public class PlayerAwakeSystem : AwakeSystem<Player,string>
        {
            public override void Awake(Player self, string a)
            {
                self.AccountId =  long.Parse(a);
            }
        }
        
        
        [ObjectSystem]
        public class PlayerAwake1System : AwakeSystem<Player, long,long>
        {
            public override void Awake(Player self, long acountId, long roleId)
            {
                self.AccountId = acountId;
                self.UnitId = roleId;
            }
        }
    }
}