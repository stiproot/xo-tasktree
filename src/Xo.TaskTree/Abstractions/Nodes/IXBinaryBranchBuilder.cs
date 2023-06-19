namespace Xo.TaskTree.Abstractions;

public interface IXBinaryBranchBuilder
{
	IXBinaryBranchBuilder AddTrue<TTrue>(Action<INodeConfigurationBuilder>? configure = null);
	IXBinaryBranchBuilder AddTrue<TTrue, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IXBinaryBranchBuilder AddTrue(INode node);

	IXBinaryBranchBuilder AddFalse<TFalse>(Action<INodeConfigurationBuilder>? configure = null);
	IXBinaryBranchBuilder AddFalse<TFalse, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IXBinaryBranchBuilder AddFalse(INode node);

	IXBinaryBranchBuilder IsNotNull<TService, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IXBinaryBranchBuilder IsNotNull<TService>(Action<INodeConfigurationBuilder>? configure = null);
}