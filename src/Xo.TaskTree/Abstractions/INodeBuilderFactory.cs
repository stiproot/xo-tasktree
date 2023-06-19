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
	///   Creates an implementation of <see cref="IPoolBranchBuilder"/>.
	/// </summary>
	/// <returns><see cref="IPoolBranchBuilder"/></returns>
	IPoolBranchBuilder Pool();

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IPoolBranchBuilder"/></returns>
	IPoolBranchBuilder Pool(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IPoolBranchBuilder"/></returns>
	IPoolBranchBuilder Pool(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IPoolBranchBuilder"/></returns>
	IPoolBranchBuilder Pool(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IPoolBranchBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IPoolBranchBuilder"/></returns>
	IPoolBranchBuilder Pool(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchBuilder"/>.
	/// </summary>
	/// <returns><see cref="ILinkedBranchBuilder"/></returns>
	ILinkedBranchBuilder Linked();

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="ILinkedBranchBuilder"/></returns>
	ILinkedBranchBuilder Linked(string id);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="ILinkedBranchBuilder"/></returns>
	ILinkedBranchBuilder Linked(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="ILinkedBranchBuilder"/></returns>
	ILinkedBranchBuilder Linked(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="ILinkedBranchBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="ILinkedBranchBuilder"/></returns>
	ILinkedBranchBuilder Linked(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchBuilder"/>.
	/// </summary>
	/// <returns><see cref="IBinaryBranchBuilder"/></returns>
	IBinaryBranchBuilder Binary();

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IBinaryBranchBuilder"/></returns>
	IBinaryBranchBuilder Binary(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IBinaryBranchBuilder"/></returns>
	IBinaryBranchBuilder Binary(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IBinaryBranchBuilder"/></returns>
	IBinaryBranchBuilder Binary(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IBinaryBranchBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IBinaryBranchBuilder"/></returns>
	IBinaryBranchBuilder Binary(IWorkflowContext context);

	///////////////////////////////////////////////////////////////

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchBuilder"/>.
	/// </summary>
	/// <returns><see cref="IHashBranchBuilder"/></returns>
	IHashBranchBuilder Hash();

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchBuilder"/>.
	/// </summary>
	/// <param name="id"></param>
	/// <returns><see cref="IHashBranchBuilder"/></returns>
	IHashBranchBuilder Hash(string id);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <param name="id"></param>
	/// <returns><see cref="IHashBranchBuilder"/></returns>
	IHashBranchBuilder Hash(
		ILogger logger,
		string id
	);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchBuilder"/>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/></param>
	/// <returns><see cref="IHashBranchBuilder"/></returns>
	IHashBranchBuilder Hash(ILogger logger);

	/// <summary>
	///   Creates an implementation of <see cref="IHashBranchBuilder"/>.
	/// </summary>
	/// <param name="context"><see cref="IWorkflowContext"/></param>
	/// <returns><see cref="IHashBranchBuilder"/></returns>
	IHashBranchBuilder Hash(IWorkflowContext context);
}
