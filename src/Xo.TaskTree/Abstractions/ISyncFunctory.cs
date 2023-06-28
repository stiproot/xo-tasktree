namespace Xo.TaskTree.Abstractions;

/// <summary>
///   A factory that produces a Func (functory) that is used by a <see cref="INode"/>.
/// </summary>
/// <remarks>
///   Strategies are how the TaskWorkflowEngine encapsulates operations that are to be run within a <see cref="INode"/>.
/// </remarks>
public interface ISyncFunctory
{
	/// <summary>
	///   Core func-creation method. Produces a factory that will return a syncrhonous operation. 
	/// </summary>
	/// <param name="param">IDictionary, 
	///   Keys: paramater names for some services' method that this functory makes use of. 
	///   Values: <see cref="IMsg"/> - Data property containing argument - Cast method can be used to typecast Data.</param>
	/// <param name="context">The workflow context.</param>
	/// <returns>Factory that will produce IMsg.</returns>
	Func<IMsg?> CreateFunc(
		IArgs args,
		IWorkflowContext? context = null
	);
}
