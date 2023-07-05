namespace Xo.TaskTree.Unit.Tests.Mocks;

///// <summary>
/////   Test fn showing how a user/consumer might provide their own fn by extending BaseFn.
///// </summary>
public class IY_InStr_OutBool_AsyncService_Fn : BaseAsyncFn
{
	const string serviceParamName = "args";
	private readonly IY_InStr_OutBool_AsyncService _service;
	private readonly IMsgFactory _msgFactory;

	public IY_InStr_OutBool_AsyncService_Fn(
		IY_InStr_OutBool_AsyncService service,
		IMsgFactory msgFactory
	)
	{
		this._service = service ?? throw new ArgumentNullException(nameof(service));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	public override async Task<IMsg?> Invoke(
		IArgs param,
		IWorkflowContext? workflowContext = null
	)
	{
		var serviceArgs = (param[serviceParamName] as Msg<string>)!.GetData();

		if (string.IsNullOrEmpty(serviceArgs))
		{
			throw new OperationCanceledException($"Invalid param provided for service {nameof(IY_InStr_OutBool_AsyncService)}");
		}

		// One of the strengths of user-defined fn is the ability to modify the result of a Task before it gets passed to the next Task.
		serviceArgs += "some modification for the purpose of demoing an alteration.";

		var result = await this._service.GetBoolAsync(serviceArgs);

		return this._msgFactory.Create<bool>(result, this._NextParamName);
	}
}

///// <summary>
/////   Test fn showing how a user/consumer might provide their own fn by extending BaseFn.
///// </summary>
public class TestStrategy2 : BaseAsyncFn
{
	const string serviceParamName = "args";
	private readonly IY_InStr_OutInt_AsyncService _service;
	private readonly IMsgFactory _msgFactory;

	public TestStrategy2(
		IY_InStr_OutInt_AsyncService service,
		IMsgFactory msgFactory
	)
	{
		this._service = service ?? throw new ArgumentNullException(nameof(service));
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	public override async Task<IMsg?> Invoke(
		IArgs param,
		IWorkflowContext? workflowContext = null
	)
	{
		var serviceArgs = this.Cast<Msg<string>>(this.SafeGet(param, serviceParamName)).GetData();
		if (string.IsNullOrEmpty(serviceArgs))
		{
			throw new OperationCanceledException($"Invalid param provided for service {nameof(IY_InStr_OutInt_AsyncService)}");
		}

		// One of the strengths of user-defined fn is the ability to modify the result of a Task before it gets passed to the next Task.
		serviceArgs += "some modification for the purpose of demoing an alteration.";

		var result = await this._service.GetIntAsync(serviceArgs);

		return this._msgFactory.Create<int>(result, this._NextParamName);
	}
}
