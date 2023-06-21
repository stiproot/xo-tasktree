//namespace Xo.TaskTree.Core;

//public class BinaryBranchNode : BaseBranchNode, IBinaryBranchNode
//{
	//protected INode? _True;
	//protected INode? _False;
	//protected IBinaryBranchNodePathResolver? _PathResolver;

	///// <inheritdoc />
	//public virtual IBinaryBranchNode AddTrue(INode? node)
	//{
		//this._True = node;
		//return this;
	//}

	///// <inheritdoc />
	//public virtual IBinaryBranchNode AddFalse(INode? node)
	//{
		//this._False = node;
		//return this;
	//}

	///// <inheritdoc />
	//public virtual IBinaryBranchNode AddPathResolver(IBinaryBranchNodePathResolver? pathResolver)
	//{
		//this._PathResolver = pathResolver;
		//return this;
	//}

	///// <inheritdoc />
	//public override async Task<IMsg?> Run(CancellationToken cancellationToken)
	//{
		//this._Logger?.LogTrace($"BinaryBrarnchNode.Run - start.");

		//cancellationToken.ThrowIfCancellationRequested();

		//this.Validate();

		//await this.ResolvePromisedParams(cancellationToken);

		//this.AddContextParamResultsToParams();

		//try
		//{
			//return await this.ResolveFunctory(cancellationToken);
		//}
		//catch (Exception ex)
		//{
			//await HandleException(ex);
			//throw;
		//}
	//}

	///// <inheritdoc />
	//public override async Task<IMsg?> ResolveFunctory(CancellationToken cancellationToken)
	//{
		//var msg = await base.ResolveFunctory(cancellationToken);

		//bool bit = this._PathResolver is not null
			//? this._PathResolver.Resolve(msg)
			//: (msg as BaseMsg<bool>)!.GetData();

		//if (bit)
		//{
			//if (this._True is not null) return await this.RunNext(msg, this._True, cancellationToken);
			//return msg;
		//}

		//if (this._False is not null) return await this.RunNext(msg, this._False, cancellationToken);
		//return msg;
	//}

	///// <inheritdoc />
	//public override void Validate()
	//{
		//base.Validate();

		//if (this._True is null && this._False is null) throw new InvalidOperationException("True and False nodes cannot both be null...");
	//}

	///// <summary>
	/////   Initializes a new instance of <see cref="BinaryBranchNode"/>. 
	///// </summary>
	//public BinaryBranchNode(
		//IMsgFactory msgFactory,
		//ILogger? logger = null,
		//string? id = null,
		//IWorkflowContext? context = null
	//) : base(msgFactory, logger, id, context) { }
//}