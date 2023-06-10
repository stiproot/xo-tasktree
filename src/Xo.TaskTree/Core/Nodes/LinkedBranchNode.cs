namespace Xo.TaskTree.Abstractions;

public class LinkedBranchNode : BaseBranchNode, ILinkedBranchNode
{
	protected INode _Next;

	public virtual ILinkedBranchNode SetNext(INode node)
	{
		this._Next = node ?? throw new ArgumentNullException(nameof(node));
		return this;
	}

	public override async Task<IMsg?> ResolveFunctory(CancellationToken cancellationToken)
	{
		var msg = await base.ResolveFunctory(cancellationToken);
		return await this.RunNext(msg, this._Next, cancellationToken);
	}

	public override void Validate()
	{
		base.Validate();

		if (this._Next is null) throw new InvalidOperationException("Next cannot be null...");
	}

	/// <summary>
	///   Initializes a new instance of <see cref="LinkedBranchNode"/>. 
	/// </summary>
	public LinkedBranchNode(
		IMsgFactory msgFactory,
		ILogger? logger = null,
		string? id = null,
		IWorkflowContext? context = null
	) : base(msgFactory, logger, id, context) { }
}
