//using System.Reflection;

//namespace Xo.TaskTree.Abstractions;

///// <inheritdoc cref="INode"/>
//public abstract class BaseNodeBuilder : INodeBuilder
//{
	//protected readonly IFunctitect _Functitect;
	//protected readonly INodeFactory _NodeFactory;
	//protected readonly IMsgFactory _MsgFactory;
	//protected NodeBuilderTypes _NodeType;

	//protected readonly IList<IMsg> _Params = new List<IMsg>();
	//protected readonly List<INode> _PromisedParams = new List<INode>();
	//protected readonly IList<Func<IWorkflowContext, IMsg>> _ContextParams = new List<Func<IWorkflowContext, IMsg>>();

	//protected IAsyncFunctory? _AsyncFunctory;
	//protected ISyncFunctory? _SyncFunctory;

	//protected ILogger? _Logger;
	//protected IWorkflowContext? _Context;
	//protected Func<Exception, Task>? _ExceptionHandlerAsync;
	//protected Action<Exception>? _ExceptionHandler;

	///// <inheritdoc />
	//public string Id { get; internal set; } = $"{Guid.NewGuid()}";
	//public bool RequiresResult { get; internal set; } = false;
	//public INodeBuilder RequireResult(bool requiresResult = true)
	//{
		//this.RequiresResult = requiresResult;
		//return this;
	//}

	//public bool HasParam(string paramName) => this._Params.Any(p => p.ParamName == paramName);

	//// todo: there is a possibility that this will be null -> if a functory adapter is being used...
	//protected Type? _FunctoryType
	//{
		//get
		//{
			//if (this._AsyncFunctory is not null) return (this._AsyncFunctory as IFunctory)!.ServiceType!;
			//if (this._SyncFunctory is not null) return (this._SyncFunctory as IFunctory)!.ServiceType!;
			//return null;
		//}
	//}

	//protected IAsyncFunctory TypeToFunctory(Type functoryType) => this._Functitect.Build(functoryType).SetServiceType(functoryType).AsAsync();

	///// <inheritdoc />
	//public INodeBuilder AddContext(IWorkflowContext? context)
	//{
		//this._Context = context;
		//return this;
	//}

	///// <inheritdoc />
	//public virtual INodeBuilder AddFunctory<T>(string? nextParamName = null)
	//{
		//// todo: what happens if this is not async?
		//this._AsyncFunctory = this._Functitect.Build<T>(nextParamName).SetServiceType(typeof(T)).AsAsync();
		//return this;
	//}

	///// <inheritdoc />
	//public virtual INodeBuilder AddFunctory(
		//Type serviceType, 
		//string? nextParamName = null
	//)
	//{
		//// todo: what happens if this is not async?
		//this._AsyncFunctory = this._Functitect.Build(serviceType, nextParamName).AsAsync();
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddFunctory<TService, TArg>(
		//TArg arg,
		//string? nextParamName = null
	//)
	//{
		//// todo: what happens if this is not async?

		//if (this._FunctoryType is not null)
		//{
			//this.AddArg<TArg>(arg);

			//this._AsyncFunctory =
				//this._Functitect
					//.Build<TService>(nextParamName: nextParamName)
					//.SetServiceType(typeof(TService))
					//.AsAsync();
		//}
		//else
		//{
			//this._AsyncFunctory =
				//this._Functitect
					//.Build<TService, TArg>(arg: arg, nextParamName: nextParamName)
					//.SetServiceType(typeof(TService))
					//.AsAsync();
		//}

		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddFunctory(Func<IReadOnlyList<IMsg>, Func<Task<IMsg?>>> fn)
	//{
		//this._AsyncFunctory = new AsyncFunctoryAdaptor(fn);
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddFunctory(IAsyncFunctory functory)
	//{
		//this._AsyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddFunctory(ISyncFunctory functory)
	//{
		//this._SyncFunctory = functory ?? throw new ArgumentNullException(nameof(functory));
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddFunctory(Func<IReadOnlyList<IMsg>, Func<IMsg?>> fn)
	//{
		//this._SyncFunctory = new SyncFunctoryAdapter(fn);
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddFunctory(
		//Func<IReadOnlyList<IMsg>, Func<IMsg?>> fn,
		//string nextParamName
	//)
	//{
		//this._SyncFunctory = new SyncFunctoryAdapter(fn).SetNextParamName(nextParamName).AsSync();
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddFunctory(Func<IWorkflowContext, Func<IMsg?>> fn)
	//{
		//this._SyncFunctory = new SyncFunctoryAdapter(fn);
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddArg(params IMsg[] msgs)
	//{
		//foreach (var m in msgs)
		//{
			//this._Params.Add(m);
		//}
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddArg(params INode[] nodes)
	//{
		//foreach (var h in nodes)
		//{
			//this._PromisedParams.Add(h);
		//}
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddArg(params Func<IWorkflowContext, IMsg>[] contextArgs)
	//{
		//foreach (var p in contextArgs)
		//{
			//this._ContextParams.Add(p);
		//}
		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddArg<TArg>(TArg arg)
	//{
		//var type = this._FunctoryType;

		//if (type is null) throw new InvalidOperationException("Unable to find functory type...");

		//string paramName = this.MatchArgToTypeMethodParam<TArg>(type);

		//this._Params.Add(this._MsgFactory.Create<TArg>(arg, paramName));

		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddArg<TArgData>(
		//TArgData data,
		//string paramName
	//)
	//{
		//if (data is null || paramName is null) throw new InvalidOperationException("Null values cannot be passed into AddArg<T>...");

		//this._Params.Add(this._MsgFactory.Create<TArgData>(data, paramName));

		//return this;
	//}

	///// <inheritdoc />
	//public INodeBuilder AddArg<TService, TServiceArg>(TServiceArg arg)
	//{
		//var serviceType = typeof(TService);
		//var serviceTypeArg = typeof(TServiceArg);

		//var method = serviceType.GetMethods().First()!;
		//var parameters = method.GetParameters();
		//var paramName = parameters.First(p => p.ParameterType == serviceTypeArg).Name!;
		//var serviceArg = this._MsgFactory.Create<TServiceArg>(arg, paramName);

		//this.AddArg(serviceType, new IMsg[] { serviceArg });

		//return this;
	//}

	///// <inheritdoc />
	//public virtual INodeBuilder AddArg<TService>() => this.AddArg(typeof(TService));

	///// <inheritdoc />
	//public virtual INodeBuilder AddArg(
		//Type serviceType,
		//IMsg[]? serviceArgs = null
	//)
	//{
		//// Get the name of the parameter that the result of this node will be used for.

		//var functoryServiceType = this._FunctoryType;

		//// in the case of anonymous functories, the functoryServiceType will be null...
		//if (functoryServiceType is null)
		//{
			//var n = this.Build(serviceType);
			//if (serviceArgs is not null) n.AddArg(serviceArgs);
			//this._PromisedParams.Add(n);
			//return this;
		//}

		//var methodInfo = functoryServiceType!.GetMethods().First();
		//var parameterInfo = methodInfo.GetParameters();

		//if (parameterInfo.Length == 0)
		//{
			//var n = this.Build(serviceType);
			//if (serviceArgs is not null) n.AddArg(serviceArgs);
			//this._PromisedParams.Add(n);
			//return this;
		//}

		//if (parameterInfo.Length == 1)
		//{
			//if (this._Params.Any(p => p.ParamName == parameterInfo[0].Name)) return this;

			//var paramName = parameterInfo[0].Name;
			//var n = this.Build(serviceType, nextParamName: paramName);
			//if (serviceArgs is not null) n.AddArg(serviceArgs);
			//this._PromisedParams.Add(n);
			//return this;
		//}

		//if (parameterInfo.Length > 1)
		//{
			//// match on data type...
			//MethodInfo newServiceTypeMethod = serviceType.GetMethods().First();
			//Type? newServiceTypeReturnType = newServiceTypeMethod.ReturnType;

			//var paramName = parameterInfo
				//.Where(p =>
					//!this._Params.Any(_p => _p.ParamName == p.Name) &&
					//p.ParameterType.FullName == newServiceTypeReturnType.FullName
				//)
				//.Select(p => p.Name)
				//.FirstOrDefault();

			//if (paramName is null) return this;

			//var n = this.Build(serviceType, paramName);
			//if (serviceArgs is not null) n.AddArg(serviceArgs);
			//this._PromisedParams.Add(n);
			//return this;
		//}

		//return this;
	//}

	//protected INodeBuilder MatchArgToNodesFunctory<TArg>(
		//INode node,
		//TArg arg
	//)
	//{
		//var serviceType = node.Functory.ServiceType;
		//var argType = typeof(TArg);
		//var method = serviceType!.GetMethods().First()!;
		//var parameters = method.GetParameters();

		//string paramName;
		//if (parameters.Length == 1)
		//{
			//paramName = parameters[0].Name!;
		//}
		//else
		//{
			//// todo: this assumes that there are no parameters of the same type... 
			//paramName = parameters.First(p => p.ParameterType == argType).Name!;
		//}

		//node.AddArg(this._MsgFactory.Create<TArg>(arg, paramName));

		//return this;
	//}

	//protected string MatchArgToTypeMethodParam<TArg>(Type type)
	//{
		//// todo: assumes first method...
		//var method = type.GetMethods().First()!;
		//var parameters = method.GetParameters();
		//string? paramName = null;

		//if (parameters.Length == 1) return parameters[0].Name!;

		//var argType = typeof(TArg);
		//paramName = parameters.Where(p => p.ParameterType == argType).Select(p => p.Name).FirstOrDefault();
		//if (paramName is null) throw new InvalidOperationException("Unable to find parameter name for type TArg...");

		//return paramName;
	//}

	//protected INode BuildBase()
	//{
		//var n = this._NodeFactory.Create(
			//this._NodeType,
			//this._Logger,
			//this.Id,
			//this._Context
		//);

		//if (this._AsyncFunctory is not null) n.SetFunctory(this._AsyncFunctory);
		//if (this._SyncFunctory is not null) n.SetFunctory(this._SyncFunctory);

		//if (this._Params.Any()) n.AddArg(this._Params.ToArray());
		//if (this._PromisedParams.Any()) n.AddArg(this._PromisedParams.ToArray());
		//if (this._ContextParams.Any()) n.AddArg(this._ContextParams.ToArray());

		//n.RequireResult(this.RequiresResult);

		//if (this._ExceptionHandlerAsync is not null) n.SetExceptionHandler(this._ExceptionHandlerAsync);
		//if (this._ExceptionHandler is not null) n.SetExceptionHandler(this._ExceptionHandler);

		//return n;
	//}

	//public abstract INode Build();

	//protected virtual INode Build(
		//Type serviceType,
		//string? methodName = null,
		//string? nextParamName = null
	//)
	//{
		//var functory = this._Functitect.Build(serviceType, methodName, nextParamName).AsAsync();
		//return this._NodeFactory.Create().SetFunctory(functory);
	//}

	///// <inheritdoc />
	//public INodeBuilder SetExceptionHandler(Func<Exception, Task> handler)
	//{
		//this._ExceptionHandlerAsync = handler;
		//return this;
	//}

	//// /// <inheritdoc />
	//public INodeBuilder SetExceptionHandler(Action<Exception> handler)
	//{
		//this._ExceptionHandler = handler;
		//return this;
	//}

	//protected virtual void PropogateMsg(
		//IMsg? msg,
		//INode next
	//)
	//{
		//var targetServiceType = next.Functory.ServiceType;
		//var targetMethodInfo = targetServiceType!.GetMethods().First();
		//var targetMethodParams = targetMethodInfo.GetParameters();

		//// How to get the param name here?
		//var outstandingParams = targetMethodParams.Where(p => !next.HasParam(p.Name!)).ToArray();
		//string paramName;

		//if (outstandingParams.Length == 1) paramName = outstandingParams.First().Name!;
		//else
		//{
			//var msgType = msg!.GetDataType();
			//paramName = outstandingParams.First(p => p.ParameterType == msgType).Name!;
		//}

		//if (paramName is null) throw new ArgumentException("Unable to find parameter name for type TArg...");

		//var clone = Functitect.CreateMsg(msg!.ObjectData, paramName);
		//next.AddArg(clone);
	//}

	//protected virtual async Task<IMsg?[]> RunNext(
		//IMsg? msg,
		//INode next,
		//CancellationToken cancellationToken
	//)
	//{
		//next.SetContext(this._Context);
		//next.Validate();
		//await next.ResolvePromisedParams(cancellationToken);
		//next.AddContextParamResultsToParams();

		//if (next.RequiresResult)
		//{
			//// todo: handle propogating null...
			//this.PropogateMsg(msg, next);
		//}

		//return await next.ResolveFunctory(cancellationToken);
	//}

	//protected virtual IMsg? CloneMsg(IMsg? msg)
	//{
		//if (msg is null) return null;

		//var clone = Functitect.CreateMsg(msg.ObjectData, msg.ParamName);

		//return clone;
	//}

	///// <summary>
	/////   Initializes a new instance of <see cref="BaseNodeBuilder"/>. 
	///// </summary>
	//public BaseNodeBuilder(
		//IFunctitect functitect,
		//INodeFactory nodeFactory,
		//IMsgFactory msgFactory,
		//ILogger? logger = null,
		//string? id = null,
		//IWorkflowContext? context = null
	//)
	//{
		//this._Functitect = functitect ?? throw new ArgumentNullException(nameof(functitect));
		//this._NodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
		//this._MsgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
		//this._Logger = logger;
		//if (id is not null) this.Id = id;
		//this._Context = context;
	//}
//}
