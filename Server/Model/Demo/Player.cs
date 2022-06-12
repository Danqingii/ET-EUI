namespace ET
{
	public enum PlayerState
	{
		Disconnect,
		Gate,
		Game,
	}

	public sealed class Player : Entity,IAwake<string>, IAwake<long,long>
	{
		public long AccountId { get; set; }
		
		public long UnitId { get; set; }

		public long SessionInstanceId;

		public PlayerState PlayerState;
	}
}