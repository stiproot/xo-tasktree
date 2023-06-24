namespace Xo.TaskTree.Abstractions;

public class MetaPathBranchBuilder : CoreNodeBuilder, IMetaPathBranchBuilder
{
	protected IMetaNode? _MetaNode;

	public IMetaBranchBuilder Init(IMetaNode metaNode)
	{
		this._MetaNode = metaNode ?? throw new ArgumentNullException(nameof(metaNode));
		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper) => this.Build(metaNodeMapper, this._MetaNode);

	protected INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		// IAsyncFunctory fn = this.TypeToFunctory(mn.FunctoryType);
		IAsyncFunctory fn = mn.FunctoryType.ToFunctory();

		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		// INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, context: this._Context)
		INode n = new Node()
			.SetFunctory(fn)
			.AddArg(promisedArgs)
			.AddArg(mn.NodeConfiguration!.Args.ToArray());

		if(mn.NodeEdge is not null) n.SetNodeEdge(new MonariusNodeEdge { Edge = this.Build(metaNodeMapper, mn.NodeEdge.Next)});
	
		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="MetaPathBranchBuilder"/>. 
	/// </summary>
	public MetaPathBranchBuilder(
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