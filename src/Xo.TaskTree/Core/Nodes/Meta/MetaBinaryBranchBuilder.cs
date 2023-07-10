namespace Xo.TaskTree.Abstractions;

public class MetaBinaryBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
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
		
		if(this._MetaNode!.NodeType is not MetaNodeTypes.Binary) throw new InvalidOperationException("Invalid meta node type.");

		this._MetaNode.NodeEdge.ThrowIfNull();

		if(this._MetaNode.NodeEdge!.True is null && this._MetaNode.NodeEdge!.False is null) throw new InvalidOperationException();

		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		this.Validate();

		INode? @true = this.BuildBinary(metaNodeMapper, this._MetaNode!.NodeEdge!.True, true);
		INode? @false = this.BuildBinary(metaNodeMapper, this._MetaNode!.NodeEdge!.False, false);
		INodeEdge e = new BinariusNodeEdge { Edge1 = @true, Edge2 = @false };

		IAsyncFn fn = this._MetaNode!.ServiceType.ToFn(this._FnFactory, this._MetaNode.NodeConfiguration?.NextParamName);

		INode[] promisedArgs = _MetaNode.NodeConfiguration!.MetaPromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		this._MetaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create()
			.Configure(this._MetaNode.NodeConfiguration)
			.AddFn(fn)
			.AddNodeEdge(e)
			.Build();

		return n;
	}

	protected INode? BuildBinary(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn,
		bool binaryBranchType
	) 
	{
		if(mn is null) return null;

		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory, mn.NodeConfiguration.NextParamName);

		IList<INode> promisedArgs = mn.NodeConfiguration!.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToList();
		mn.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create()
			.Configure(mn.NodeConfiguration)
			.AddFn(fn)
			.Build();
		
		if(mn.NodeEdge is not null)
		{
			INode thenNode = metaNodeMapper.Map(mn.NodeEdge.Next!);

			INodeEdge thenEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(thenNode);

			n.SetNodeEdge(thenEdge);
		}

		Func<IArgs, IMsg?> decisionFn = DecisionFactory(mn.NodeConfiguration.ControllerType, binaryBranchType);

		var decisionEdge = NodeEdgeFactory.Create(NodeEdgeTypes.Monarius).Add(n);

		var decisionNode  = this._NodeBuilderFactory
			.Create()
			.Configure(c => c.RequireResult())
			.AddFn(decisionFn)
			.AddController(new TrueController())
			.AddNodeEdge(decisionEdge)
			.Build();
	
		return decisionNode;
	}

	private static Func<IArgs, IMsg?> DecisionFactory(
		ControllerTypes? controllerType,
		bool conditionType
	)
	{
		return controllerType switch
		{
			ControllerTypes.True => p => SMsgFactory.Create<bool>(p.First()!.Data<bool>() == conditionType),
			ControllerTypes.IsNotNull => p => SMsgFactory.Create<bool>(p.First()!.HasData == conditionType),
			_ => throw new NotSupportedException()
		};
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
