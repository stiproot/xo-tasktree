//namespace Xo.TaskTree.Abstractions;

//public class PoolBranchNode : BaseBranchNode, IPoolBranchNode
//{
	//protected readonly List<INode> _Pool = new();

	//public IPoolBranchNode AddNext(INode node)
	//{
		//this._Pool.Add(node ?? throw new ArgumentNullException(nameof(node)));
		//return this;
	//}

	//public IPoolBranchNode AddNext(params INode[] node)
	//{
		//this._Pool.AddRange(node ?? throw new ArgumentNullException(nameof(node)));
		//return this;
	//}

	//public override async Task<IMsg?> ResolveFunctory(CancellationToken cancellationToken)
	//{
		//var msg = await base.ResolveFunctory(cancellationToken);

		//foreach (var n in this._Pool) if (n.RequiresResult && msg is not null) this.PropogateMsg(msg, n);

		//var msgs = await Task.WhenAll(this._Pool.Select(n => n.Run(cancellationToken)));

		//return this._MsgFactory.Create<IMsg?[]>(msgs); // todo: double this logic...
	//}

	//public override void Validate()
	//{
		//base.Validate();

		//if (!this._Pool.Any()) throw new InvalidOperationException("Pool cannot be empty :P ...");
	//}

	///// <summary>
	/////   Initializes a new instance of <see cref="PoolBranchNode"/>. 
	///// </summary>
	//public PoolBranchNode(
		//IMsgFactory msgFactory,
		//ILogger? logger = null,
		//string? id = null,
		//IWorkflowContext? context = null
	//) : base(msgFactory, logger, id, context) { }
//}