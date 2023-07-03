namespace Xo.TaskTree.Core;

internal static class TypeExtensions
{
	public static IMetaNode ToMetaNode(this Type @this,
			Action<INodeConfigurationBuilder>? configure = null,
			MetaNodeTypes nodeType = MetaNodeTypes.Default
	)
	{
		var metaNode = new MetaNode(@this) { NodeType = nodeType };

		if (configure is not null)
		{
			var builder = new NodeConfigurationBuilder(metaNode.NodeConfiguration, @this);

			configure(builder);
		}

		return metaNode;
	}

	public static IAsyncFn ToFn(this Type @this,
		IFnFactory fnFactory
	)
		=> fnFactory.Build(@this).SetServiceType(@this).AsAsync();

	public static IAsyncFn ToFn(this Type @this,
		IFnFactory fnFactory,
		string? nextParamName
	)
		=> fnFactory.Build(@this, nextParamName: nextParamName).SetServiceType(@this).AsAsync();

	public static INode ToNode(this Type @this,
		IFnFactory fnFactory,
		string? methodName = null,
		string? nextParamName = null
	)
	{
		var fn = fnFactory.Build(@this, methodName, nextParamName).AsAsync();

		// todo: what about other dependencies?...
		return new Node().SetFn(fn);
	}
}