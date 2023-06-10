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

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNodeBuilder"/>.
	/// </summary>
	/// <returns><see cref="IPoolBranchNodeBuilder"/></returns>
	IPoolBranchNodeBuilder Pool();

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IPoolBranchNodeBuilder"/></returns>
	IPoolBranchNodeBuilder Pool(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IPoolBranchNodeBuilder"/></returns>
	IPoolBranchNodeBuilder Pool(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IPoolBranchNodeBuilder"/></returns>
	IPoolBranchNodeBuilder Pool(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IPoolBranchNodeBuilder"/></returns>
	IPoolBranchNodeBuilder Pool(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNodeBuilder"/>.
	/// </summary>
	/// <returns><see cref="ILinkedBranchNodeBuilder"/></returns>
	ILinkedBranchNodeBuilder Linked();

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="ILinkedBranchNodeBuilder"/></returns>
	ILinkedBranchNodeBuilder Linked(string id);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="ILinkedBranchNodeBuilder"/></returns>
	ILinkedBranchNodeBuilder Linked(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="ILinkedBranchNodeBuilder"/></returns>
	ILinkedBranchNodeBuilder Linked(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="ILinkedBranchNodeBuilder"/></returns>
	ILinkedBranchNodeBuilder Linked(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNodeBuilder"/>.
	/// </summary>
	/// <returns><see cref="IBinaryBranchNodeBuilder"/></returns>
	IBinaryBranchNodeBuilder Binary();

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IBinaryBranchNodeBuilder"/></returns>
	IBinaryBranchNodeBuilder Binary(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IBinaryBranchNodeBuilder"/></returns>
	IBinaryBranchNodeBuilder Binary(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IBinaryBranchNodeBuilder"/></returns>
	IBinaryBranchNodeBuilder Binary(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IBinaryBranchNodeBuilder"/></returns>
	IBinaryBranchNodeBuilder Binary(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNodeBuilder"/>.
	/// </summary>
	/// <returns><see cref="IHashBranchNodeBuilder"/></returns>
	IHashBranchNodeBuilder Hash();

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IHashBranchNodeBuilder"/></returns>
	IHashBranchNodeBuilder Hash(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IHashBranchNodeBuilder"/></returns>
	IHashBranchNodeBuilder Hash(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IHashBranchNodeBuilder"/></returns>
	IHashBranchNodeBuilder Hash(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchNodeBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IHashBranchNodeBuilder"/></returns>
	IHashBranchNodeBuilder Hash(IWorkflowContext context);
}
