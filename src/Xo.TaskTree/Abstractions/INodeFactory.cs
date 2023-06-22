namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Factory for producing instances of <see cref="INode"/> implementations.
/// </summary>
public interface INodeFactory
{
	/// <summary>
	///   Creates an instance of an <see cref="INode"/> implementation.
	/// </summary>
	/// <returns><see cref="INode"/></returns>
	INode Create();

	/// <summary>
	///   Creates an implementation of <see cref="INode"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="INode"/></returns>
	INode Create(string id);

	/// <summary>
	///   Creates an implementation of <see cref="INode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="INode"/></returns>
	INode Create(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="INode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="INode"/></returns>
	INode Create(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="INode"/>.
	/// </summary>
	/// <param name="context"><see cref="ILogger"/></param>
	/// <returns><see cref="INode"/></returns>
	INode Create(IWorkflowContext context);

	INode Create(
		BranchTypes nodeType,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	);
}
