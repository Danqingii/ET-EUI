
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgServerInfosViewComponentAwakeSystem : AwakeSystem<DlgServerInfosViewComponent> 
	{
		public override void Awake(DlgServerInfosViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgServerInfosViewComponentDestroySystem : DestroySystem<DlgServerInfosViewComponent> 
	{
		public override void Destroy(DlgServerInfosViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
