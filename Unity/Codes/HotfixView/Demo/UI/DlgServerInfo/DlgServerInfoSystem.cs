using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[FriendClass(typeof(DlgServerInfo))]
	[FriendClass(typeof(ServerInfosComponent))]
	[FriendClass(typeof(ServerInfo))]
	public static  class DlgServerInfoSystem
	{

		public static void RegisterUIEvent(this DlgServerInfo self)
		{
		     self.View.E_EnterServerBtn.AddListenerAsync(self.OnEnterServerClick);
		     self.View.E_ServerInfosLoopH_Srect.AddItemRefreshListener((tran,index) => {self.OnLeepListItemRefresClick(tran,index);});
		}
		
		public static void ShowWindow(this DlgServerInfo self, Entity contextData = null)
		{
			int count = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfoList.Count;
			self.AddUIScrollItems(ref self.ScrollItemServerInfos,count);
			self.View.E_ServerInfosLoopH_Srect.SetVisible(true,count);
		}

		public static void HideWindow(this DlgServerInfo self)
		{
			self.RemoveUIScrollItems(ref self.ScrollItemServerInfos);
		}

		//滑动列表刷新
		public static void OnLeepListItemRefresClick(this DlgServerInfo self, Transform tran,int index)
		{
			Scroll_Item_ServerInfo itemServerInfo = self.ScrollItemServerInfos[index].BindTrans(tran);
			ServerInfo serverInfo = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfoList[index];
			itemServerInfo.EBtn_SelectImg.color = serverInfo.Id == self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId ? Color.red : Color.yellow;
			itemServerInfo.ETxt_ContentTxt.text = serverInfo.ServerName;
			itemServerInfo.EBtn_SelectBtn.AddListener(() => { self.OnSelectServerItemClick(serverInfo.Id); });
		}

		public static void OnSelectServerItemClick(this DlgServerInfo self,long serverId)
		{
			self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId = (int)serverId;
			Log.Debug($"当前选择的服务器为Id为{serverId}");
			self.View.E_ServerInfosLoopH_Srect.RefillCells();
		}
		
		public static async ETTask OnEnterServerClick(this DlgServerInfo self)
		{
			bool isSuc = self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId != 0;

			if (!isSuc)
			{
				Log.Error($"请先选择服务器");
				return;
			}

			try
			{
				int errorCode = await LoginHelper.GetRoles(self.ZoneScene());
				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error($"获取角色信息列表失败{errorCode}");
					return;
				}
				
				self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_ServerInfo);
				self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_RoleInfo);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
