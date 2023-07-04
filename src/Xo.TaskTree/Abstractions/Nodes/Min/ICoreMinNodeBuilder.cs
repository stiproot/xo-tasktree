namespace Xo.TaskTree.Abstractions;

public interface ICoreMinNodeBuilder
{
	ICoreMinNodeBuilder Configure(Action<IMinNodeConfigurationBuilder> configure);
	bool HasParam(string paramName); 
	IFnFactory FnFactory { get; }
	Type? ServiceType { get; }
	// ICoreMinNodeBuilder RequireResult(bool requiresResult = true);
	// ICoreMinNodeBuilder AddContext(IWorkflowContext? context);
	ICoreMinNodeBuilder AddFn(IAsyncFn fn);
	ICoreMinNodeBuilder AddFn(ISyncFn fn);
	ICoreMinNodeBuilder AddFn(Func<IArgs, Task<IMsg?>> fn);
	ICoreMinNodeBuilder AddFn(Func<IArgs, IMsg?> fn);
	ICoreMinNodeBuilder AddFn(Func<IWorkflowContext, IMsg?> fn);
	// ICoreMinNodeBuilder AddArg(params IMsg[] msgs);
	// ICoreMinNodeBuilder AddArg(params INode[] nodes);
	// ICoreMinNodeBuilder AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs);
	ICoreMinNodeBuilder SetExceptionHandler(Func<Exception, Task> handler);
	ICoreMinNodeBuilder SetExceptionHandler(Action<Exception> handler);
	IMinNode Build();
}