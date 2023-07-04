//namespace Xo.TaskTree.Abstractions;

//public interface INode
//{
	//string Id { get; }
	//IFn Fn { get; }
	//bool RequiresResult { get; }
	//bool IgnoresPromisedResults { get; }
	//bool IsSync { get; }

	//INode RequireResult(bool requiresResult = true);
	//INode IgnorePromisedResults(bool ignorePromisedResults = true);
	//bool HasParam(string paramName);
	//int ArgCount();

	//INode SetId(string id);
	//INode SetContext(IWorkflowContext? context);
	//INode SetLogger(ILogger logger);

	//INode SetNodeEdge(INodeEdge nodeEdge);
	//INode SetInvoker(IInvoker invoker);
	//INode SetController(IController controller);

	//INode SetNodevaluator(INodevaluator nodevaluator);
	//INode RunNodesInLoop();

	//INode SetFn(IAsyncFn fn);
	//INode SetFn(Func<IArgs, Task<IMsg?>> fn);
	//INode SetFn(ISyncFn fn);
	//INode SetFn(Func<IArgs, IMsg?> fn);
	//INode SetFn(Func<IWorkflowContext, IMsg?> fn);

	//INode AddArg(params INode[] args);
	//INode AddArg(params IMsg?[] args);
	//INode AddArg<T>(
		//T data,
		//string paramName
	//);
	//INode AddArg(params Func<IWorkflowContext, IMsg>[] args);

	//INode SetExceptionHandler(Func<Exception, Task> handler);
	//INode SetExceptionHandler(Action<Exception> handler);

	//Task<IMsg?[]> Run(CancellationToken cancellationToken);
	//void Validate();
	//Task ResolvePromisedParams(CancellationToken cancellationToken);
	//void AddContextParamResultsToParams();
	//Task<IMsg?[]> ResolveFn(CancellationToken cancellationToken);
	//Task HandleException(Exception ex);
//}
