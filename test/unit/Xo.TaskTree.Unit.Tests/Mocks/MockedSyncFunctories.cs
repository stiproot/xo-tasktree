namespace Xo.TaskTree.Unit.Tests.Mocks;

/// <summary>
///   Test functory extending the  
/// </summary>
public class TestSyncFunctory : BaseSyncFunctory
{
	private const string paramName = "sleep";
	private readonly IY_InInt_OutBool_SyncService _synchronousService;
	private readonly IMsgFactory _msgFactory;

	public TestSyncFunctory(
		IY_InInt_OutBool_SyncService synchronousService,
		IMsgFactory msgFactory
	)
	{
		this._synchronousService = synchronousService ?? throw new ArgumentNullException(nameof(synchronousService));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	public override IMsg? CreateFunc(
		IArgs param,
		IWorkflowContext? context = null
	)
	{
		// var arg = (param[paramName] as Msg<int>).GetData();
		var arg = this.Cast<Msg<int>>(this.SafeGet(param, paramName)).GetData();

		var result = this._synchronousService.GetBool(arg);

		return this._msgFactory.Create<bool>(result, this._NextParamName);
	}
}
