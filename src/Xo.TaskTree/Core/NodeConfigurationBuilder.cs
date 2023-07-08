namespace Xo.TaskTree.Core;

public class NodeConfigurationBuilder : INodeConfigurationBuilder
{
	private readonly INodeConfiguration _nodeConfiguration = new NodeConfiguration();
	private Type? _serviceType;

	public Type ServiceType => this._serviceType!;

	public INodeConfigurationBuilder RequireResult()
	{
		this._nodeConfiguration.RequiresResult = true;
		return this;
	}

	public INodeConfigurationBuilder IgnorePromisedResults()
	{
		this._nodeConfiguration.IgnoresPromisedResults = true;
		return this;
	}

	public INodeConfigurationBuilder AddServiceType(Type serviceType)
	{
		this._serviceType = serviceType;
		return this;
	}

	public INodeConfigurationBuilder NextParam(string nextParamName)
	{
		this._nodeConfiguration.NextParamName = nextParamName;
		return this;
	}

	public INodeConfigurationBuilder Key(string key)
	{
		this._nodeConfiguration.Key = key;
		return this;
	}

	public INodeConfigurationBuilder AddArg<T>(
			T data,
			string paramName
	)
	{
		var msg = new Msg<T>(data, paramName);

		this._nodeConfiguration.Args.Add(msg);

		return this;
	}

	public INodeConfigurationBuilder AddArg(params INode[] args)
	{
		foreach (var a in args)
		{
			this._nodeConfiguration.PromisedArgs.Add(a);
		}

		return this;
	}

	public INodeConfigurationBuilder AddArg(params IMetaNode[] args)
	{
		foreach (var a in args)
		{
			this._nodeConfiguration.MetaPromisedArgs.Add(a);
		}

		return this;
	}

	public INodeConfigurationBuilder AddArg(params IMsg?[] args)
	{
		foreach (var a in args)
		{
			if(a is null)
			{
				continue;
			}

			this._nodeConfiguration.Args.Add(a);
		}

		return this;
	}

	public INodeConfigurationBuilder AddArg(params Func<IWorkflowContext, IMsg>[] args)
	{
		foreach (var a in args)
		{
			this._nodeConfiguration.ContextArgs.Add(a);
		}

		return this;
	}

	public INodeConfigurationBuilder MatchArg<T>(T arg)
	{
		if (this._serviceType is null) throw new InvalidOperationException($"{nameof(NodeConfigurationBuilder)}.{nameof(MatchArg)}<T> - fn-type is null.");

		var argType = typeof(T);

		var serviceParamName = TypeInspector.MatchTypeToParamType(argType, this._serviceType);

		var msg = new Msg<T>(arg, serviceParamName);

		this._nodeConfiguration.Args.Add(msg);

		return this;
	}

	public INodeConfigurationBuilder MatchArgs<T>(T arg)
	{
		if (this._serviceType is null) throw new InvalidOperationException($"{nameof(NodeConfigurationBuilder)}.{nameof(MatchArg)}<T> - fn-type is null.");

		var argType = typeof(T);

		var serviceParamName = TypeInspector.MatchTypeToParamType(this._nodeConfiguration, argType, this._serviceType);

		var msg = new Msg<T>(arg, serviceParamName);

		this._nodeConfiguration.Args.Add(msg);

		return this;
	}

	public INodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		if (this._serviceType is null) throw new InvalidOperationException($"{nameof(NodeConfigurationBuilder)}.{nameof(MatchArg)}<T> - fn-type is null.");

		var argType = typeof(T);

		var arg = argType.ToMetaNode(configure);

		var serviceParamName = TypeInspector.MatchReturnTypeToParamType(argType, this._serviceType);

		arg.NodeConfiguration!.NextParamName = serviceParamName;

		this._nodeConfiguration.MetaPromisedArgs.Add(arg);

		return this;
	}

	public INodeConfigurationBuilder SetId(string id)
	{
		this._nodeConfiguration.Id = id;
		return this;
	}

	public INodeConfigurationBuilder AddContext(IWorkflowContext? workflowContext)
	{
		this._nodeConfiguration.WorkflowContext = workflowContext;
		return this;
	}

	public INodeConfigurationBuilder ControllerType(ControllerTypes controllerType)
	{
		this._nodeConfiguration.ControllerType = controllerType;
		return this;
	}

	public INodeConfiguration Configuration() => this._nodeConfiguration;

	public NodeConfigurationBuilder() { }
	public NodeConfigurationBuilder(
		INodeConfiguration nodeConfiguration,
		Type serviceType
	)
		=> (this._nodeConfiguration, this._serviceType) = (nodeConfiguration, serviceType);
	public NodeConfigurationBuilder(Type serviceType) => this._serviceType = serviceType;
	public NodeConfigurationBuilder(INodeConfiguration nodeConfiguration) => this._nodeConfiguration = nodeConfiguration;
}