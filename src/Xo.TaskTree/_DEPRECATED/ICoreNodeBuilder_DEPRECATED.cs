//namespace Xo.TaskTree.Abstractions;

//public interface ICoreNodeBuilder
//{
	//bool HasParam(string paramName); 
	//IFnFactory FnFactory { get; }
	//Type? ServiceType { get; }
	//ICoreNodeBuilder RequireResult(bool requiresResult = true);
	//ICoreNodeBuilder AddContext(IWorkflowContext? context);
	//ICoreNodeBuilder AddFn(IAsyncFn fn);
	//ICoreNodeBuilder AddFn(ISyncFn fn);
	//ICoreNodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn);
	//ICoreNodeBuilder AddFn(Func<IArgs, IMsg?> fn);
	//ICoreNodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn);
	//ICoreNodeBuilder AddArg(params IMsg[] msgs);
	//ICoreNodeBuilder AddArg(params INode[] nodes);
	//ICoreNodeBuilder AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs);
	//ICoreNodeBuilder SetExceptionHandler(Func<Exception, Task> handler);
	//ICoreNodeBuilder SetExceptionHandler(Action<Exception> handler);
	//INode Build();
//}