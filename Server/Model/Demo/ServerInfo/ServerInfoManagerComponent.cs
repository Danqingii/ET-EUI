using System.Collections.Generic;

namespace ET
{ 
    /// <summary>
    /// 服务器区服管理组件
    /// </summary>
    [ChildType(typeof(ServerInfo))]
    [ComponentOf(typeof(Scene))]
    public class ServerInfoManagerComponent : Entity,IAwake,IDestroy,ILoad
    {
        public List<ServerInfo> ServerInfos = new List<ServerInfo>();
    }
}