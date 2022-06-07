using System.Collections.Generic;

namespace ET
{
    public class ServerInfoManagerComponentAwakeSystem: AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    public class ServerInfoManagerComponentDestroySystem: DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach (var serverInfo in self.ServerInfos)
            {
                serverInfo?.Dispose();
            }
            self.ServerInfos.Clear();
        }
    }
    
    public class ServerInfoManagerComponentLoadSystem: LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    [FriendClass(typeof(ServerInfoManagerComponent))]
    [FriendClass(typeof(ServerInfo))]
    public static class ServerInfoManagerComponentSystem
    {
        public static async ETTask Awake(this ServerInfoManagerComponent self)
        {
            //从数据库得到当前zone的全部区服信息
            List<ServerInfo> serverInfos = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(d => true);

            if (serverInfos == null || serverInfos.Count <= 0)
            {
                Log.Error($"zone{self.DomainZone()} 无任何区服信息");

                //注意! 商业游戏都是在Web管理后台来操作服务器的配置数据的
                var serverInfoConfigs =  ServerInfoConfigCategory.Instance.GetAll();
                foreach (var config in serverInfoConfigs.Values)
                {
                    ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(config.Id);
                    newServerInfo.ServerName = config.ServerName;
                    newServerInfo.Status = config.Status;
                    self.ServerInfos.Add(newServerInfo);

                    //写入数据到数据库中
                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(newServerInfo);
                }
                
                return;
            }
            
            self.ServerInfos.Clear();

            //添加全部的区服信息
            foreach (var serverInfo in serverInfos)
            {
                self.AddChild(serverInfo);
                self.ServerInfos.Add(serverInfo);
            }
            
            await ETTask.CompletedTask;
        }
    }
}