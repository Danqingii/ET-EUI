
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgServerInfoViewComponentAwakeSystem : AwakeSystem<DlgServerInfoViewComponent> 
	{
		public override void Awake(DlgServerInfoViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgServerInfoViewComponentDestroySystem : DestroySystem<DlgServerInfoViewComponent> 
	{
		public override void Destroy(DlgServerInfoViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
