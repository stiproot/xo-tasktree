namespace Xo.TaskTree.Core;

public class NodeConfigurationBuilder : INodeConfigurationBuilder
{
	private readonly INodeConfiguration _config = new NodeConfiguration();
	private Type? _functoryType;

	public INodeConfigurationBuilder RequireResult()
	{
		this._config.RequiresResult = true;
		return this;
	}

	public INodeConfigurationBuilder AddFnType(Type functoryType)
	{
		this._functoryType = functoryType;
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
		if (this._functoryType is null) throw new InvalidOperationException($"{nameof(NodeConfigurationBuilder)}.{nameof(MatchArg)}<T> - functory-type is null.");

		var argType = typeof(T);

		var functoryParamName = TypeInspector.MatchTypeToParamType(argType, this._functoryType);

		var msg = new Msg<T>(arg, functoryParamName);

		this._config.Args.Add(msg);

		return this;
	}

	public INodeConfigurationBuilder MatchArg<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		if (this._functoryType is null) throw new InvalidOperationException($"{nameof(NodeConfigurationBuilder)}.{nameof(MatchArg)}<T> - functory-type is null.");

		var argType = typeof(T);

		var arg = argType.ToMetaNode(configure);

		var functoryParamName = TypeInspector.MatchReturnTypeToParamType(argType, this._functoryType);

		arg.NodeConfiguration!.NextParamName = functoryParamName;

		this._config.PromisedArgs.Add(arg);

		return this;
	}

	// todo: delay build operation, or just reference config directly -> Build() is misleading.
	public INodeConfiguration Build() => this._config;

	public NodeConfigurationBuilder() { }
	public NodeConfigurationBuilder(
		INodeConfiguration nodeConfiguration,
		Type functoryType
	)
		=> (this._config, this._functoryType) = (nodeConfiguration, functoryType);
	public NodeConfigurationBuilder(Type functoryType) => this._functoryType = functoryType;
}