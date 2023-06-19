namespace Xo.TaskTree.Abstractions;

public abstract class BaseBranchNode : BaseNode, IBranchNode
{
	protected virtual void PropogateMsg(
		IMsg? msg,
		INode next
	)
	{
		var targetServiceType = next.Functory.ServiceType;
		var targetMethodInfo = targetServiceType!.GetMethods().First();
		var targetMethodParams = targetMethodInfo.GetParameters();

		// How to get the param name here?
		var outstandingParams = targetMethodParams.Where(p => !next.HasParam(p.Name!)).ToArray();
		string paramName;

		if (outstandingParams.Length == 1) paramName = outstandingParams.First().Name!;
		else
		{
			var msgType = msg!.GetDataType();
			paramName = outstandingParams.First(p => p.ParameterType == msgType).Name!;
		}

		if (paramName is null) throw new ArgumentException("Unable to find parameter name for type TArg...");

		var clone = Functitect.CreateMsg(msg!.ObjectData, paramName);
		next.AddArg(clone);
	}

	protected virtual async Task<IMsg?[]> RunNext(
		IMsg? msg,
		INode next,
		CancellationToken cancellationToken
	)
	{
		next.SetContext(this._Context);
		next.Validate();
		await next.ResolvePromisedParams(cancellationToken);
		next.AddContextParamResultsToParams();

		if (next.RequiresResult)
		{
			// todo: handle propogating null...
			this.PropogateMsg(msg, next);
		}

		return await next.ResolveFunctory(cancellationToken);
	}

	protected virtual IMsg? CloneMsg(IMsg? msg)
	{
		if (msg is null) return null;

		var clone = Functitect.CreateMsg(msg.ObjectData, msg.ParamName);

		return clone;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="Node"/>. 
	/// </summary>
	public BaseBranchNode(
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(msgFactory, logger, id, context) { }
}