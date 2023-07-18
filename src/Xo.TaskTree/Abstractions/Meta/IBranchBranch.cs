namespace Xo.TaskTree.Abstractions;

public interface IBranchBranch
{
	IStateManager Branch<T, U>(
		Action<INodeConfigurationBuilder>? configureT = null,
		Action<INodeConfigurationBuilder>? configureU = null,
		Action<IStateManager>? thenT = null,
		Action<IStateManager>? thenU = null
	);

	IStateManager Branch<T, U, V>(
		Action<INodeConfigurationBuilder>? configureT = null,
		Action<INodeConfigurationBuilder>? configureU = null,
		Action<INodeConfigurationBuilder>? configureV = null,
		Action<IStateManager>? thenT = null,
		Action<IStateManager>? thenU = null,
		Action<IStateManager>? thenV = null
	);
}