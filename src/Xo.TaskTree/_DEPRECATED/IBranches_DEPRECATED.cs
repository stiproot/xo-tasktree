//namespace Xo.TaskTree.Abstractions;

//public interface IRootBranch
//{
    //IStateManager Root<T>(Action<INodeConfigurationBuilder>? configure = null);
//}

//public interface IIfBranch
//{
    //IStateManager If<T>(Action<INodeConfigurationBuilder>? configure = null);
    //IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null);
//}

//public interface IThenBranch
//{
    //IStateManager Then<T>(
        //Action<INodeConfigurationBuilder>? configure = null,
        //Action<IStateManager>? then = null
    //);
//}

//public interface IKeyBranch
//{
    //IStateManager Key<T>(
        //Action<INodeConfigurationBuilder>? configure = null,
        //Action<IStateManager>? then = null
    //);
//}

//public interface IHashBranch
//{
    //IStateManager Hash<T, U>(
        //Action<INodeConfigurationBuilder>? configureT = null,
        //Action<INodeConfigurationBuilder>? configureU = null,
        //Action<IStateManager>? thenT = null,
        //Action<IStateManager>? thenU = null
    //);

    //IStateManager Hash<T, U, V>(
        //Action<INodeConfigurationBuilder>? configureT = null,
        //Action<INodeConfigurationBuilder>? configureU = null,
        //Action<INodeConfigurationBuilder>? configureV = null,
        //Action<IStateManager>? thenT = null
    //);
//}

//public interface IElseBranch
//{
    //IStateManager Else<T>(
        //Action<INodeConfigurationBuilder>? configure = null,
        //Action<IStateManager>? then = null
    //);
//}

//public interface IBranchBranch
//{
    //IStateManager Branch<T, U>(
        //Action<INodeConfigurationBuilder>? configureT = null,
        //Action<INodeConfigurationBuilder>? configureU = null,
        //Action<IStateManager>? thenT = null,
        //Action<IStateManager>? thenU = null
    //);
    //IStateManager Branch<T, U, V>(
        //Action<INodeConfigurationBuilder>? configureT = null,
        //Action<INodeConfigurationBuilder>? configureU = null,
        //Action<INodeConfigurationBuilder>? configureV = null,
        //Action<IStateManager>? thenT = null,
        //Action<IStateManager>? thenU = null,
        //Action<IStateManager>? thenV = null
    //);

//}

//public interface IPathBranch
//{
    //IStateManager Path<T, U, V>(
        //Action<INodeConfigurationBuilder>? configureT = null,
        //Action<INodeConfigurationBuilder>? configureU = null,
        //Action<INodeConfigurationBuilder>? configureV = null
    //);
//}