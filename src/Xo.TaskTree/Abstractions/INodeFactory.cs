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

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNode"/>.
	/// </summary>
	/// <returns><see cref="IPoolBranchNode"/></returns>
	IPoolBranchNode Pool();

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNode"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IPoolBranchNode"/></returns>
	IPoolBranchNode Pool(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IPoolBranchNode"/></returns>
	IPoolBranchNode Pool(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IPoolBranchNode"/></returns>
	IPoolBranchNode Pool(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNode"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IPoolBranchNode"/></returns>
	IPoolBranchNode Pool(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNode"/>.
	/// </summary>
	/// <returns><see cref="ILinkedBranchNode"/></returns>
	ILinkedBranchNode Linked();

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNode"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="ILinkedBranchNode"/></returns>
	ILinkedBranchNode Linked(string id);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="ILinkedBranchNode"/></returns>
	ILinkedBranchNode Linked(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="ILinkedBranchNode"/></returns>
	ILinkedBranchNode Linked(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNode"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="ILinkedBranchNode"/></returns>
	ILinkedBranchNode Linked(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNode"/>.
	/// </summary>
	/// <returns><see cref="IBinaryBranchNode"/></returns>
	IBinaryBranchNode Binary();

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNode"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IBinaryBranchNode"/></returns>
	IBinaryBranchNode Binary(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IBinaryBranchNode"/></returns>
	IBinaryBranchNode Binary(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IBinaryBranchNode"/></returns>
	IBinaryBranchNode Binary(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNode"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IBinaryBranchNode"/></returns>
	IBinaryBranchNode Binary(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNode"/>.
	/// </summary>
	/// <returns><see cref="IHashBranchNode"/></returns>
	IHashBranchNode Hash();

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNode"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IHashBranchNode"/></returns>
	IHashBranchNode Hash(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IHashBranchNode"/></returns>
	IHashBranchNode Hash(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNode"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IHashBranchNode"/></returns>
	IHashBranchNode Hash(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNode"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IHashBranchNode"/></returns>
	IHashBranchNode Hash(IWorkflowContext context);

	INode Create(
		NodeTypes nodeType,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	);
}
