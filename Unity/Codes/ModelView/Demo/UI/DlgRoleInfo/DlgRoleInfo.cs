using System.Collections.Generic;

namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgRoleInfo :Entity,IAwake,IUILogic
	{

		public DlgRoleInfoViewComponent View { get => this.Parent.GetComponent<DlgRoleInfoViewComponent>();}

		public Dictionary<int, Scroll_Item_RoleInfo> ScrollItemServerInfos = new Dictionary<int, Scroll_Item_RoleInfo>();

	}
}
