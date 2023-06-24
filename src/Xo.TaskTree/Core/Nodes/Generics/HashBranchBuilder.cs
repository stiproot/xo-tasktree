namespace Xo.TaskTree.Abstractions;

public class HashBranchBuilder : CoreNodeBuilder, IHashBranchBuilder
{
	protected readonly IDictionary<string, INode> _Hash = new Dictionary<string, INode>();

	public virtual IHashBranchBuilder AddNext(string key, INode node)
	{
		this._Hash.Add(key, node ?? throw new ArgumentNullException(nameof(node)));
		return this;
	}

	public virtual IHashBranchBuilder AddNext<T>(string key)
	{
		var n = typeof(T).ToNode();

		this._Hash.Add(key, n);

		return this;
	}

	public override INode Build()
	{
		//var n = this.BuildBase() as IHashBranchNode;
		//n!
			//.SetHash(this._Hash);
		//return n;

		throw new NotImplementedException();
	}

	/// <summary>
	///   Initializes a new instance of <see cref="HashBranchBuilder"/>. 
	/// </summary>
	public HashBranchBuilder(
		// IFunctitect functitect,
		// INodeFactory nodeFactory,
		// IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	// ) : base(functitect, nodeFactory, msgFactory, logger, id, context)
	) : base(logger, id, context)
	{
	}
}
