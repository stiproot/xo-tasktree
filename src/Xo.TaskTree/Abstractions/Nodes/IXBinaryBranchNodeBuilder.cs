namespace Xo.TaskTree.Abstractions;

public interface IXBinaryBranchNodeBuilder
{
	IXBinaryBranchNodeBuilder AddTrue<TTrue>(Action<INodeConfigurationBuilder>? configure = null);
	IXBinaryBranchNodeBuilder AddFalse<TFalse>(Action<INodeConfigurationBuilder>? configure = null);
	IXBinaryBranchNodeBuilder AddTrue<TTrue, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IXBinaryBranchNodeBuilder AddFalse<TFalse, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IXBinaryBranchNodeBuilder IsNotNull<TService, TArg>(TArg arg);
	IXBinaryBranchNodeBuilder IsNotNull<TService>();

	// IXBinaryBranchNodeBuilder AddTrue(INode node);
	// IXBinaryBranchNodeBuilder AddFalse(INode node);
	// IXBinaryBranchNodeBuilder AddPathResolver(Func<IMsg?, bool> pathResolver);
	// IXBinaryBranchNodeBuilder AddPathResolver(IBinaryBranchNodePathResolver pathResolver);
	// IXBinaryBranchNodeBuilder AddIsNotNullPathResolver();
}