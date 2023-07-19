namespace Xo.TaskTree.Abstractions;

public abstract class BaseNodeBuilder
{
	protected readonly IFnFactory _FnFactory;
	protected ILogger? _Logger;
	protected IAsyncFn? _AsyncFn;
	protected ISyncFn? _SyncFn;
	protected INodeEdge? _NodeEdge;
	protected INodeConfiguration? _NodeConfiguration;
	protected Func<Exception, Task>? _ExceptionHandlerAsync;
	protected Action<Exception>? _ExceptionHandler;
	protected IController? _Controller;
	protected INodeResolver _Resolver;

	public BaseNodeBuilder(
		IFnFactory fnFactory,
		INodeResolver nodeResolver,
		ILogger? logger = null
	)
	{
		this._FnFactory = fnFactory ?? throw new ArgumentNullException(nameof(fnFactory));
		this._Resolver = nodeResolver ?? throw new ArgumentNullException(nameof(nodeResolver));
		this._Logger = logger;
	}
}