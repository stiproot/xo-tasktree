namespace Xo.TaskTree.Unit.Tests.Mocks;

/// <summary>
///   Test fn extending the  
/// </summary>
public class IQ_InInt_OutBool_Fn : BaseFn
{
	private const string paramName = "sleep";
	private readonly IY_InInt_OutBool_SyncService _synchronousService;
	private readonly IMsgFactory _msgFactory;
	public override bool IsSync => true;

	public IQ_InInt_OutBool_Fn(
		IY_InInt_OutBool_SyncService synchronousService,
		IMsgFactory msgFactory
	)
	{
		this._synchronousService = synchronousService ?? throw new ArgumentNullException(nameof(synchronousService));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	public override Task<IMsg?> InvokeAsync(
		IArgs param,
		IWorkflowContext? workflowContext = null
	)
		=> throw new NotSupportedException();

	public override IMsg? Invoke(
		IArgs param,
		IWorkflowContext? workflowContext = null
	)
	{
		var arg = this.Cast<Msg<int>>(this.SafeGet(param, paramName)).GetData();

		var result = this._synchronousService.GetBool(arg);

		return this._msgFactory.Create<bool>(result, this._NextParamName);
	}
}
