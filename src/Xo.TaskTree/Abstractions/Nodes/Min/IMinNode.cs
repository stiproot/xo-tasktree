namespace Xo.TaskTree.Abstractions;

public interface IMinNode
{
	INodeConfiguration NodeConfiguration { get; }
	INodeEdge? NodeEdge { get; }
	IFn Fn { get; }
	int ArgCount();
	IMinNode SetNodeConfiguration(INodeConfiguration nodeConfiguration);
	IMinNode SetNodeEdge(INodeEdge nodeEdge);
	IMinNode SetInvoker(IInvoker invoker);
	IMinNode SetController(IController controller);
	IMinNode SetNodevaluator(INodevaluator nodevaluator);
	IMinNode RunNodesInLoop();
	IMinNode SetFn(IAsyncFn fn);
	IMinNode SetFn(ISyncFn fn);
	IMinNode SetExceptionHandler(Func<Exception, Task> handler);
	IMinNode SetExceptionHandler(Action<Exception> handler);
	Task<IMsg?[]> Run(CancellationToken cancellationToken);
	void Validate();

	// string Id { get; }
	// bool RequiresResult { get; }
	// bool IgnoresPromisedResults { get; }
	// bool IsSync { get; }
	// INode RequireResult(bool requiresResult = true);
	// INode IgnorePromisedResults(bool ignorePromisedResults = true);
	// bool HasParam(string paramName);
	// INode SetId(string id);
	// INode SetContext(IWorkflowContext? context);
	// INode SetLogger(ILogger logger);
	// INode SetFn(Func<IArgs, Task<IMsg?>> fn);
	// INode SetFn(Func<IArgs, IMsg?> fn);
	// INode SetFn(Func<IWorkflowContext, IMsg?> fn);
	// INode AddArg(params INode[] args);
	// INode AddArg(params IMsg?[] args);
	// INode AddArg<T>(
		// T data,
		// string paramName
	// );
	// INode AddArg(params Func<IWorkflowContext, IMsg>[] args);
	// Task ResolvePromisedParams(CancellationToken cancellationToken);
	// void AddContextParamResultsToParams();
	// Task<IMsg?[]> ResolveFn(CancellationToken cancellationToken);
	// Task HandleException(Exception ex);
}
