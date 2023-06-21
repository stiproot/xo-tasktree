namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Factory for producing instances of <see cref="INodeBuilder"/> implementations.
/// </summary>
public interface INodeBuilderFactory
{
	/// <summary>
	///   Creates an instance of an <see cref="INodeBuilder"/> implementation.
	/// </summary>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder Create();

	/// <summary>
	///   Creates an implementation of <see cref="INodeBuilder"/>.
	/// </summary>
	/// <param name="nodeType"></param>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder Create(NodeTypes nodeType);

	/// <summary>
	///   Creates an implementation of <see cref="INodeBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder Create(string id);

	/// <summary>
	///   Creates an implementation of <see cref="INodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder Create(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="INodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder Create(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="INodeBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="ILogger"/></param>
	/// <returns><see cref="INodeBuilder"/></returns>
	INodeBuilder Create(IWorkflowContext context);
}
