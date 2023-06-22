namespace Xo.TaskTree.Abstractions;

public interface IPathBranch
{
    IStateManager Path<T, U, V>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<INodeConfigurationBuilder>? configureV = null
    );
}