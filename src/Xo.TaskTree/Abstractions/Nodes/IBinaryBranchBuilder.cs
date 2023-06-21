namespace Xo.TaskTree.Abstractions;

public interface IBinaryBranchBuilder : IBranchBuilder
{
	IBinaryBranchBuilder AddTrue<TTrue>(Action<INodeConfigurationBuilder>? configure = null);
	IBinaryBranchBuilder AddTrue<TTrue, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IBinaryBranchBuilder AddTrue(INode node);

	IBinaryBranchBuilder AddFalse<TFalse>(Action<INodeConfigurationBuilder>? configure = null);
	IBinaryBranchBuilder AddFalse<TFalse, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IBinaryBranchBuilder AddFalse(INode node);

	IBinaryBranchBuilder IsNotNull<TService, TArg>(
		TArg arg,
		Action<INodeConfigurationBuilder>? configure = null
	);
	IBinaryBranchBuilder IsNotNull<TService>(Action<INodeConfigurationBuilder>? configure = null);
}