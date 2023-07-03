namespace Xo.TaskTree.Core;

internal static class LambdaExtensions
{
	public static INodeConfiguration? Build(this Action<INodeConfigurationBuilder>? @this,
			Type serviceType
	)
	{
		if (@this is null) return null;

		var builder = new NodeConfigurationBuilder(serviceType);

		@this(builder);

		return builder.Build();
	}

	public static INodeConfiguration? SafeBuild(this Action<INodeConfigurationBuilder>? @this,
			Type serviceType
	)
	{
		var builder = new NodeConfigurationBuilder(serviceType);

		if (@this is not null) @this(builder);

		return builder.Build();
	}
}