namespace Xo.TaskTree.Abstractions;

public class PoolBranchBuilder : BaseNodeBuilder, IPoolBranchBuilder
{
	protected readonly List<INode> _Pool = new();

	public IPoolBranchBuilder AddNext(INode node)
	{
		this._Pool.Add(node ?? throw new ArgumentNullException(nameof(node)));

		return this;
	}

	public IPoolBranchBuilder AddNext(params INode[] node)
	{
		this._Pool.AddRange(node ?? throw new ArgumentNullException(nameof(node)));

		return this;
	}

	public IPoolBranchBuilder AddNext<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		var n = this.Build(typeof(T));

		// if (requiresResult) n.RequireResult();

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
	///   Initializes a new instance of <see cref="PoolBranchBuilder"/>. 
	/// </summary>
	public PoolBranchBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context) => this._NodeType = NodeTypes.Pool;
}