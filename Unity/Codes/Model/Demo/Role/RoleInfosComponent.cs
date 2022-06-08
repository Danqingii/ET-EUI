using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 角色信息组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    [ChildType(typeof(RoleInfo))]
    public class RoleInfosComponent : Entity,IAwake,IDestroy
    {
        public List<RoleInfo> RoleInfos = new List<RoleInfo>();
        public int CurrentRoleId = 0;
    }
}