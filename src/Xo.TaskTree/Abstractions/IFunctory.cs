namespace Xo.TaskTree.Abstractions;

/// <summary>
///   A factory that produces a Task (functory) that is used by a <see cref="INode"/>.
/// </summary>
/// <remarks>
///   Strategies are how the TaskWorkflowEngine encapsulates operations that are to be run within a <see cref="INode"/>.
/// </remarks>
public interface IFunctory
{
	/// <summary>
	///   The type of the service that a functory will be built around. 
	/// </summary>
	Type? ServiceType { get; }

	IFunctory SetServiceType(Type serviceType);

	/// <summary>
	///   A factory that produces a Task (functory) that is used by a <see cref="INode"/>.
	/// </summary>
	IFunctory SetNextParamName(string? nextParamName = null);

	/// <summary>
	///  ... 
	/// </summary>
	/// <param name="pairs"></param>
	/// <param name="key"></param>
	IMsg SafeGet(
		IArgs pairs,
		string key
	);

	/// <summary>
	///  ... 
	/// </summary>
	/// <param name="engineMessage"></param>
	T Cast<T>(IMsg engineMessage);

	/// <summary>
	///  ... 
	/// </summary>
	/// <returns><see cref="ISyncFunctory"/></returns>
	ISyncFunctory AsSync();

	/// <summary>
	///  ... 
	/// </summary>
	/// <returns><see cref="IAsyncFunctory"/></returns>
	IAsyncFunctory AsAsync();
}