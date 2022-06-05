using System.Collections.Generic;


namespace ET
{
	public static class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone,long accountId)
		{
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
			
			//通过固定的算法 得到一个gate网关服务器配置
			int n = accountId.GetHashCode() % zone;

			return zoneGates[n];
		}
	}
}
