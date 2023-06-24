namespace Xo.TaskTree.Abstractions;

public abstract class BaseNodeBuilder
{
	protected ILogger? _Logger;
	protected IAsyncFunctory? _AsyncFunctory;
	protected ISyncFunctory? _SyncFunctory;
	protected IWorkflowContext? _Context;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected readonly IList<IMsg> _Params = new List<IMsg>();
	protected readonly List<INode> _PromisedParams = new List<INode>();
	protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();
	public string Id { get; internal set; } = Guid.NewGuid().ToString();
	public bool RequiresResult { get; internal set; } = false;

	public BaseNodeBuilder(
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	)
	{
		this._Logger = logger;
		if (id is not null) this.Id = id;
		this._Context = context;
	}
}