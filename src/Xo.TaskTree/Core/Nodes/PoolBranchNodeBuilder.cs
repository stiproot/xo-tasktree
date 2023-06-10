namespace Xo.TaskTree.Abstractions;

public class PoolBranchNodeBuilder : BaseNodeBuilder, IPoolBranchNodeBuilder
{
	protected readonly List<INode> _Pool = new();

	public IPoolBranchNodeBuilder AddNext(INode node)
	{
		this._Pool.Add(node ?? throw new ArgumentNullException(nameof(node)));
		return this;
	}

	public IPoolBranchNodeBuilder AddNext(params INode[] node)
	{
		this._Pool.AddRange(node ?? throw new ArgumentNullException(nameof(node)));
		return this;
	}

	public IPoolBranchNodeBuilder AddNext<T>(bool requiresResult = true)
	{
		var n = this.Build(typeof(T));

		if (requiresResult) n.RequireResult();

		this._Pool.Add(n);

		return this;
	}

	public override INode Build()
	{
		var n = this.BuildBase() as IPoolBranchNode;

		n!
			.AddNext(this._Pool.ToArray());

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="PoolBranchNodeBuilder"/>. 
	/// </summary>
	public PoolBranchNodeBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context) => this._NodeType = NodeTypes.Pool;
}