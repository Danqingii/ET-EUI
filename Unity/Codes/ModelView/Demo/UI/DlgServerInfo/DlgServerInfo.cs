using System.Collections.Generic;

namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgServerInfo :Entity,IAwake,IUILogic
	{

		public DlgServerInfoViewComponent View { get => this.Parent.GetComponent<DlgServerInfoViewComponent>();}

		public Dictionary<int, Scroll_Item_ServerInfo> ScrollItemServerInfos = new Dictionary<int, Scroll_Item_ServerInfo>();

	}
}
