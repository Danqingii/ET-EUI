namespace ET
{
    public class RoleInfosComponentAwakeSystem: AwakeSystem<RoleInfosComponent>
    {
        public override void Awake(RoleInfosComponent self)
        {
            
        }
    }
    
    public class RoleInfosComponentDestroySystem : DestroySystem<RoleInfosComponent>
    {
        public override void Destroy(RoleInfosComponent self)
        {
            self.ClearRoleInfo();
            self.CurrentRoleId = 0;
        }
    }

    [FriendClass(typeof(RoleInfosComponent))]
    public static class RoleInfosComponentSystem
    {
        public static void AddRoleInfo(this RoleInfosComponent self,RoleInfo roleInfo)
        {
            self.RoleInfos.Add(roleInfo);
        }
        
        public static void RemoveRoleInfo(this RoleInfosComponent self,int index)
        {
            self.RoleInfos.RemoveAt(index);
        }

        public static void ClearRoleInfo(this RoleInfosComponent self)
        {
            foreach (var roleInfo in self.RoleInfos)
            {
                roleInfo?.Dispose();
            }
            self.RoleInfos.Clear();
        }
    }
}