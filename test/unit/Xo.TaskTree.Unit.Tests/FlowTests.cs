namespace Xo.TaskTree.Unit.Tests;

public class CoreUseCaseTests
{
    private static CancellationToken NewCancellationToken() => new CancellationToken();
	private static IFlowBuilder builder = null!;

	[Fact]
	public async Task IF_THEN_ELSE()
	{
		var cancellationToken = NewCancellationToken();

		var n = builder
			.If<IY_OutConstBool_SyncService>()
			.Then<IY_InStr_OutConstInt_AsyncService>(
				then => then.Then<IY_InInt_OutBool_SyncService>(c => c.RequireResult()),
				configure => configure.MatchArg("<<arg>>")
			)
			.Else<IY_InStr_AsyncService>(c => c.MatchArg<IY_InStr_OutConstStr_AsyncService>(c => c.MatchArg("<<arg>>")))
			.Build();

		IMsg?[]? r = await n.Run(cancellationToken);

		var d = (r[0] as Msg<int>)!.GetData();
	}
}