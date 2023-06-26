namespace Xo.TaskTree.Abstractions;

public class MetaBinaryBranchBuilder : CoreNodeBuilder, IMetaBinaryBranchBuilder
{
	protected IMetaNode? _MetaNode;

	public IMetaBranchBuilder Init(IMetaNode metaNode)
	{
		this._MetaNode = metaNode ?? throw new ArgumentNullException(nameof(metaNode));
		return this;
	}

	public INode Build(IMetaNodeMapper metaNodeMapper)
	{
		IAsyncFunctory fn = this._MetaNode!.FunctoryType.ToFunctory(this._Functitect, "__");
		INode n = this._NodeFactory.Create(this._Logger, context: this._Context);
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

		// HERE!
		// todo: process node configuration then...
		// mn could point to more nodes...

		IAsyncFunctory fn = mn.FunctoryType.ToFunctory(this._Functitect, mn.NodeConfiguration?.NextParamName ?? "__");

		INode n = this._NodeFactory.Create(this._Logger, context: this._Context);
		if(mn.NodeConfiguration?.RequiresResult is true)
		{
			n.RequireResult();
		}

		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		// todo: this is ridiculous...
		Func<IDictionary<string, IMsg>, Func<IMsg>> decisionFn = (p) => () => SMsgFactory.Create<bool>(((p.First().Value) as Msg<bool>)!.GetData() is true, "__");
		var decisionEdge = new MonariusNodeEdge().Add(n);
		var decisionNode  = new Node() 
			.SetFunctory(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge)
			.RequireResult();
	
		return decisionNode;
	}

	// todo: this is duplication... not cool bro!
	protected INode BuildFalse(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		IAsyncFunctory fn = mn.FunctoryType.ToFunctory(this._Functitect, mn.NodeConfiguration?.NextParamName ?? "__");

		INode n = this._NodeFactory.Create(this._Logger, context: this._Context);

		if(mn.NodeConfiguration?.RequiresResult is true)
		{
			n.RequireResult();
		}

		INode[] promisedArgs = mn.PromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();

		// todo: this is ridiculous...
		Func<IDictionary<string, IMsg>, Func<IMsg>> decisionFn = (p) => () => SMsgFactory.Create<bool>(((p.First().Value) as Msg<bool>)!.GetData() is false, "__");
		var decisionEdge = new MonariusNodeEdge().Add(n);
		var decisionNode = new Node()
			.SetFunctory(decisionFn)
			.SetController(new TrueController())
			.SetNodeEdge(decisionEdge)
			.RequireResult();
	
		return decisionNode;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="BinaryBranchBuilder"/>. 
	/// </summary>
	public MetaBinaryBranchBuilder(
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