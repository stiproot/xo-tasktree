namespace Xo.TaskTree.Abstractions;

public class MetaBinaryBranchBuilder : NodeBuilder, IMetaBinaryBranchBuilder
{
	protected IMetaNode? _MetaNode;

	public IMetaBinaryBranchBuilder Init(IMetaNode metaNode)
	{
		this._MetaNode = metaNode ?? throw new ArgumentNullException(nameof(metaNode));
		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		IAsyncFunctory fn = this.TypeToFunctory(this._MetaNode!.FunctoryType);
		INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, this.Id, this._Context);
		INode[] promisedArgs = this._MetaNode.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();

		INode @true = this.BuildTrue(metaNodeMapper, this._MetaNode.NodeEdge!.True);
		INode @false = this.BuildFalse(metaNodeMapper, this._MetaNode.NodeEdge!.False);
		INodeEdge e = new BinariusNodeEdge { Edge1 = @true, Edge2 = @false };

		n
			.SetFunctory(fn)
			.AddArg(this._MetaNode.Args.ToArray())
			.AddArg(promisedArgs)
			.SetNodeEdge(e);

		return n;
	}

	protected INode BuildTrue(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		IAsyncFunctory fn = this.TypeToFunctory(mn.FunctoryType);
		INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, context: this._Context);
		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		// todo: this is ridiculous...
		Func<IDictionary<string, IMsg>, Func<IMsg>> decisionFn = (p) => () => this._MsgFactory.Create<bool>(((p.First().Value) as Msg<bool>)!.GetData());
		var decisionEdge = new MonariusNodeEdge().Add(n);
		var decisionNode  = this._NodeFactory
			.Create()
			.SetFunctory(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge);
	
		return decisionNode;
	}

	// todo: this is duplication... not cool bro!
	protected INode BuildFalse(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		IAsyncFunctory fn = this.TypeToFunctory(mn.FunctoryType);
		INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, context: this._Context);
		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		// todo: this is ridiculous...
		Func<IDictionary<string, IMsg>, Func<IMsg>> decisionFn = (p) => () => this._MsgFactory.Create<bool>(((p.First().Value) as Msg<bool>)!.GetData() is false);
		var decisionEdge = new MonariusNodeEdge().Add(n);
		var decisionNode  = this._NodeFactory
			.Create()
			.SetFunctory(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge);
	
		return decisionNode;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public MetaBinaryBranchBuilder(
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