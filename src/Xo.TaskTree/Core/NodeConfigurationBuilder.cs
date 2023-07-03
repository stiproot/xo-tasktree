namespace Xo.TaskTree.Core;

public class NodeConfigurationBuilder : INodeConfigurationBuilder
{
	private readonly INodeConfiguration _config = new NodeConfiguration();
	private Type? _serviceType;

	public INodeConfigurationBuilder RequireResult()
	{
		this._config.RequiresResult = true;
		return this;
	}

	public INodeConfigurationBuilder AddServiceType(Type serviceType)
	{
		this._serviceType = serviceType;
		return this;
	}

	public INodeConfigurationBuilder NextParam(string nextParamName)
	{
		this._config.NextParamName = nextParamName;
		return this;
	}

	public INodeConfigurationBuilder Key(string key)
	{
		this._config.Key = key;
		return this;
	}

	public INodeConfigurationBuilder AddArg<T>(
			T data,
			string paramName
	)
	{
		var msg = new Msg<T>(data, paramName);

		this._config.Args.Add(msg);

		return this;
	}

	public INodeConfigurationBuilder MatchArg<T>(T arg)
	{
		if (this._serviceType is null) throw new InvalidOperationException($"{nameof(NodeConfigurationBuilder)}.{nameof(MatchArg)}<T> - fn-type is null.");

		var argType = typeof(T);

		var serviceParamName = TypeInspector.MatchTypeToParamType(argType, this._serviceType);

		var msg = new Msg<T>(arg, serviceParamName);

		this._config.Args.Add(msg);

		return this;
	}

	public INodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		if (this._serviceType is null) throw new InvalidOperationException($"{nameof(NodeConfigurationBuilder)}.{nameof(MatchArg)}<T> - fn-type is null.");

		var argType = typeof(T);

		var arg = argType.ToMetaNode(configure);

		var serviceParamName = TypeInspector.MatchReturnTypeToParamType(argType, this._serviceType);

		arg.NodeConfiguration!.NextParamName = serviceParamName;

		this._config.PromisedArgs.Add(arg);

		return this;
	}

	// todo: delay build operation, or just reference config directly -> Build() is misleading.
	public INodeConfiguration Build() => this._config;

	public NodeConfigurationBuilder() { }
	public NodeConfigurationBuilder(
		INodeConfiguration nodeConfiguration,
		Type serviceType
	)
		=> (this._config, this._serviceType) = (nodeConfiguration, serviceType);
	public NodeConfigurationBuilder(Type serviceType) => this._serviceType = serviceType;
}