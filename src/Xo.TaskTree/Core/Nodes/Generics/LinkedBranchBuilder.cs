namespace Xo.TaskTree.Abstractions;

public class LinkedBranchBuilder : CoreNodeBuilder, ILinkedBranchBuilder
{
	protected INode? _Next;

	public virtual ILinkedBranchBuilder SetNext(INode node)
	{
		this._Next = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	}

	public virtual ILinkedBranchBuilder SetNext<T>(bool requiresResult = true)
	{
		this._Next = typeof(T).ToNode(this._Functitect);

		if (requiresResult) this._Next.RequireResult();

		return this;
	}

	public override INode Build()
	{
		// var n = this.BuildBase() as ILinkedBranchNode;
		// n!
			// .SetNext(this._Next);
		// return n;

		throw new NotImplementedException();
	}

	/// <summary>
	///   Initializes a new instance of <see cref="LinkedBranchBuilder"/>. 
	/// </summary>
	public LinkedBranchBuilder(
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
