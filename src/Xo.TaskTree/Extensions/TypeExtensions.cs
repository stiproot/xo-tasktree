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
			configure(new NodeConfigurationBuilder(metaNode.NodeConfiguration, @this));
		}

		return metaNode;
	}

	public static IFn ToFn(this Type @this,
		IFnFactory fnFactory
	)
		=> fnFactory.Build(@this).SetServiceType(@this).AsAsync();

	public static IFn ToFn(this Type @this,
		IFnFactory fnFactory,
		string? nextParamName
	)
		=> fnFactory.Build(@this, nextParamName: nextParamName).SetServiceType(@this).AsAsync();
}