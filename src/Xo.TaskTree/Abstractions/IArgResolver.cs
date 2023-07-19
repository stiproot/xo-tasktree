namespace Xo.TaskTree.Abstractions;

/// <summary>
///   A node evaluator.
/// </summary>
public interface IArgResolver
{
	/// <summary>
	///   Core run operation. 
	/// </summary>
	/// <param name="nodes"><see cref="INode"/>'s to be run.</param>
	/// <returns>Task node result set. A list of <see cref="IMsg"/>s.</returns>
	Task<IList<IMsg>> ResolveAsync(
		IList<INode> nodes,
		CancellationToken cancellationToken
	);
}
