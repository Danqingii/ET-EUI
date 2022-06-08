namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgServerInfos :Entity,IAwake,IUILogic
	{

		public DlgServerInfosViewComponent View { get => this.Parent.GetComponent<DlgServerInfosViewComponent>();} 

		 

	}
}
