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
			.RootIf<ISvc_OutConstBool_SyncService>()
			.Then<ISvc_InStr_OutConstInt_AsyncService>(
				configure => configure.MatchArg("<<arg-1>>"),
				then => then.Then<ISvc_InInt_OutBool_SyncService>(configure: c => c.RequireResult())
			)
			.Else<ISvc_InStr_AsyncService>(c => c.MatchArg<ISvc_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg-2>>")));
		
		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<bool>(); 

		Assert.True(d);
	}

	[Fact]
	public async Task IF_not_null_THEN_then()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.IsNotNull<ISvc_OutObj_SyncService>()
			.Then<ISvc_InObj_OutConstInt_AsyncService>(c => c.AddArg(new object(), "arg1"))
			.Else<ISvc_InStr_AsyncService>(c => c.AddArg("<<args>>", "args3"));
		
		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task IF_not_null_THEN_requires_result()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.IsNotNull<ISvc_OutObj_SyncService>()
			.Then<ISvc_InObj_OutConstInt_AsyncService>(c => c.RequireResult())
			.Else<ISvc_InStr_AsyncService>(c => c.AddArg("<<args>>", "args3"));
		
		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task IF_then_ELSE_args()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<ISvc_OutConstFalseBool_SyncService>()
			.Then<ISvc_InStr_AsyncService>(configure => configure.MatchArg("<<arg-1>>"))
			.Else<ISvc_InStr_OutConstInt_AsyncService>(c => c.MatchArg<ISvc_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg-2>>")));
		
		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task IF_THEN_if_then()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<ISvc_OutConstBool_SyncService>()
			.Then<ISvc_InStr_OutConstInt_AsyncService>(
				configure => configure.MatchArg("<<arg>>"),
				then => then.If<ISvc_InInt_OutBool_SyncService>(configure: c => c.RequireResult()).Then<ISvc_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.RequireResult()).Else<ISvc_AsyncService>()
			)
			.Else<ISvc_InStr_AsyncService>(c => c.MatchArg<ISvc_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg>>")));

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<string>(); 

		Assert.NotNull(d);
		Assert.IsType<Guid>(Guid.Parse(d));
	}

	[Fact]
	public async Task IF_ELSE_args()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<ISvc_OutConstFalseBool_SyncService>()
			.Else<ISvc_InStr_OutConstInt_AsyncService>(c => c.MatchArg<ISvc_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg>>")));

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task promised_ARGS_ARGS()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<ISvc_InBoolStr_OutConstInt_AsyncService>(c => 
				c
					.MatchArg<ISvc_OutConstBool_SyncService>()
					.MatchArg<ISvc_InBool_OutConstStrIfFalseElseDynamicStr_AsyncService>(c => c.MatchArg(true))
			);

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task ARGS_ARGS()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<ISvc_InBoolStr_OutConstInt_AsyncService>(c => 
				c
					.MatchArg("<<args>>")
					.MatchArg(true)
			);

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task KEY_HASH()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<ISvc_OutConstBool_SyncService>()
			.Key<ISvc_InBool_OutConstStr_AsyncService>(c => c.RequireResult())
			.Hash<ISvc_AsyncService, ISvc_InBoolStr_OutConstInt_AsyncService>(
				c => c.Key("key-a"),
				c => c.MatchArg(true).MatchArg("<<arg>>").Key("<<str>>")
			);

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task KEY_HASH_THEN()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<ISvc_OutConstBool_SyncService>()
			.Key<ISvc_InBool_OutConstStr_AsyncService>(c => c.RequireResult())
			.Hash<ISvc_InBoolStr_OutConstInt_AsyncService, ISvc_AsyncService>(
				c => c.MatchArg(true).MatchArg("<<arg>>").Key("<<str>>"),
				c => c.Key("key-a"),
				then => then.Then<ISvc_InStr_OutConstInt_AsyncService>(c => c.MatchArg("<<arg>>"))
			);

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task PATH_two_nodes()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<ISvc_OutConstBool_SyncService>()
			.Path<ISvc_InBool_OutConstStr_AsyncService, ISvc_InStr_OutConstInt_AsyncService>(
				c => c.RequireResult(),
				c => c.RequireResult()
			);

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}

	[Fact]
	public async Task PATH_three_nodes()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<ISvc_OutConstBool_SyncService>()
			.Path<ISvc_InBool_OutConstStr_AsyncService, ISvc_InStr_OutConstInt_AsyncService, ISvc_InInt_OutConstInt_AsyncService>(
				c => c.RequireResult(),
				c => c.RequireResult(),
				c => c.RequireResult()
			);

		var n = mn.Build();

		var msgs = await n.Resolve(cancellationToken);
		var msg = msgs.First(); 
		var d = msg.Data<int>(); 

		Assert.Equal(1, d);
	}
}
