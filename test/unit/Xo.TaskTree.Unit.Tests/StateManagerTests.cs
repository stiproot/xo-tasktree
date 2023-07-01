namespace Xo.TaskTree.Unit.Tests;

public class StateManagerTests
{
	private static CancellationToken NewCancellationToken() => new CancellationToken();
	private readonly IStateManager _stateManager;

	public StateManagerTests(IStateManager stateManager) => this._stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));

	[Fact]
	public async Task IF_THEN_then()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<IY_OutConstBool_SyncService>()
			.Then<IY_InStr_OutConstInt_AsyncService>(
				configure => configure.MatchArg("<<arg-1>>"),
				then => then.Then<IY_InInt_OutBool_SyncService>(configure: c => c.RequireResult())
			)
			.Else<IY_InStr_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg-2>>")));
		
		var n = mn.Build();

		var msgs = await n.Run(cancellationToken);
		var msg = msgs.First(); 
		var d = (msg as Msg<bool>)!.GetData();

		Assert.True(d);
	}

	[Fact]
	public async Task IF_then_ELSE_args()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<IY_OutConstFalseBool_SyncService>()
			.Then<IY_InStr_AsyncService>(configure => configure.MatchArg("<<arg-1>>"))
			.Else<IY_InStr_OutConstInt_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg-2>>")));
		
		var n = mn.Build();

		var msgs = await n.Run(cancellationToken);
		var msg = msgs.Second(); 
		var d = (msg as Msg<int>)!.GetData();

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task IF_THEN_if_then()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<IY_OutConstBool_SyncService>()
			.Then<IY_InStr_OutConstInt_AsyncService>(
				configure => configure.MatchArg("<<arg>>"),
				then => then.If<IY_InInt_OutBool_SyncService>(configure: c => c.RequireResult()).Then<IY_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.RequireResult()).Else<IY_AsyncService>()
			)
			.Else<IY_InStr_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg>>")));

		var n = mn.Build();

		var msgs = await n.Run(cancellationToken);
		var msg = msgs.First(); 
		var d = (msg as Msg<string>)!.GetData();

		Assert.NotNull(d);
		Assert.IsType<Guid>(Guid.Parse(d));
	}

	[Fact]
	public async Task IF_ELSE_args()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<IY_OutConstFalseBool_SyncService>()
			.Else<IY_InStr_OutConstInt_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg>>")));

		var n = mn.Build();

		var msgs = await n.Run(cancellationToken);
		var msg = msgs.First(); 
		var d = (msg as Msg<int>)!.GetData();

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task promised_ARGS_ARGS()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<IY_InBoolStr_OutConstInt_AsyncService>(c => 
				c
					.MatchArg<IY_OutConstBool_SyncService>()
					.MatchArg<IY_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.MatchArg(true))
			);

		var n = mn.Build();

		var msgs = await n.Run(cancellationToken);
		var msg = msgs.First(); 
		var d = (msg as Msg<int>)!.GetData();

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task ARGS_ARGS()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<IY_InBoolStr_OutConstInt_AsyncService>(c => 
				c
					.MatchArg("<<args>>")
					.MatchArg(true)
			);

		var n = mn.Build();

		var msgs = await n.Run(cancellationToken);
		var msg = msgs.First(); 
		var d = (msg as Msg<int>)!.GetData();

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task KEY_HASH()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<IY_OutConstBool_SyncService>()
			.Key<IY_InBool_OutConstStr_AsyncService>(c => c.RequireResult())
			.Hash<IY_AsyncService, IY_InBoolStr_OutConstInt_AsyncService>(
				c => c.Key("key-a"),
				c => c.MatchArg(true).MatchArg("<<arg>>").Key("<<str>>")
			);

		var n = mn.Build();

		var msgs = await n.Run(cancellationToken);
		var msg = msgs.Second(); 
		var d = (msg as Msg<int>)!.GetData();

		Assert.Equal(1, d);
	}

	//[Fact]
	//public async Task KEY_HASH_then()
	//{
		//var cancellationToken = NewCancellationToken();

		//var mn = this._stateManager
			//.Root<IY_OutConstBool_SyncService>()
			//.Key<IY_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.RequireResult())
			//.Hash<IY_AsyncService, IY_InBoolStr_OutConstInt_AsyncService>(
				//c => c.Key("key-a"),
				//c => c.MatchArg(true).MatchArg("<<arg>>").Key("key-b"),
				//then => then.Then<IY_InStr_AsyncService>(c => c.MatchArg("<<arg>>"))
			//);
	//}

	//[Fact]
	//public async Task PATH()
	//{
		//var cancellationToken = NewCancellationToken();

		//var mn = this._stateManager
			//.Root<IY_OutConstBool_SyncService>()
			//.Key<IY_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.RequireResult())
			//.Hash<IY_AsyncService, IY_InBoolStr_OutConstInt_AsyncService>(
				//c => c.Key("key-a"),
				//c => c.MatchArg(true).MatchArg("<<arg>>").Key("key-b"),
				//then => then.Then<IY_InStr_AsyncService>(c => c.MatchArg("<<arg>>"))
			//);
	//}
}