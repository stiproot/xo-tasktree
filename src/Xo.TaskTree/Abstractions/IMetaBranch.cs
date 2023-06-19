namespace Xo.TaskTree.Abstractions;

public interface IAllish : IIfMetaBranch, IThenMetaBranch, IElseMetaBranch
{
}

public interface IRootish : IIfMetaBranch
{
}

public interface IPathish : IThenMetaBranch, IElseMetaBranch
{
}

public interface IIfMetaBranch
{
    IPathish If<T>(Action<INodeConfigurationBuilder>? config = null);
    IPathish If<T>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    );
    IPathish If<T, TResolver>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    );
    IPathish If<T>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else,
        Action<INodeConfigurationBuilder> config
    );
    IPathish If<T, TResolver>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else,
        Action<INodeConfigurationBuilder> config
    );
    IPathish If<T>(
        Action<INodeConfigurationBuilder> config,
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    );
}

public interface IThenMetaBranch
{
    IAllish Then<T>();
    IAllish Then<T>(Action<INodeConfigurationBuilder> config);
    IAllish Then<T>(
        Action<IFlowBuilder> then,
        Action<INodeConfigurationBuilder> config
    );
}

public interface IElseMetaBranch
{
    IAllish Else<T>();
    IAllish Else<T>(Action<INodeConfigurationBuilder> config);
    IAllish Else<T>(
        Action<IFlowBuilder> then,
        Action<INodeConfigurationBuilder> config
    );
}