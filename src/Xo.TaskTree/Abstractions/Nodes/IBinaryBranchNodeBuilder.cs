namespace Xo.TaskTree.Abstractions;

public interface IBinaryBranchNodeBuilder : IBranchNodeBuilder
{
	IBinaryBranchNodeBuilder AddTrue<TTrue>(bool requiresResult = true);
	IBinaryBranchNodeBuilder AddFalse<TFalse>(bool requiresResult = true);
	IBinaryBranchNodeBuilder AddTrue<TTrue, TArgs>(
		TArgs args,
		bool requiresResult = true
	);
	IBinaryBranchNodeBuilder AddFalse<TFalse, TArgs>(
		TArgs args,
		bool requiresResult = true
	);
	IBinaryBranchNodeBuilder AddTrue(INode node);
	IBinaryBranchNodeBuilder AddFalse(INode node);
	IBinaryBranchNodeBuilder AddPathResolver(Func<IMsg?, bool> pathResolver);
	IBinaryBranchNodeBuilder AddPathResolver(IBinaryBranchNodePathResolver pathResolver);
	IBinaryBranchNodeBuilder AddIsNotNullPathResolver();
	IBinaryBranchNodeBuilder IsNotNull<TService, TArg>(TArg arg);
	IBinaryBranchNodeBuilder IsNotNull<TService>();
}