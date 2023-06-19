namespace Xo.TaskTree.Abstractions;

public interface IFlowBuilder
{
    IFlowBuilder Root<T>();
    IFlowBuilder Root<T>(Action<INodeConfigurationBuilder> config);
    IFlowBuilder Root<T>(Action<IFlowBuilder> arg);
    IFlowBuilder Root<T>(
        Action<IFlowBuilder> arg,
        Action<INodeConfigurationBuilder> config
    );
    INode Root<T>(
        Action<INodeConfigurationBuilder> config,
        Action<IFlowBuilder> arg,
        Action<IFlowBuilder> next
    );

    IFlowBuilder Arg<T>();
    IFlowBuilder Arg<T>(Action<INodeConfigurationBuilder> config);
    IFlowBuilder Arg(Action<IFlowBuilder> arg);

    IFlowBuilder AsArg<T>();
    IFlowBuilder AsArgs<T, U, V>();

    IFlowBuilder If<T>(Action<INodeConfigurationBuilder>? config = null);
    IFlowBuilder If<T>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    );
    IFlowBuilder If<T, TResolver>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    );
    IFlowBuilder If<T>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else,
        Action<INodeConfigurationBuilder> config
    );
    IFlowBuilder If<T, TResolver>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else,
        Action<INodeConfigurationBuilder> config
    );

    INode If<T>(
        Action<INodeConfigurationBuilder> config,
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    );

    IFlowBuilder Then<T>();
    IFlowBuilder Then<T>(Action<INodeConfigurationBuilder> config);
    IFlowBuilder Then<T>(
        Action<IFlowBuilder> then,
        Action<INodeConfigurationBuilder> config
    );

    IFlowBuilder Else<T>();
    IFlowBuilder Else<T>(Action<INodeConfigurationBuilder> config);

    IFlowBuilder Pool<T, U, V>();
    INode Pool<T, U, V>(
        Action<INodeConfigurationBuilder> tConfig,
        Action<INodeConfigurationBuilder> uConfig,
        Action<INodeConfigurationBuilder> vConfig,
        Action<IFlowBuilder>? then = null
    );

    IFlowBuilder Next<T>();
    IFlowBuilder Hash<T, U, V>();

    IFlowBuilder Node<T>();
    IFlowBuilder Node<T>(Action<INodeConfigurationBuilder> config);

    INode Build();
}