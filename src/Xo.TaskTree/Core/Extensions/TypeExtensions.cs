namespace Xo.TaskTree.Core;

internal static class TypeExtensions
{
	/* todo: this is sexy... but benchmarks need to be done... */
	//public static IMetaNode ToMetaNode(this Type @this,
			//Action<INodeConfigurationBuilder>? configure = null,
			//MetaNodeTypes nodeType = MetaNodeTypes.Default,
			//bool safe = false
	//)
		//=> new MetaNode(@this) { NodeType = nodeType }.Configure(safe ? configure.SafeBuild(@this) : configure.Build(@this));

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

	public static IAsyncFunctoryInvoker ToFunctory(this Type @this,
				IFunctitect functitect
	)
		=> functitect.Build(@this).SetServiceType(@this).AsAsync();

	public static IAsyncFunctoryInvoker ToFunctory(this Type @this,
				IFunctitect functitect,
				string? nextParamName
	)
		=> functitect.Build(@this, nextParamName: nextParamName).SetServiceType(@this).AsAsync();

	public static INode ToNode(this Type @this,
		IFunctitect functitect,
		string? methodName = null,
		string? nextParamName = null
	)
	{
		var functory = functitect.Build(@this, methodName, nextParamName).AsAsync();

		// todo: what about other dependencies?...
		return new Node().SetFunctory(functory);
	}
}