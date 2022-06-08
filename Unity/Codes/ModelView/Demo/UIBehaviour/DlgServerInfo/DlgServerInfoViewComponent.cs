
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgServerInfoViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Image E_BgImg
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_BgImg == null )
     			{
		    		this.m_E_BgImg = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Bg");
     			}
     			return this.m_E_BgImg;
     		}
     	}

		public UnityEngine.UI.Button E_EnterServerBtn
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterServerBtn == null )
     			{
		    		this.m_E_EnterServerBtn = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_EnterServer");
     			}
     			return this.m_E_EnterServerBtn;
     		}
     	}

		public UnityEngine.UI.Image E_EnterServerImg
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterServerImg == null )
     			{
		    		this.m_E_EnterServerImg = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_EnterServer");
     			}
     			return this.m_E_EnterServerImg;
     		}
     	}

		public UnityEngine.UI.LoopHorizontalScrollRect E_ServerInfosLoopH_Srect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ServerInfosLoopH_Srect == null )
     			{
		    		this.m_E_ServerInfosLoopH_Srect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopHorizontalScrollRect>(this.uiTransform.gameObject,"E_ServerInfos");
     			}
     			return this.m_E_ServerInfosLoopH_Srect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_BgImg = null;
			this.m_E_EnterServerBtn = null;
			this.m_E_EnterServerImg = null;
			this.m_E_ServerInfosLoopH_Srect = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Image m_E_BgImg = null;
		private UnityEngine.UI.Button m_E_EnterServerBtn = null;
		private UnityEngine.UI.Image m_E_EnterServerImg = null;
		private UnityEngine.UI.LoopHorizontalScrollRect m_E_ServerInfosLoopH_Srect = null;
		public Transform uiTransform = null;
	}
}
