namespace Xo.TaskTree.Core;

public class FlowBuilder : IFlowBuilder
{
    public IFlowBuilder Root<T>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Root<T>(Action<INodeConfigurationBuilder> config)
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Root<T>(Action<IFlowBuilder> arg)
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Root<T>(
        Action<IFlowBuilder> arg,
        Action<INodeConfigurationBuilder> config
    )
    {
        throw new NotImplementedException();
    }

    public INode Root<T>(
        Action<INodeConfigurationBuilder> config,
        Action<IFlowBuilder> arg,
        Action<IFlowBuilder> then
    )
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Arg<T>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Arg(Action<IFlowBuilder> arg)
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Arg<T>(Action<INodeConfigurationBuilder> config)
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder AsArg<T>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder AsArgs<T, U, V>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder If<T>(Action<INodeConfigurationBuilder>? config = null)
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder If<T>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    )
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Then<T>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Then<T>(Action<INodeConfigurationBuilder> config)
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Then<T>(
        Action<IFlowBuilder> then,
        Action<INodeConfigurationBuilder> config
    )
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder If<T, TResolver>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    )
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder If<T>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else,
        Action<INodeConfigurationBuilder> config
    )
    {
        throw new NotImplementedException();
    }

    public INode If<T>(
        Action<INodeConfigurationBuilder> config,
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else
    )
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder If<T, TResolver>(
        Action<IFlowBuilder> then,
        Action<IFlowBuilder> @else,
        Action<INodeConfigurationBuilder> config
    )
    {
        throw new NotImplementedException();
    }


    public IFlowBuilder Else<T>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Else<T>(Action<INodeConfigurationBuilder> config)
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Else<T>(
        Action<IFlowBuilder> @else,
        Action<INodeConfigurationBuilder> config
    )
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Pool<T, U, V>()
    {
        throw new NotImplementedException();
    }

    public INode Pool<T, U, V>(
        Action<INodeConfigurationBuilder> tConfig,
        Action<INodeConfigurationBuilder> uConfig,
        Action<INodeConfigurationBuilder> vConfig,
        Action<IFlowBuilder>? then = null
    )
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Next<T>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Hash<T, U, V>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Node<T>()
    {
        throw new NotImplementedException();
    }

    public IFlowBuilder Node<T>(Action<INodeConfigurationBuilder> config)
    {
        throw new NotImplementedException();
    }

    public virtual INode Build() => throw new NotImplementedException();
}