namespace Xo.TaskTree.Abstractions;

public interface ICoreNodeBuilder
{
	bool HasParam(string paramName); 
	IFunctitect Functitect { get; }
	Type? FunctoryType { get; }
	ICoreNodeBuilder RequireResult(bool requiresResult = true);
	ICoreNodeBuilder AddContext(IWorkflowContext? context);
	ICoreNodeBuilder AddFunctory(IAsyncFunctory functory);
	ICoreNodeBuilder AddFunctory(ISyncFunctory functory);
	ICoreNodeBuilder AddFunctory(Func<IArgs, Task<IMsg?>> fn);
	ICoreNodeBuilder AddFunctory(Func<IArgs, IMsg?> fn);
	ICoreNodeBuilder AddFunctory(Func<IWorkflowContext, IMsg?> fn);
	ICoreNodeBuilder AddArg(params IMsg[] msgs);
	ICoreNodeBuilder AddArg(params INode[] nodes);
	ICoreNodeBuilder AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs);
	ICoreNodeBuilder SetExceptionHandler(Func<Exception, Task> handler);
	ICoreNodeBuilder SetExceptionHandler(Action<Exception> handler);
	INode Build();
}