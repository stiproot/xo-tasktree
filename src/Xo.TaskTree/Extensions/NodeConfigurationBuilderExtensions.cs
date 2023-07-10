namespace Xo.TaskTree.Core;

public static class NodeConfigurationBuilderExtensions
{
	public static INodeConfigurationBuilder AddArg<TArg>(this INodeConfigurationBuilder @this,
		TArg arg
	)
	{
		string paramName = TypeInspector.MatchTypeToParamType(typeof(TArg), @this.ServiceType);

		@this.AddArg(SMsgFactory.Create<TArg>(arg, paramName));

		return @this;
	}

	public static Action<INodeConfigurationBuilder> Enrich(this Action<INodeConfigurationBuilder>? @this,
		Action<INodeConfigurationBuilder> enrichment
	)
		=> (Action<INodeConfigurationBuilder>)Delegate.Combine(@this, enrichment);
}