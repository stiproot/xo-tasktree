namespace Xo.TaskTree.Abstractions;

public class MetaHashBranchBuilder : CoreNodeBuilder, IMetaHashBranchBuilder
{
	protected IMetaNode? _MetaNode;

	public IMetaBranchBuilder Init(IMetaNode metaNode)
	{
		this._MetaNode = metaNode ?? throw new ArgumentNullException(nameof(metaNode));
		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		// IAsyncFunctory fn = this.TypeToFunctory(this._MetaNode!.FunctoryType);
		IAsyncFunctory fn = this._MetaNode!.FunctoryType.ToFunctory();
		// INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, this.Id, this._Context);
		INode n = new Node(); 
		INode[] promisedArgs = this._MetaNode.PromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();

		INode[] decisions = this._MetaNode!.NodeEdge!.Nexts!.Select(v => this.BuildTrue(metaNodeMapper, v)).ToArray();

		INodeEdge e = new MultusNodeEdge { Edges = decisions };

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

		// IAsyncFunctory fn = this.TypeToFunctory(mn.FunctoryType);
		IAsyncFunctory fn = mn.FunctoryType.ToFunctory();
		// INode n = this._NodeFactory.Create(NodeBuilderTypes.Default, this._Logger, context: this._Context);
		INode n = new Node(); 
		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		// todo: this is ridiculous...
		Func<IDictionary<string, IMsg>, Func<IMsg>> decisionFn = 
			(p) => 
				() => StMsgFactory.Create<bool>(((p.First().Value) as Msg<string>)!.GetData().Equals(mn.NodeConfiguration!.Key));

		var decisionEdge = new MonariusNodeEdge().Add(n);
		var decisionNode  = new Node() 
			.SetFunctory(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge);
	
		return decisionNode;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public MetaHashBranchBuilder(
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