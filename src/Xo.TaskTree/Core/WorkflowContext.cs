using System.Collections.Concurrent;

namespace Xo.TaskTree.Core;

/// <inheritdoc cref="IWorkflowContext"/>
public class WorkflowContext : IWorkflowContext
{
	private readonly ConcurrentDictionary<string, IMsg> _dictionary;

	/// <summary>
	///   Initializes a new instance of <see cref="WorkflowContext"/>.
	/// </summary>
	public WorkflowContext() => this._dictionary = new ConcurrentDictionary<string, IMsg>();

	/// <inheritdoc />
	public T? GetMsgData<T>(string key)
	{
		var msg = this.GetMsg(key);

		return ((Msg<T>)msg).GetData();
	}

	/// <inheritdoc />
	public IMsg GetMsg(string key)
	{
		if (this._dictionary.TryGetValue(key, out var msg)) return msg;

		throw new InvalidOperationException($"WorkflowContext.GetResult - No msg found. key: {key}");
	}

	/// <inheritdoc />
	public IEnumerable<IMsg> GetMsgs(params string[] keys)
	{
		if (keys is null) throw new ArgumentNullException(nameof(keys));

		if (!keys.Any()) throw new ArgumentException("WorkflowContext.GetResults - No keys provided.");

		return keys.Select(k => this.GetMsg(k));
	}

	/// <inheritdoc />
	public IEnumerable<(string, IMsg)> GetKeyMsgPairs(params string[] keys)
	{
		if (keys is null) throw new ArgumentNullException(nameof(keys));

		if (!keys.Any()) throw new ArgumentException("WorkflowContext.GetResults - No keys provided.");

		return keys.Select(k => (k, this.GetMsg(k)));
	}

	/// <inheritdoc />
	public IEnumerable<(string, IMsg)> GetAllKeyMsgPairs()
		=> this._dictionary.Select(kvp => (kvp.Key, kvp.Value));

	/// <inheritdoc />
	public void AddMsg(string key, IMsg msg) => this._dictionary.TryAdd(key, msg);

	/// <inheritdoc />
	public void AddData<T>(
		string key,
		T data
	)
	{
		var msg = new Msg<T>(data);
		this.AddMsg(key, msg);
	}
}
