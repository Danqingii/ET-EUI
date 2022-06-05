using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ET
{
	public static  class DlgLoginSystem
	{

		public static void RegisterUIEvent(this DlgLogin self)
		{
			self.View.E_LoginButton.AddListenerAsync(self.OnLoginClickHandler);
		}

		public static void ShowWindow(this DlgLogin self, Entity contextData = null)
		{
			
		}
		
		public static async ETTask OnLoginClickHandler(this DlgLogin self)
		{
			try
			{
				if (self.View.E_AccountInputField.GetComponent<InputField>().text == "" && self.View.E_PasswordInputField.GetComponent<InputField>().text == "")
				{
					self.View.E_AccountInputField.GetComponent<InputField>().text = "Dzw798897751";
					self.View.E_PasswordInputField.GetComponent<InputField>().text = "Dzw798897751";
				}

				int errorCode = await LoginHelper.Login(
					self.DomainScene(), 
					ConstValue.AccountAddress, 
					self.View.E_AccountInputField.GetComponent<InputField>().text, 
					self.View.E_PasswordInputField.GetComponent<InputField>().text);

				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error($"登陆错误码:{errorCode}");
					return;
				}

				Log.Debug("登陆成功");
				//self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
				//self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Lobby);
			}
			catch (Exception e)
			{
				Log.Error($"登陆错误:{e}");
			}

			await ETTask.CompletedTask;
		}
		
		public static void HideWindow(this DlgLogin self)
		{

		}
		
	}
}
