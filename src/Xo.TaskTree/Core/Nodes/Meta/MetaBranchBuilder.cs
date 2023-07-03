namespace Xo.TaskTree.Abstractions;

public class MetaBranchBuilder : CoreNodeBuilder, IMetaBranchBuilder
{
	protected IMetaNode? _MetaNode;

	public IMetaBranchBuilder Init(IMetaNode metaNode)
	{
		this._MetaNode = metaNode ?? throw new ArgumentNullException(nameof(metaNode));
		return this;
	}

	public virtual IMetaBranchBuilder Validate()
	{
		this._MetaNode.ThrowIfNull();

		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		IAsyncFn fn = this._MetaNode!.FnType.ToFn(this._FnFactory, this._MetaNode!.NodeConfiguration?.NextParamName);

		INode[] promisedArgs = this._MetaNode.NodeConfiguration!.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();

		INode n = this._NodeFactory
			.Create(this._Logger, this.Id, this._Context)
			.SetFn(fn)
			.AddArg(this._MetaNode.NodeConfiguration.Args.ToArray())
			.AddArg(promisedArgs);
		
		if(this._MetaNode.NodeConfiguration?.RequiresResult is true) n.RequireResult();

		if(this._MetaNode.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(this._MetaNode.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public MetaBranchBuilder(
		IFnFactory fnFactory,
		INodeFactory nodeFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(
			fnFactory, 
			nodeFactory,
			logger, 
			id, 
			context
	)
	{
	}
}
