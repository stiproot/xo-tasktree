namespace Xo.TaskTree.Abstractions;

/// <summary>
///   A factory that produces a Task (fn) that is used by a <see cref="INode"/>.
/// </summary>
/// <remarks>
///   Strategies are how the TaskWorkflowEngine encapsulates operations that are to be run within a <see cref="INode"/>.
/// </remarks>
public interface IFn
{
	/// <summary>
	///   The type of the service that a fn will be built around. 
	/// </summary>
	Type? ServiceType { get; }

	IFn SetServiceType(Type serviceType);

	/// <summary>
	///   A factory that produces a Task (fn) that is used by a <see cref="INode"/>.
	/// </summary>
	IFn SetNextParamName(string? nextParamName = null);

	/// <inheritdoc />
	IMsg SafeGet(
		IArgs pairs,
		string key
	);

	/// <summary>
	///  ... 
	/// </summary>
	/// <returns><see cref="Task{IMsg}"/></returns>
	Task<IMsg?> InvokeAsync(
		IArgs args,
		IWorkflowContext? workflowContext = null
	);

	/// <summary>
	///  ... 
	/// </summary>
	/// <returns><see cref="IMsg"/></returns>
	IMsg? Invoke(
		IArgs args,
		IWorkflowContext? workflowContext = null
	);
}