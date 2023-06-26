namespace Xo.TaskTree.Unit.Tests;

public class StateManagerTests
{
    private static CancellationToken NewCancellationToken() => new CancellationToken();
	private readonly IStateManager _stateManager;

	public StateManagerTests(IStateManager stateManager) => this._stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));

	[Fact]
	public async Task IF_THEN_ELSE()
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
	public async Task IF_THEN_if_then_else_ELSE()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.RootIf<IY_OutConstBool_SyncService>()
			.Then<IY_InStr_OutConstInt_AsyncService>(
				configure => configure.MatchArg("<<arg>>"),
				then => then.If<IY_InInt_OutBool_SyncService>(configure: c => c.RequireResult()).Then<IY_InInt_OutBool_SyncService>().Else<IY_AsyncService>()
			)
			.Else<IY_InStr_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg>>")));
	}

	[Fact]
	public async Task THEN_THEN()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<IY_OutConstBool_SyncService>()
			.Then<IY_InBoolStr_OutConstInt_AsyncService>(c => c.MatchArg("<<arg>>").RequireResult())
			.Then<IY_InInt_OutBool_SyncService>(c => c.RequireResult());
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
				c => c.MatchArg(true).MatchArg("<<arg>>").Key("key-b")
			);
	}

	[Fact]
	public async Task KEY_HASH_then()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<IY_OutConstBool_SyncService>()
			.Key<IY_InBool_OutConstStr_AsyncService>(c => c.RequireResult())
			.Hash<IY_AsyncService, IY_InBoolStr_OutConstInt_AsyncService>(
				c => c.Key("key-a"),
				c => c.MatchArg(true).MatchArg("<<arg>>").Key("key-b"),
				then => then.Then<IY_InStr_AsyncService>(c => c.MatchArg("<<arg>>"))
			);
	}

	[Fact]
	public async Task PATH()
	{
		var cancellationToken = NewCancellationToken();

		var mn = this._stateManager
			.Root<IY_OutConstBool_SyncService>()
			.Key<IY_InBool_OutConstStr_AsyncService>(c => c.RequireResult())
			.Hash<IY_AsyncService, IY_InBoolStr_OutConstInt_AsyncService>(
				c => c.Key("key-a"),
				c => c.MatchArg(true).MatchArg("<<arg>>").Key("key-b"),
				then => then.Then<IY_InStr_AsyncService>(c => c.MatchArg("<<arg>>"))
			);
	}
}