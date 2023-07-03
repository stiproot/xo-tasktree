namespace Xo.TaskTree.Abstractions;

public class MetaBranchBranchBuilder : CoreNodeBuilder, IMetaBranchBranchBuilder
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
		IAsyncFunctoryInvoker fn = this._MetaNode!.FunctoryType.ToFunctory(this._Functitect);
		INode n = this._NodeFactory.Create(this._Logger, context: this._Context);
		INode[] promisedArgs = this._MetaNode.NodeConfiguration.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();

		INode[] ns = this._MetaNode!.NodeEdge!.Nexts!.Select(v => this.Build(metaNodeMapper, v)).ToArray();

		INodeEdge e = new MultusNodeEdge { Edges = ns };

		n
			.SetFunctory(fn)
			.AddArg(this._MetaNode.NodeConfiguration.Args.ToArray())
			.AddArg(promisedArgs)
			.SetNodeEdge(e);

		return n;
	}

	protected INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		IAsyncFunctoryInvoker fn = mn.FunctoryType.ToFunctory(this._Functitect);

		INode[] promisedArgs = mn.NodeConfiguration.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		INode n = this._NodeFactory.Create(this._Logger, context: this._Context)
			.SetFunctory(fn)
			.AddArg(promisedArgs)
			.AddArg(mn.NodeConfiguration!.Args.ToArray());
	
		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public MetaBranchBranchBuilder(
		IFunctitect functitect,
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