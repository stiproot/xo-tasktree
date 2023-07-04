namespace Xo.TaskTree.Abstractions;

public interface ICoreNodeBuilder
{
	ICoreNodeBuilder Configure(Action<INodeConfigurationBuilder> configure);
	ICoreNodeBuilder Configure(INodeConfiguration nodeConfiguration);
	bool HasParam(string paramName); 
	IFnFactory FnFactory { get; }
	Type? ServiceType { get; }
	ICoreNodeBuilder AddNodeEdge(INodeEdge nodeEdge);
	ICoreNodeBuilder AddFn(IAsyncFn fn);
	ICoreNodeBuilder AddFn(ISyncFn fn);
	ICoreNodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn);
	ICoreNodeBuilder AddFn(Func<IArgs, IMsg?> fn);
	ICoreNodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn);
	ICoreNodeBuilder SetExceptionHandler(Func<Exception, Task> handler);
	ICoreNodeBuilder SetExceptionHandler(Action<Exception> handler);
	ICoreNodeBuilder AddInvoker(IInvoker invoker);
	ICoreNodeBuilder AddController(IController controller);
	// ICoreNodeBuilder RequireResult(bool requiresResult = true);
	INode Build();

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