
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgRoleInfoViewComponent : Entity,IAwake,IDestroy 
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

		public UnityEngine.UI.Button E_CreateRoleBtn
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CreateRoleBtn == null )
     			{
		    		this.m_E_CreateRoleBtn = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_CreateRole");
     			}
     			return this.m_E_CreateRoleBtn;
     		}
     	}

		public UnityEngine.UI.Image E_CreateRoleImg
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CreateRoleImg == null )
     			{
		    		this.m_E_CreateRoleImg = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_CreateRole");
     			}
     			return this.m_E_CreateRoleImg;
     		}
     	}

		public UnityEngine.UI.Button E_DeleteRoleBtn
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DeleteRoleBtn == null )
     			{
		    		this.m_E_DeleteRoleBtn = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_DeleteRole");
     			}
     			return this.m_E_DeleteRoleBtn;
     		}
     	}

		public UnityEngine.UI.Image E_DeleteRoleImg
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DeleteRoleImg == null )
     			{
		    		this.m_E_DeleteRoleImg = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_DeleteRole");
     			}
     			return this.m_E_DeleteRoleImg;
     		}
     	}

		public UnityEngine.UI.Button E_EnterGameBtn
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterGameBtn == null )
     			{
		    		this.m_E_EnterGameBtn = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_EnterGame");
     			}
     			return this.m_E_EnterGameBtn;
     		}
     	}

		public UnityEngine.UI.Image E_EnterGameImg
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterGameImg == null )
     			{
		    		this.m_E_EnterGameImg = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_EnterGame");
     			}
     			return this.m_E_EnterGameImg;
     		}
     	}

		public UnityEngine.UI.LoopHorizontalScrollRect E_RoleInfosLoopH_Srect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RoleInfosLoopH_Srect == null )
     			{
		    		this.m_E_RoleInfosLoopH_Srect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopHorizontalScrollRect>(this.uiTransform.gameObject,"E_RoleInfos");
     			}
     			return this.m_E_RoleInfosLoopH_Srect;
     		}
     	}

		public UnityEngine.UI.InputField E_RoleNameIField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RoleNameIField == null )
     			{
		    		this.m_E_RoleNameIField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"E_RoleName");
     			}
     			return this.m_E_RoleNameIField;
     		}
     	}

		public UnityEngine.UI.Image E_RoleNameImg
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RoleNameImg == null )
     			{
		    		this.m_E_RoleNameImg = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_RoleName");
     			}
     			return this.m_E_RoleNameImg;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_BgImg = null;
			this.m_E_CreateRoleBtn = null;
			this.m_E_CreateRoleImg = null;
			this.m_E_DeleteRoleBtn = null;
			this.m_E_DeleteRoleImg = null;
			this.m_E_EnterGameBtn = null;
			this.m_E_EnterGameImg = null;
			this.m_E_RoleInfosLoopH_Srect = null;
			this.m_E_RoleNameIField = null;
			this.m_E_RoleNameImg = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Image m_E_BgImg = null;
		private UnityEngine.UI.Button m_E_CreateRoleBtn = null;
		private UnityEngine.UI.Image m_E_CreateRoleImg = null;
		private UnityEngine.UI.Button m_E_DeleteRoleBtn = null;
		private UnityEngine.UI.Image m_E_DeleteRoleImg = null;
		private UnityEngine.UI.Button m_E_EnterGameBtn = null;
		private UnityEngine.UI.Image m_E_EnterGameImg = null;
		private UnityEngine.UI.LoopHorizontalScrollRect m_E_RoleInfosLoopH_Srect = null;
		private UnityEngine.UI.InputField m_E_RoleNameIField = null;
		private UnityEngine.UI.Image m_E_RoleNameImg = null;
		public Transform uiTransform = null;
	}
}
