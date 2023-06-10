namespace Xo.TaskTree.Abstractions;

public class LinkedBranchNodeBuilder : BaseNodeBuilder, ILinkedBranchNodeBuilder
{
	protected INode _Next;

	public virtual ILinkedBranchNodeBuilder SetNext(INode node)
	{
		this._Next = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	}

	public virtual ILinkedBranchNodeBuilder SetNext<T>(bool requiresResult = true)
	{
		this._Next = this.Build(typeof(T));
		if (requiresResult) this._Next.RequireResult();
		return this;
	}

	public override INode Build()
	{
		var n = this.BuildBase() as ILinkedBranchNode;

		n!
			.SetNext(this._Next);

		return n;
	}

	/// <summary>
	///   Initializes a new instance of <see cref="LinkedBranchNodeBuilder"/>. 
	/// </summary>
	public LinkedBranchNodeBuilder(
		IFunctitect functitect,
		INodeFactory nodeFactory,
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(functitect, nodeFactory, msgFactory, logger, id, context) => this._NodeType = NodeTypes.Linked;
}
