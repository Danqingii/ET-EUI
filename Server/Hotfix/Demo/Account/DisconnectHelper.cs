using System;

namespace ET
{
    /// <summary>
    /// 断开连接帮助
    /// </summary>
    [FriendClass(typeof(Player))]
    public static class DisconnectHelper
    {
        /// <summary>
        /// 踢玩家下线
        /// </summary>
        /// <param name="player">gate服映射</param>
        /// <param name="isException">是否异常下线</param>
        public static async ETTask KickPlayer(Player player,bool isException = false)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }

            long instanceId = player.InstanceId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate,player.AccountId.GetHashCode()))
            {
                if (player.IsDisposed || player.InstanceId != instanceId)
                {
                    return;
                }

                //非异常情况下线 也就是正常点击退出游戏
                if (!isException)
                {
                    switch (player.PlayerState)
                    {
                        case PlayerState.Disconnect:
                            break;
                        case PlayerState.Gate:
                            break;
                        case PlayerState.Game:
                            //TODO 通知游戏逻辑服下线unit角色逻辑 并且保存数据到数据库
                            var g2GRequestExitGame = (M2G_RequestExitGame) await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestExitGame());
                            
                            //通知登陆中心服移除角色登陆信息
                            StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
                            var l2GRemoveLoginRecord = (L2G_RemoveLoginRecord)await MessageHelper.CallActor(loginCenterConfig.InstanceId, new G2L_RemoveLoginRecord()
                            {
                                AccountId = player.AccountId,
                                ServerId = player.DomainZone()
                            });
                            break;
                    }
                }

               

                player.PlayerState = PlayerState.Disconnect;
                player.DomainScene().GetComponent<PlayerComponent>()?.Remove(player.AccountId);
                player?.Dispose();
                await TimerComponent.Instance.WaitAsync(300);
            }
            
            await ETTask.CompletedTask;
        }
    }
}