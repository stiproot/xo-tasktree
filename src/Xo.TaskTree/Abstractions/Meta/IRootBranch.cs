namespace Xo.TaskTree.Abstractions;

public interface IRootBranch
{
	IStateManager Root<T>(Action<INodeConfigurationBuilder>? configure = null);
}