namespace Xo.TaskTree.Abstractions;

public class MetaPoolBranchBuilder : BaseNodeBuilder, IMetaPoolBranchBuilder
{
	public IMetaPoolBranchBuilder Init(IMetaNode metaNode)
	{
		throw new NotImplementedException();
	}

	public override INode Build()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	///   Initializes a new instance of <see cref="PoolBranchBuilder"/>. 
	/// </summary>
	public MetaPoolBranchBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context) => this._NodeType = NodeTypes.Pool;
}