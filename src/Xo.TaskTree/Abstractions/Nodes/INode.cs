namespace Xo.TaskTree.Abstractions;

/// <summary>
///   A node representing a unit of work to be performed.
///   Nodes are "strung" together to form workflows.
/// </summary>
public interface INode
{
	/// <summary>
	///   The unique identifier for the node. 
	/// </summary>
	/// <returns><see cref="string"/></returns>
	string Id { get; }

	IFunctory Functory { get; }

	bool RequiresResult { get; }

	INode RequireResult(bool requiresResult = true);

	bool HasParam(string paramName);

	Task ResolvePromisedParams(CancellationToken cancellationToken);

	void AddContextParamResultsToParams();

	Task<IMsg?> ResolveFunctory(CancellationToken cancellationToken);

	Task HandleException(Exception ex);

	INodeEdge? NodeEdge{ get; }

	/// <summary>
	///   Flag specifying if a synchronous functory is set. 
	/// </summary>
	/// <remarks>
	///   Functories can be of type <see cref="IFunctory"/> or <see cref="ISyncFunctory"/>. 
	/// </remarks>
	/// <returns><see cref="bool"/></returns>
	bool IsSync { get; }

	INode SetNodeEdge(INodeEdge nodeEdge);

	/// <summary>
	///   Sets the implementation of <see cref="INodevaluator"/> that runs the <see cref="INode"/>s. 
	///   nodes will be run in parallel by default.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="nodevaluator">The runner.</param>
	/// <returns><see cref="INode"/></returns>
	INode SetNodevaluator(INodevaluator nodevaluator);

	/// <summary>
	///   Sets the implementation of <see cref="INodevaluator"/> to <see cref="LoopNodeEvaluator"/>. 
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <returns><see cref="INode"/></returns>
	INode RunNodesInLoop();

	/// <summary>
	///   Sets this nodes <see cref="IFunctory"/>. 
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="functory">The functory that will produce a asynchronous operation to run.</param>
	/// <returns><see cref="INode"/></returns>
	INode SetFunctory(IAsyncFunctory functory);

	/// <summary>
	///   Builds a <see cref="IAsyncFunctory"/> around a function pointer.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="fn">The function pointer that will produce an asynchronous operation to run.</param>
	/// <returns><see cref="INode"/></returns>
	INode SetFunctory(Func<IDictionary<string, IMsg>, Func<Task<IMsg?>>> fn);

	/// <summary>
	///   Sets this nodes <see cref="ISyncFunctory"/>.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="functory">The functory that will produce a synchronous operation to run.</param>
	/// <returns><see cref="INode"/></returns>
	INode SetFunctory(ISyncFunctory functory);

	/// <summary>
	///   Builds a <see cref="ISyncFunctory"/> around a function pointer.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="fn">The function pointer that will produce a synchronous operation to run.</param>
	/// <returns><see cref="INode"/></returns>
	INode SetFunctory(Func<IDictionary<string, IMsg>, Func<IMsg?>> fn);

	INode SetFunctory(Func<IWorkflowContext, Func<IMsg>> fn);

	/// <summary>
	///   Sets this node's <see cref="IWorkflowContext"/>. The context is shared between all nodes in a workflow.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="IWorkflowContext"></param>
	/// <returns><see cref="INode"/></returns>
	INode SetContext(IWorkflowContext? context);

	/// <summary>
	///  Sets the Id of this node. 
	/// </summary>
	/// <remarks>
	///   When building up a workflow whereby nodes separated by two nodes or more need to share results, 
	///   in some instances it can be helpful to provide your own id.</remarks>
	/// <param name="id"></param>
	/// <returns><see cref="INode"/></returns>
	INode SetId(string id);

	/// <summary>
	///  Sets the an <see cref="ILogger"/> implementation. 
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="INode"/></returns>
	INode SetLogger(ILogger logger);

	/// <summary>
	///   Adds nodes to be run before this one, which typically yield results to be used as arguments for this nodes functory.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="nodes"></param>
	/// <returns><see cref="INode"/></returns>
	INode AddArg(params INode[] nodes);

	/// <summary>
	///   Adds a <see cref="IMsg"/>, which houses data required to be used as an argument for this nodes functory.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="msgs"></param>
	/// <returns><see cref="INode"/></returns>
	INode AddArg(params IMsg[] msgs);

	/// <summary>
	///   Creates a <see cref="IMsg"/>, out of type T. 
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="data"></param>
	/// <param name="paramName"></param>
	/// <returns><see cref="INode"/></returns>
	INode AddArg<T>(
		T data,
		string paramName
	);

	/// <summary>
	///   Adds a delegate that represents a query to be run against the <see cref="IWorkflowContext"/>, used to extract results from previous nodes,
	///   to be used in this nodes functory.
	/// </summary>
	/// <remarks>Method can be chained.</remarks>
	/// <param name="contextArgs"></param>
	/// <returns><see cref="INode"/></returns>
	INode AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs);

	/// <summary>
	///   Sets an asynchronous exception handler to be called if this node's functory fails.
	/// </summary>
	/// <remarks>Method can be chained. This handler is called before the synchronous exception handler, if one is provided.</remarks>
	/// <param name="handler"></param>
	/// <returns><see cref="INode"/></returns>
	INode SetExceptionHandler(Func<Exception, Task> handler);

	/// <summary>
	///   Sets an synchronous exception handler to be called if this node's functory fails.
	/// </summary>
	/// <remarks>Method can be chained. This handler is called after the asynchronous exception handler, if one is provided.</remarks>
	/// <param name="handler"></param>
	/// <returns><see cref="INode"/></returns>
	INode SetExceptionHandler(Action<Exception> handler);

	/// <summary>
	///   Core run operation. 
	///   Runs parameter nodes, consolidates results into an argument list, and then invokes this node's functory.
	/// </summary>
	/// <remarks>Uses the functory factory provided to produce the primary functory.</remarks>
	/// <returns><see cref="IMsg"/></returns>
	Task<IMsg?> Run(CancellationToken cancellationToken);

	/// <summary>
	///   Core validation operation. Validates that a either a asynchronous or synchronous functory has been provided.
	/// </summary>
	void Validate();
}
