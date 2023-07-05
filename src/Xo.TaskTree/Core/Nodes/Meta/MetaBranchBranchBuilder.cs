namespace Xo.TaskTree.Abstractions;

public class MetaBranchBranchBuilder : CoreBranchBuilder, IMetaBranchBuilder
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
		IAsyncFn fn = this._MetaNode!.ServiceType.ToFn(this._FnFactory);
		INode[] promisedArgs = this._MetaNode.NodeConfiguration.MetaPromisedArgs.Select(p =>  metaNodeMapper.Map(p)).ToArray();
		this._MetaNode.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode[] ns = this._MetaNode!.NodeEdge!.Nexts!.Select(v => this.Build(metaNodeMapper, v)).ToArray();

		INodeEdge e = new MultusNodeEdge { Edges = ns };

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.AddFn(fn)
			.AddNodeEdge(e)
			.Build();

		return n;
	}

	protected INode Build(
		IMetaNodeMapper metaNodeMapper,
		IMetaNode? mn
	) 
	{
		if(mn is null) throw new InvalidOperationException();

		IAsyncFn fn = mn.ServiceType.ToFn(this._FnFactory);

		INode[] promisedArgs = mn.NodeConfiguration.MetaPromisedArgs.Select(p => metaNodeMapper.Map(p)).ToArray();
		mn.NodeConfiguration.PromisedArgs.AddRange(promisedArgs);

		INode n = this._NodeBuilderFactory
			.Create(this._Logger)
			.AddFn(fn)
			.Build();
	
		return n;
	}

	public MetaBranchBranchBuilder(
		INodeBuilderFactory nodeBuilderFactory,
		IFnFactory fnFactory,
		ILogger? logger = null,
		IWorkflowContext? context = null
	) : base(
			nodeBuilderFactory,
			fnFactory, 
			logger, 
			context
	)
	{
	}
}