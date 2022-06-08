
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[EnableMethod]
	public  class Scroll_Item_RoleInfo : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_Item_RoleInfo BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

		public UnityEngine.UI.Button EBtn_SelectBtn
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_EBtn_SelectBtn == null )
     				{
		    			this.m_EBtn_SelectBtn = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EBtn_Select");
     				}
     				return this.m_EBtn_SelectBtn;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EBtn_Select");
     			}
     		}
     	}

		public UnityEngine.UI.Image EBtn_SelectImg
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_EBtn_SelectImg == null )
     				{
		    			this.m_EBtn_SelectImg = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EBtn_Select");
     				}
     				return this.m_EBtn_SelectImg;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EBtn_Select");
     			}
     		}
     	}

		public UnityEngine.UI.Text ETxt_ContentTxt
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_ETxt_ContentTxt == null )
     				{
		    			this.m_ETxt_ContentTxt = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"ETxt_Content");
     				}
     				return this.m_ETxt_ContentTxt;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"ETxt_Content");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EBtn_SelectBtn = null;
			this.m_EBtn_SelectImg = null;
			this.m_ETxt_ContentTxt = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EBtn_SelectBtn = null;
		private UnityEngine.UI.Image m_EBtn_SelectImg = null;
		private UnityEngine.UI.Text m_ETxt_ContentTxt = null;
		public Transform uiTransform = null;
	}
}
