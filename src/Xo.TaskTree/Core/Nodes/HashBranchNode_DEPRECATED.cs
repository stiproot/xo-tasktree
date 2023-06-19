namespace Xo.TaskTree.Abstractions;

public class HashBranchNode : BaseBranchNode, IHashBranchNode
{
	protected readonly IDictionary<string, INode> _Hash = new Dictionary<string, INode>();

	public virtual IHashBranchNode AddNext(string key, INode node)
	{
		this._Hash.Add(key, node ?? throw new ArgumentNullException(nameof(node)));
		return this;
	}

	public virtual IHashBranchNode SetHash(IDictionary<string, INode> hash)
	{
		this._Hash.Clear();
		foreach (var kvp in hash) this._Hash.Add(kvp);
		return this;
	}

	public override async Task<IMsg?> ResolveFunctory(CancellationToken cancellationToken)
	{
		var msg = await base.ResolveFunctory(cancellationToken);
		string key = (msg as BaseMsg<string>)!.GetData();
		var node = this._Hash[key];

		return await this.RunNext(msg, node, cancellationToken);
	}

	public override void Validate()
	{
		base.Validate();

		if (!this._Hash.Any()) throw new InvalidOperationException("Hash cannot be empty...");
	}

	/// <summary>
	///   Initializes a new instance of <see cref="HashBranchNode"/>. 
	/// </summary>
	public HashBranchNode(
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(msgFactory, logger, id, context) { }
}
