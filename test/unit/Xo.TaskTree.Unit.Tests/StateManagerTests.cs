namespace Xo.TaskTree.Unit.Tests;

public class StateManagerTests
{
    private static CancellationToken NewCancellationToken() => new CancellationToken();
	private static IStateManager manager = null!;

	[Fact]
	public async Task IF_THEN_ELSE()
	{
		var cancellationToken = NewCancellationToken();

		var mn = manager
			.RootIf<IY_OutConstBool_SyncService>()
			.Then<IY_InStr_OutConstInt_AsyncService>(
				then => then.Then<IY_InInt_OutBool_SyncService>(configure: c => c.RequireResult()),
				configure => configure.MatchArg("<<arg>>")
			)
			.Else<IY_InStr_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg>>")));

	}

	[Fact]
	public async Task THEN()
	{
		var cancellationToken = NewCancellationToken();

		var mn = manager
			.Root<IY_OutConstBool_SyncService>()
			.Then<IY_InBoolStr_OutConstInt_AsyncService>(configure: c => c.MatchArg("<<arg>>").RequireResult())
			.Then<IY_InInt_OutBool_SyncService>(configure: c => c.RequireResult());
	}
}