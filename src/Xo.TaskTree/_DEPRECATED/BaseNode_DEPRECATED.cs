//namespace Xo.TaskTree.Abstractions;

///// <inheritdoc cref="INode"/>
//public abstract class BaseNode : INode
//{
	//protected ILogger? _Logger;
	//protected IAsyncFn? _AsyncFn;
	//protected ISyncFn? _SyncFn;
	//protected IWorkflowContext? _Context;
	//protected Func<Exception, Task>? _ExceptionHandlerAsync;
	//protected Action<Exception>? _ExceptionHandler;
	//protected INodeEdge? _NodeEdge;
	//protected IController? _Controller;
	//protected IInvoker _Invoker = new Invoker(new NodeEdgeResolver());
	//protected INodevaluator _Nodevaluator = new ParallelNodeEvaluator();
	//protected readonly IList<IMsg> _Params = new List<IMsg>();
	//protected readonly List<INode> _PromisedParams = new List<INode>();
	//protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	///// <inheritdoc />
	//public string Id { get; internal set; } = $"{Guid.NewGuid()}";

	///// <inheritdoc />
	//public INodeEdge? NodeEdge => this._NodeEdge;

	///// <inheritdoc />
	//public bool HasParam(string paramName) => this._Params.Any(p => p.ParamName == paramName);

	///// <inheritdoc />
	//public int ArgCount() => this._Params.Count() + this._PromisedParams.Count() + this._ContextParams.Count();

	///// <inheritdoc />
	//public bool RequiresResult { get; internal set; } = false;

	//public bool IgnoresPromisedResults { get; internal set; } = false;

	///// <inheritdoc />
	//public IFn Fn => this._AsyncFn is not null ? (IFn)this._AsyncFn! : (IFn)this._SyncFn!;

	///// <inheritdoc />
	//public bool IsSync => this._SyncFn != null;

	///// <inheritdoc />
	//public INode SetNodeEdge(INodeEdge nodeEdge)
	//{
		//this._NodeEdge = nodeEdge ?? throw new ArgumentNullException(nameof(nodeEdge));
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetNodevaluator(INodevaluator nodevaluator)
	//{
		//this._Nodevaluator = nodevaluator ?? throw new ArgumentNullException(nameof(nodevaluator));
		//return this;
	//}

	///// <inheritdoc />
	//public INode RunNodesInLoop()
	//{
		//this.SetNodevaluator(new LoopNodeEvaluator());
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetFn(IAsyncFn fn)
	//{
		//this._AsyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetFn(Func<IArgs, Task<IMsg?>> fn)
	//{
		//this._AsyncFn = new AsyncFnAdaptor(fn);
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetFn(ISyncFn fn)
	//{
		//this._SyncFn = fn ?? throw new ArgumentNullException(nameof(fn));
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetFn(Func<IArgs, IMsg?> fn)
	//{
		//this._SyncFn = new SyncFnAdapter(fn);
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetFn(Func<IWorkflowContext, IMsg?> fn)
	//{
		//this._SyncFn = new SyncFnAdapter(fn);
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetContext(IWorkflowContext? context)
	//{
		//this._Context = context;
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetId(string id)
	//{
		//this.Id = id;
		//return this;
	//}

	///// <inheritdoc />
	//public INode SetLogger(ILogger logger)
	//{
		//this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		//return this;
	//}

	///// <inheritdoc />
	//public INode AddArg(params INode[] nodes)
	//{
		//foreach (var h in nodes)
		//{
			//this._PromisedParams.Add(h);
		//}
		//return this;
	//}

	///// <inheritdoc />
	//public INode AddArg(params IMsg?[] msgs)
	//{
		//foreach (var m in msgs)
		//{
			//if(m is null)
			//{
				//continue;
			//}
			//this._Params.Add(m);
		//}
		//return this;
	//}

	///// <inheritdoc />
	//public INode AddArg<T>(
			//T data,
			//string paramName
	//)
	//{
		//if (data is null || paramName is null) throw new InvalidOperationException("Null values cannot be passed into AddArg<T>...");

		//// this._Params.Add(this._MsgFactory.Create<T>(data, paramName));
		//this._Params.Add(SMsgFactory.Create<T>(data, paramName));

		//return this;
	//}

	///// <inheritdoc />
	//public INode AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs)
	//{
		//foreach (var p in contextArgs)
		//{
			//this._ContextParams.Add(p);
		//}

		//return this;
	//}

	///// <inheritdoc />
	//public INode SetExceptionHandler(Func<Exception, Task> handler)
	//{
		//this._ExceptionHandlerAsync = handler;

		//return this;
	//}

	///// <inheritdoc />
	//public INode SetExceptionHandler(Action<Exception> handler)
	//{
		//this._ExceptionHandler = handler;

		//return this;
	//}

	///// <inheritdoc />
	//public virtual async Task<IMsg?[]> Run(CancellationToken cancellationToken)
	//{
		//this._Logger?.LogTrace($"Node.Run - start.");

		//cancellationToken.ThrowIfCancellationRequested();

		//this.Validate();

		//await this.ResolvePromisedParams(cancellationToken);

		//this.AddContextParamResultsToParams();

		//try
		//{
			//return await this.ResolveFn(cancellationToken);
		//}
		//catch (Exception ex)
		//{
			//await HandleException(ex);
			//throw;
		//}
	//}

	///// <inheritdoc />
	//public virtual void Validate()
	//{
		//this._Logger?.LogTrace($"Node.Validate - start.");

		//if (this._AsyncFn is null && this._SyncFn is null)
		//{
			//this._Logger?.LogError($"Node validation failed.");
			//throw new InvalidOperationException("Strategy factory has not been set.");
		//}

		//this._Logger?.LogError($"Node.Validate - end.");
	//}

	///// <inheritdoc />
	//public virtual async Task ResolvePromisedParams(CancellationToken cancellationToken)
	//{
		//this._Logger?.LogTrace($"Node.ResolvePromisedParams - running param nodes.");

		//if (!this._PromisedParams.Any()) return;

		//var results = await this._Nodevaluator.RunAsync(this._PromisedParams, cancellationToken);

		//IEnumerable<IMsg> nonNullResults = results.Where(p => p is not null).ToList()!;

		//if(this.IgnoresPromisedResults) return;

		//foreach (var r in nonNullResults)
		//{
			//this._Params.Add(r);
		//}
	//}

	///// <inheritdoc />
	//public virtual void AddContextParamResultsToParams()
	//{
		//this._Logger?.LogTrace($"Node.AddContextParamResultsToParams - start.");

		//// Are there any params that need to be extracted from the shared context?
		//if (!this._ContextParams.Any())
		//{
			//return;
		//}

		//if (this._Context == null)
		//{
			//throw new InvalidOperationException("Context has not been provided");
		//}

		//foreach (var f in this._ContextParams)
		//{
			//this._Params.Add(f(this._Context));
		//}

		//this._Logger?.LogTrace($"Node.AddContextParamResultsToParams - end.");
	//}

	///// <inheritdoc />
	//public virtual async Task<IMsg?[]> ResolveFn(CancellationToken cancellationToken)
	//{
		//this._Logger?.LogTrace($"BaseNode.ResolveFn - starting...");

		//var result = this.IsSync
				//? this._SyncFn!.Invoke(this._Params.AsArgs(), this._Context)
				//: await this._AsyncFn!.Invoke(this._Params.AsArgs(), this._Context);

		//if (result is not null && this._Context is not null)
		//{
			//this._Context.AddMsg(this.Id, result);
		//}

		//if(this._Controller is not null)
		//{
			//var bit = this._Controller.Control(result);

			//if(bit is false) return result.ToArray();
		//}

		//if(this._NodeEdge is not null)
		//{
			//return await this._Invoker.Invoke(this._NodeEdge, result.ToArray(), cancellationToken);
		//}

		//return result.ToArray();
	//}

	///// <inheritdoc />
	//public virtual async Task HandleException(Exception ex)
	//{
		//if (this._ExceptionHandlerAsync != null)
		//{
			//await this._ExceptionHandlerAsync(ex);
		//}

		//if (this._ExceptionHandler != null)
		//{
			//this._ExceptionHandler(ex);
		//}
	//}

	//public INode SetInvoker(IInvoker invoker)
	//{
		//this._Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
		//return this;
	//}

	//public INode SetController(IController controller)
	//{
		//this._Controller = controller ?? throw new ArgumentNullException(nameof(controller));

		//return this;
	//}

	///// <inheritdoc />
	//public virtual INode RequireResult(bool requiresResult = true)
	//{
		//this.RequiresResult = requiresResult;
		//return this;
	//}

	//public INode IgnorePromisedResults(bool ignorePromisedResults = true)
	//{
		//this.IgnoresPromisedResults = ignorePromisedResults;
		//return this;
	//}

	///// <summary>
	/////   Initializes a new instance of <see cref="Node"/>. 
	///// </summary>
	//public BaseNode(
		//ILogger? logger = null,
		//string? id = null,
		//IWorkflowContext? context = null
	//)
	//{
		//this._Logger = logger;
		//if (id is not null) this.Id = id;
		//this._Context = context;
	//}
//}