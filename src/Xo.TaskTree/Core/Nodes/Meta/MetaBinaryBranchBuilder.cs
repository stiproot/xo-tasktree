namespace Xo.TaskTree.Abstractions;

public class MetaBinaryBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
{
	public virtual IMetaBranchBuilder Validate(IMetaNode metaNode)
	{
		metaNode.ThrowIfNull();

		if (metaNode!.NodeType is not MetaNodeTypes.Binary) throw new InvalidOperationException("Invalid meta node type.");

		metaNode.NodeEdge.ThrowIfNull();

		if (metaNode.NodeEdge!.True is null && metaNode.NodeEdge!.False is null) throw new InvalidOperationException();

		return this;
	}

	public INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode metaNode
	)
	{
		this.Validate(metaNode);

		INode? @true = this.BuildBinary(metaNodeMapper, metaNode!.NodeEdge!.True, true);
		INode? @false = this.BuildBinary(metaNodeMapper, metaNode!.NodeEdge!.False, false);
		INodeEdge e = NodeEdgeFactory.Create(NodeEdgeTypes.Binarius).Add(@true, @false);

		IAsyncFn fn = metaNode!.ServiceType.ToFn(this._FnFactory, metaNode.NodeConfiguration.NextParamName);

		INode[] promisedArgs = metaNode.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		metaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create()
			.Configure(metaNode.NodeConfiguration)
			.AddFn(fn)
			.AddNodeEdge(e)
			.Build();

		return n;
	}

	protected INode? BuildBinary(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn,
		bool resolveTo
	)
	{
		if (mn is null) return null;

		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory, mn.NodeConfiguration.NextParamName);

		IList<INode> promisedArgs = mn.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToList();
		mn.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create()
			.Configure(mn.NodeConfiguration)
			.AddFn(fn)
			.Build();

		if (mn.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		Func<IArgs, IMsg?> decisionFn = DecisionFnFactory.Create(mn.NodeConfiguration.ControllerType, resolveTo);

		var decisionEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(n);

		var decisionNode = this._NodeBuilderFactory
			.Create()
			.Configure(c => c.RequireResult())
			.AddFn(decisionFn)
			.AddController(new TrueController())
			.AddNodeEdge(decisionEdge)
			.Build();

		return decisionNode;
	}

	public MetaBinaryBranchBuilder(
		INodeBuilderFactory nodeBuilderFactory,
		IFnFactory fnFactory,
		ILogger? logger = null,
		IWorkflowContext? workflowContext = null
	) : base(
			nodeBuilderFactory,
			fnFactory,
			logger,
			workflowContext
	)
	{
	}
}
