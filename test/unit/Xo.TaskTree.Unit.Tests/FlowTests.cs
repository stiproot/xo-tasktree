namespace Xo.TaskTree.Unit.Tests;

public class CoreUseCaseTests
{
    private static CancellationToken NewCancellationToken() => new CancellationToken();
	private static IFlowBuilder builder = null!;

	[Fact]
	public async Task IfThenElse()
	{
		var n = builder
			.If<IY_OutConstBool_SyncService>()
			.Then<IY_InStr_OutConstInt_AsyncService>(
				then => then.Then<IY_InInt_OutBool_SyncService>(configure => configure.RequireResult()),
				configure => configure.MatchArg("<<arg>>")
			)
			.Else<IY_InStr_AsyncService>(configure => configure.MatchArg<IY_InStr_OutConstStr_AsyncService>(configure => configure.MatchArg("<<arg>>")))
			.Build();
		

	}
}