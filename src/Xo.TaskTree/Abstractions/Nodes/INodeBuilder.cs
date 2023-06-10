namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Builder for constructing a nodes. 
///   The service is retrieved using <see cref="IServiceCollection"/>, and therefore must be registered.
///   WARNING: Reflection is used for the construction of a functory factory, this builder should therefore only be used when necessary :)
/// </summary>
public interface INodeBuilder
{
	bool RequiresResult { get; }
	bool HasParam(string paramName);

	INodeBuilder RequireResult(bool requiresResult = true);
	INodeBuilder AddContext(IWorkflowContext? context);

	INodeBuilder AddFunctory<T>(string? nextParamName = null);
	INodeBuilder AddFunctory<TService, TArg>(TArg arg, string? nextParamName = null);
	INodeBuilder AddFunctory(Func<IDictionary<string, IMsg>, Func<Task<IMsg?>>> fn);
	INodeBuilder AddFunctory(IAsyncFunctory functory);
	INodeBuilder AddFunctory(ISyncFunctory functory);
	INodeBuilder AddFunctory(Func<IDictionary<string, IMsg>, Func<IMsg?>> fn);
	INodeBuilder AddFunctory(Func<IWorkflowContext, Func<IMsg?>> fn);
	INodeBuilder AddFunctory(
		Func<IDictionary<string, IMsg>, Func<IMsg?>> fn,
		string nextParamName
	);

	INodeBuilder AddArg(params IMsg[] msgs);
	INodeBuilder AddArg(params INode[] nodes);
	INodeBuilder AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs);
	INodeBuilder AddArg<TArg>(TArg arg);
	INodeBuilder AddArg<TArgData>(
		TArgData data,
		string paramName
	);
	INodeBuilder AddArg<TService, TArg>(TArg arg);
	INodeBuilder AddArg<TService>();
	INodeBuilder AddArg(
		Type serviceType,
		IMsg[]? serviceTypeArgs = null
	);

	/// <summary>
	///   Sets an asynchronous exception handler to be called if this node's functory fails.
	/// </summary>
	/// <remarks>Method can be chained. This handler is called before the synchronous exception handler, if one is provided.</remarks>
	/// <param name="handler"></param>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder SetExceptionHandler(Func<Exception, Task> handler);

	/// <summary>
	///   Sets an synchronous exception handler to be called if this node's functory fails.
	/// </summary>
	/// <remarks>Method can be chained. This handler is called after the asynchronous exception handler, if one is provided.</remarks>
	/// <param name="handler"></param>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder SetExceptionHandler(Action<Exception> handler);

	INode Build();
}