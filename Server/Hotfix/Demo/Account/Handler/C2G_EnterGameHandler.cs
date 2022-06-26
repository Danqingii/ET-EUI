using System;

namespace ET
{
    [MessageHandler]
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(SessionStateComponent))]
    [FriendClass(typeof(Player))]
    [FriendClass(typeof(GateMapComponent))]
    public class C2G_EnterGameHandler : AMRpcHandler<C2G_EnterGame,G2C_EnterGame>
    {
        protected override async ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Gate)
            {
                Log.Error($"请求Scene错误,当前Scene为{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            
            //重复请求进入游戏
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeat;
                reply();
                return;
            }

            SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            if (sessionPlayerComponent == null)
            {
                response.Error = ErrorCode.ERR_SessionPlayerError;
                reply();
                return;
            }

            Player player = Game.EventSystem.Get(sessionPlayerComponent.PlayerInstanceId) as Player;
            if (player == null || player.IsDisposed)
            {
                response.Error = ErrorCode.ERR_NonePlayer;
                reply();
                return;
            }

            long instanceId = session.InstanceId;
            
            using (session.AddComponent<SessionLockingComponent>())
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate,player.AccountId.GetHashCode()))
            {
                if (session.InstanceId != instanceId || player.IsDisposed)
                {
                    response.Error = ErrorCode.ERR_SessionPlayerError;
                    reply();
                    return;
                }

                if (session.GetComponent<SessionStateComponent>() != null && session.GetComponent<SessionStateComponent>().State == SessionState.Game)
                {
                    response.Error = ErrorCode.ERR_SessionStateError;
                    reply();
                    return;
                }

                //如果player已经在游戏逻辑服了 需要顶号操作
                //重点!! (session可以映射player) (但player不能映射session)
                //因为session跟多名玩家通讯 如果有多名玩家一直顶号操作
                if (player.PlayerState == PlayerState.Game)
                {
                    try
                    {
                        IActorResponse reqEnter = await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestEnterGameState());
                        if (reqEnter.Error == ErrorCode.ERR_Success)
                        {
                            //只需要确定Map服上面的 unit映射是存在的 代表是可以顶号操作的 直接返回成功
                            reply();
                            return;
                        }
                        
                        Log.Error($"二次登陆失败,错误码:{reqEnter.Error}/{reqEnter.Message}");
                        response.Error = ErrorCode.ERR_ReEnterGameError;
                        await DisconnectHelper.KickPlayer(player, true);
                        reply();
                        session?.Disconnect().Coroutine();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"二次登陆失败,错误日志{e}");
                        response.Error = ErrorCode.ERR_ReEnterGameError2;
                        await DisconnectHelper.KickPlayer(player, true);
                        reply();
                        session?.Disconnect().Coroutine();
                        throw;
                    }
                }
                else
                {
                    //正常情况
                    try
                    {
                        //GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
                        //gateMapComponent.Scene = await SceneFactory.Create(gateMapComponent, "GateMap", SceneType.Map);

                        //从数据库或者从缓存服中加载出Unit实体和相关组件
                        (bool isNewUnit, Unit unit) = await UnitHelper.LoadUnit(player);
                        unit.AddComponent<UnitGateComponent, long>(session.InstanceId);

                        
                        //玩家Unit上线后的初始化操作
                        await UnitHelper.InitUnit(unit, isNewUnit);
                        
                        long unitId = unit.Id;

                        StartSceneConfig mapSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Game");
                        await TransferHelper.Transfer(unit, mapSceneConfig.InstanceId, mapSceneConfig.Name);

                        player.UnitId = unitId;
                        response.UnitId = unitId;

                        reply();

                        SessionStateComponent sessionStateComponent = session.GetComponent<SessionStateComponent>();
                        if (sessionStateComponent == null)
                        {
                            sessionStateComponent = session.AddComponent<SessionStateComponent>();
                        }
                        //改变session的状态
                        sessionStateComponent.State = SessionState.Game;

                        //改变gate上的player映射状态
                        player.PlayerState = PlayerState.Game;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"角色进入游戏逻辑服出现问题 账号Id{player.AccountId} 角色Id{player.Id} 异常信息{e}");
                        response.Error = ErrorCode.ERR_EnterGameError;
                        reply();
                        await DisconnectHelper.KickPlayer(player,true);
                        session?.Disconnect().Coroutine();
                    }
                }
            }
            
            //
        }
    }
} 