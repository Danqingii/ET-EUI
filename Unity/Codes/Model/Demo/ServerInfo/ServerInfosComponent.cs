using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 存放服务器列表组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    [ChildType(typeof(ServerInfo))]
    public class ServerInfosComponent : Entity, IAwake,IDestroy
    {
        public List<ServerInfo> ServerInfoList = new List<ServerInfo>();
        public int CurrentServerId = 0;
    }
}