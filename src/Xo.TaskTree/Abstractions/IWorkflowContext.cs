namespace Xo.TaskTree.Abstractions;

/// <summary>
///   A workflowContext shared by each <see cref="INode"/> within a workflow.
/// </summary>
public interface IWorkflowContext
{
	/// <summary>
	///   Gets the data of a <see cref="Core.Msg{T}"/> contained within the workflowContext, with key provided.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key"></param>
	/// <returns><typeparam name="T"></typeparam></returns>
	T? GetMsgData<T>(string key);

	/// <summary>
	///   Returns the result of a <see cref="INode"/> run within the workflow that has been added to the workflowContext.
	/// </summary>
	/// <param name="key">The unique id of the <see cref="INode"/></param>
	/// <returns><see cref="object"/></returns>
	IMsg GetMsg(string key);

	/// <summary>
	///   Returns the results of multiple <see cref="INode"/>s that have run within the workflow that has been added to the workflowContext.
	/// </summary>
	/// <param name="keys">An array of unique <see cref="INode"/> ids.</param>
	/// <returns><see cref="IEnumerable{object}"/></returns>
	IEnumerable<IMsg> GetMsgs(params string[] keys);

	/// <summary>
	///   Returns the results of multiple <see cref="INode"/>s that have run within the workflow that has been added to the workflowContext, along with their keys.
	/// </summary>
	/// <param name="keys">An array of unique <see cref="INode"/> ids.</param>
	/// <returns><see cref="IEnumerable{(object, object)}"/></returns>
	IEnumerable<(string, IMsg)> GetKeyMsgPairs(params string[] keys);

	/// <summary>
	///   Returns all results of the <see cref="INode"/>s that have run within the workflow that have been added to the workflowContext, along with their keys.
	/// </summary>
	/// <returns><see cref="IEnumerable{(object, object)}"/></returns>
	IEnumerable<(string, IMsg)> GetAllKeyMsgPairs();

	/// <summary>
	///   Adds the result of a <see cref="INode"/> run within the workflow to the workflowContext.
	/// </summary>
	/// <remarks>This can only be used when all values associated with the provided keys are of the same type.</remarks>
	/// <param name="key">The unique id of the <see cref="INode"/></param>
	/// <param name="value">Result of the Task contained represented by <see cref="INode"/></param>
	void AddMsg(string key, IMsg msg);

	/// <summary>
	///   Constructs a <see cref="Msg{T}"/> around a type and adds it to the workflow-workflowContext.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="key"></param>
	/// <param name="data"></param>
	void AddData<T>(
		string key,
		T data
	);
}
