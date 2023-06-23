namespace Xo.TaskTree.Abstractions;

public class MetaBranchBranchBuilder : NodeBuilder, IMetaBranchBranchBuilder
{
	protected IMetaNode? _MetaNode;

	public IMetaBranchBranchBuilder Init(IMetaNode metaNode)
	{
		this._MetaNode = metaNode ?? throw new ArgumentNullException(nameof(metaNode));
		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		IAsyncFunctory fn = this.TypeToFunctory(this._MetaNode!.FunctoryType);
		INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, this.Id, this._Context);
		INode[] promisedArgs = this._MetaNode.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();

		INode[] ns = this._MetaNode!.NodeEdge!.Nexts!.Select(v => this.Build(metaNodeMapper, v)).ToArray();

		INodeEdge e = new MultusNodeEdge { Edges = ns };

		n
			.SetFunctory(fn)
			.AddArg(this._MetaNode.Args.ToArray())
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

		IAsyncFunctory fn = this.TypeToFunctory(mn.FunctoryType);

		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, context: this._Context)
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
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context)
	{
	}
}