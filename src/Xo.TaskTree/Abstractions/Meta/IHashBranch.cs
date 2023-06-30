namespace Xo.TaskTree.Abstractions;

public interface IHashBranch
{
    IStateManager Key<T>(
        Action<INodeConfigurationBuilder>? configure = null
    );
    IStateManager Hash<T, U>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<IStateManager>? thenT = null,
        Action<IStateManager>? thenU = null
    );
    IStateManager Hash<T, U, V>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<INodeConfigurationBuilder>? configureV = null,
        Action<IStateManager>? thenT = null,
        Action<IStateManager>? thenU = null,
        Action<IStateManager>? thenV = null
    );
}