namespace Xo.TaskTree.Abstractions;

public class StateManager : IStateManager 
{
    public IMetaNode RootNode{ get; set; }
    public IMetaNode StateNode { get; set; }

    public IStateManager Root<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        this.RootNode = this.StateNode = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default, NodeEdge = new MetaNodeEdge() };

        if(configure is null) return this;

        // todo: process config...
        return this;
    }

    public IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        // todo: double check pointers...
        this.RootNode = this.StateNode = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Binary, NodeEdge = new MetaNodeEdge() };

        if(configure is null) return this;

        // todo: process config...
        return this;
    }

    public IStateManager If<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        throw new NotImplementedException();
    }

    public IStateManager Then<T>(
        Action<IStateManager>? then = null,
        Action<INodeConfigurationBuilder>? configure = null
    )
    {
        if(this.StateNode.NodeType is MetaNodeTypes.Binary)
        {
            this.StateNode.NodeEdge.True = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default }; // we do not presume to define a node-edge...

            /* PROCESS CONFIGURATION */ 
            if(configure is not null)
            {
                var configBuilder = new NodeConfigurationBuilder();
                configure(configBuilder);
                INodeConfiguration config = configBuilder.Build();

                this.StateNode.NodeEdge.True.NodeConfiguration = config;

                // todo: should this not be removed?... as it is in config...
                this.StateNode.NodeEdge.True.PromisedArgs.AddRange(config.PromisedArgs);
                this.StateNode.NodeEdge.True.Args.AddRange(config.Args);
            }

            /* PROCESS THEN */
            if(then is not null)
            {
                // NEW LEVEL
                IStateManager manager = new StateManager();

                then(manager);

                IMetaNode root = manager.RootNode;

                this.StateNode.NodeEdge.True.NodeEdge = new MetaNodeEdge { Next = root };
            }
        }
        else
        {
            this.StateNode.NodeEdge.Next = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default }; // we do not presume to define a node-edge...

            /* PROCESS CONFIGURATION */ 
            if(configure is not null)
            {
                var configBuilder = new NodeConfigurationBuilder();
                configure(configBuilder);
                INodeConfiguration config = configBuilder.Build();

                this.StateNode.NodeEdge.Next.NodeConfiguration = config;

                // todo: should this not be removed?... as it is in config...
                this.StateNode.NodeEdge.Next.PromisedArgs.AddRange(config.PromisedArgs);
                this.StateNode.NodeEdge.Next.Args.AddRange(config.Args);
            }

            this.StateNode = this.StateNode.NodeEdge.Next;
        }

        return this;
    }

    public IStateManager Else<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        if(this.StateNode.NodeType is MetaNodeTypes.Binary)
        {
            this.StateNode.NodeEdge.False = new MetaNode{ FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default }; // we do not presume to define a node-edge...

            // todo: process configuration...
        }
        else
        {
            // todo: update State to this else?
        }

        return this;
    }
}