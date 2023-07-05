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
		return new Node().SetFn(fn);
	}
}