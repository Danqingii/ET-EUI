namespace ET
{
    public class ServerInfosComponentAwakeSystem : AwakeSystem<ServerInfosComponent>
    {
        public override void Awake(ServerInfosComponent self)
        {
            
        }
    }
    
    public class ServerInfosComponentDestroySystem : DestroySystem<ServerInfosComponent>
    {
        public override void Destroy(ServerInfosComponent self)
        {
            foreach (var serverInfo in self.ServerInfoList)
            {
                serverInfo?.Dispose();
            }
            self.ServerInfoList.Clear();
        }
    }

    [FriendClass(typeof(ServerInfosComponent))]
    public static class ServerInfosComponentSystem
    {
        public static void Add(this ServerInfosComponent self,ServerInfo serverInfo)
        {
            self.ServerInfoList.Add(serverInfo);
        }
        
        public static void Remove(this ServerInfosComponent self,ServerInfo serverInfo)
        {
            self.ServerInfoList.Remove(serverInfo);
        }
    }
}