using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[FriendClass(typeof(DlgRoleInfo))]
	[FriendClass(typeof(RoleInfosComponent))]
	[FriendClass(typeof(RoleInfo))]
	public static  class DlgRoleInfoSystem
	{

		public static void RegisterUIEvent(this DlgRoleInfo self)
		{
			self.View.E_CreateRoleBtn.AddListenerAsync(self.OnCreateRoleHandler);
			self.View.E_DeleteRoleBtn.AddListenerAsync(self.OnDeleteRoleHandler);
			self.View.E_EnterGameBtn.AddListenerAsync(self.OnEnterGameHandler);
			self.View.E_RoleInfosLoopH_Srect.AddItemRefreshListener((trans, index) => { self.OnLeepListRoleRefresClick(trans, index);});
		}

		public static void ShowWindow(this DlgRoleInfo self, Entity contextData = null)
		{
			self.RefreshRoleItems();
		}

		public static void HideWindow(this DlgRoleInfo self)
		{
			self.RemoveUIScrollItems(ref self.ScrollItemServerInfos);
		}

		public static async ETTask OnCreateRoleHandler(this DlgRoleInfo self)
		{
			string name = self.View.E_RoleNameIField.text;

			if (string.IsNullOrEmpty(name))
			{
				Log.Error("名字输入为空");
				return;
			}

			try
			{
				int errorCode = await LoginHelper.CreateRole(self.ZoneScene(), name);

				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}

				self.RefreshRoleItems();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		
		public static async ETTask OnDeleteRoleHandler(this DlgRoleInfo self)
		{
			if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId <= 0)
			{
				 Log.Error("请先选择一个需要删除的角色");
                 return;				
			}

			try
			{
				int errorCode = await LoginHelper.DelectRole(self.ZoneScene());
				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}

				self.RefreshRoleItems();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
		
		public static async ETTask OnEnterGameHandler(this DlgRoleInfo self)
		{
			if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId <= 0)
			{
				Log.Error("请先选择一个角色");
				return;
			}

			try
			{
				int errorCode = await LoginHelper.GetRelamekey(self.ZoneScene());
				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}

				errorCode = await LoginHelper.EnterGame(self.ZoneScene());
				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}

			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}

		public static void OnLeepListRoleRefresClick(this DlgRoleInfo self,Transform trans,int index)
		{
			Scroll_Item_RoleInfo scrollItemRoleInfo = self.ScrollItemServerInfos[index].BindTrans(trans);
			RoleInfo roleInfo = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos[index];
			scrollItemRoleInfo.EBtn_SelectImg.color = roleInfo.Id == self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId ? Color.red : Color.yellow;
			scrollItemRoleInfo.ETxt_ContentTxt.text = roleInfo.Name;
			scrollItemRoleInfo.EBtn_SelectBtn.AddListener(() => { self.OnSelectRoleItemClick(roleInfo.Id); });
		}
		
		public static void OnSelectRoleItemClick(this DlgRoleInfo self,long roleId)
		{
			self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = roleId;
			Log.Debug($"当前选择的角色Id为{roleId}");
			self.View.E_RoleInfosLoopH_Srect.RefillCells();
		}

		public static void RefreshRoleItems(this DlgRoleInfo self)
		{
			int count = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos.Count;
			self.AddUIScrollItems(ref self.ScrollItemServerInfos,count);
			self.View.E_RoleInfosLoopH_Srect.SetVisible(true,count);
		}
	}
}
