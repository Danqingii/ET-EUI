using System;

namespace ET
{
    [Timer(TimerType.PlayerOffLineOutTime)]
    public class  PlayerOffLineOutTime : ATimer<PlayerOffLineOutTimeComponent>
    {
        public override void Run(PlayerOffLineOutTimeComponent self)
        {
            try
            {
                self.KickPlayer();
            }
            catch (Exception e)
            {
                Log.Error($"玩家下线定时器错误:{self.Id}\n{e}");
            }
        }
    }

    public class PlayerOffLineOutTimeComponentDestroySystem : DestroySystem<PlayerOffLineOutTimeComponent>
    {
        public override void Destroy(PlayerOffLineOutTimeComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    public class PlayerOffLineOutTimeComponentAwakeSystem : AwakeSystem<PlayerOffLineOutTimeComponent>
    {
        public override void Awake(PlayerOffLineOutTimeComponent self)
        {
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 1000, TimerType.PlayerOffLineOutTime, self);
        }
    }

    public static class PlayerOffLineOutTimeComponentSystem
    {
        public static void KickPlayer(this PlayerOffLineOutTimeComponent self)
        {
            DisconnectHelper.KickPlayer(self.GetParent<Player>()).Coroutine();
        }
    }
}