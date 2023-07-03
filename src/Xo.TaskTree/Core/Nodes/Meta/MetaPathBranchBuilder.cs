namespace Xo.TaskTree.Abstractions;

public class MetaPathBranchBuilder : CoreNodeBuilder, IMetaPathBranchBuilder
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

		this._MetaNode!.NodeEdge.ThrowIfNull();

		this._MetaNode!.NodeEdge!.Next.ThrowIfNull();

		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		this.Validate();

		return this.Build(metaNodeMapper, this._MetaNode!);
	}

	protected INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode mn
	) 
	{
		// IAsyncFn fn = this.TypeToFn(mn.FnType);
		IAsyncFn fn = mn.FnType.ToFn(this._FnFactory);

		INode[] promisedArgs = mn.NodeConfiguration.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		INode n = this._NodeFactory.Create(this._Logger, context: this._Context)
			.SetFn(fn)
			.AddArg(promisedArgs)
			.AddArg(mn.NodeConfiguration!.Args.ToArray());

		if(mn.NodeEdge is not null) n.SetNodeEdge(new MonariusNodeEdge { Edge = this.Build(metaNodeMapper, mn.NodeEdge.Next!)});
	
		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="MetaPathBranchBuilder"/>. 
	/// </summary>
	public MetaPathBranchBuilder(
		IFnFactory functitect,
		INodeFactory nodeFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(
			functitect, 
			nodeFactory,
			logger, 
			id, 
			context
	)
	{
	}
}