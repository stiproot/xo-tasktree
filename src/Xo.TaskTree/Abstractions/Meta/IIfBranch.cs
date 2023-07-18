namespace Xo.TaskTree.Abstractions;

public interface IIfBranch
{
	IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null);

	IStateManager IsNotNull<T>(Action<INodeConfigurationBuilder>? configure = null);

	IStateManager If<T>(Action<INodeConfigurationBuilder>? configure = null);

	IStateManager Then<T>(
		Action<INodeConfigurationBuilder>? configure = null,
		Action<IStateManager>? then = null
	);

	IStateManager Else<T>(
		Action<INodeConfigurationBuilder>? configure = null,
		Action<IStateManager>? then = null
	);
}