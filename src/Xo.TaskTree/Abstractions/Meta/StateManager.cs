namespace Xo.TaskTree.Abstractions;

public class StateManager : IStateManager 
{
    public IMetaNode RootNode{ get; set; }
    public IMetaNode StateNode { get; set; }

    public IStateManager Root<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        this.RootNode = this.StateNode = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default }.Configure(configure.Build());

        return this;
    }

    public IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        // todo: double check pointers...
        this.RootNode = this.StateNode = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Binary }.Configure(configure.Build());

        return this;
    }

    public IStateManager If<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        throw new NotImplementedException();
    }

    public IStateManager Then<T>(
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    )
    {
        IMetaNode transition = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default };

        if(this.StateNode.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge { };

        IMetaNode? @ref = this.StateNode.NodeType switch
        {
            MetaNodeTypes.Binary => this.StateNode.NodeEdge.True,
            _ => this.StateNode.NodeEdge.Next
        };

        return this.Transition(@ref, transition, configure, then);
    }

    public IStateManager Else<T>(
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    )
    {
        IMetaNode transition = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default };

        if(this.StateNode.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge { };

        IMetaNode? @ref = this.StateNode.NodeEdge.False;

        return this.Transition(@ref, transition, configure, then);
    }

    private IStateManager Transition(
        IMetaNode? @ref,
        IMetaNode transition,
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    )
    {
        INodeConfiguration? config = configure.Build();
        transition.Configure(config);

        /* PROCESS THEN */
        if(then is not null)
        {
            // NEW LEVEL
            IStateManager manager = new StateManager();
            then(manager);
            IMetaNode root = manager.RootNode;

            transition.NodeEdge = new MetaNodeEdge { Next = root };

            // todo: should this.StateNode not be set to the lower levels root?
            // IMetaNode state = manager.StateNode;
            // this.StateNode = state;
        }

        @ref = this.StateNode = transition;

        return this;
    }
}