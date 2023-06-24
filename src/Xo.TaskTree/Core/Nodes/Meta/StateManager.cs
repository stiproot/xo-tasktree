namespace Xo.TaskTree.Core;

public class StateManager : IStateManager 
{
    private readonly IMetaNodeMapper _metaNodeMapper;
    public IMetaNode? RootNode { get; set; }
    public IMetaNode? StateNode { get; set; }

    public IStateManager Root<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        this.RootNode = this.StateNode = new MetaNode(typeof(T)) { NodeType = MetaNodeTypes.Default }.Configure(configure.Build());

        return this;
    }

    public IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        // todo: double check pointers...
        this.RootNode = this.StateNode = typeof(T).ToMetaNode(nodeType:MetaNodeTypes.Binary).Configure(configure.Build());

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

        IMetaNode transition = typeof(T).ToMetaNode();

        if(this.StateNode!.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge();

        // todo: what about next?...
        if(this.StateNode.NodeEdge.True is null) this.StateNode.NodeEdge.True = transition;

        // IMetaNode? @ref = this.StateNode.NodeType switch
        // {
            // MetaNodeTypes.Binary => this.StateNode.NodeEdge.True,
            // _ => this.StateNode.NodeEdge.Next
        // };

        // return this.Transition(@ref, transition, configure, then);
        return this.Transition(transition, configure, then);
    }

    public IStateManager Else<T>(
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    )
    {
        IMetaNode transition = typeof(T).ToMetaNode();

        if(this.StateNode!.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge();

        // IMetaNode? @ref = this.StateNode.NodeEdge.False;
        this.StateNode.NodeEdge.False = transition;

        // return this.Transition(@ref, transition, configure, then);
        return this.Transition(transition, configure, then);
    }

    public IStateManager Key<T>(
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    )
    {
        IMetaNode transition = typeof(T).ToMetaNode(configure, MetaNodeTypes.Hash);

        if(this.StateNode!.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge { Nexts = new() };

        return this.Transition(transition, configure, then);
    }

    public IStateManager Hash<T, U>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<IStateManager>? thenT = null,
        Action<IStateManager>? thenU = null
    )
    {
        IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
        IMetaNode transitionU = typeof(U).ToMetaNode(configureU);

        if(thenT is not null) transitionT.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenT) };
        if(thenU is not null) transitionU.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenU) };

        this.StateNode!.NodeEdge!.Nexts!.Add(transitionT);
        this.StateNode!.NodeEdge!.Nexts!.Add(transitionU);

        return this;
    }

    public IStateManager Branch<T, U>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<IStateManager>? thenT = null,
        Action<IStateManager>? thenU = null
    )
    {
        if(this.StateNode!.NodeEdge is null) this.StateNode!.NodeEdge = new MetaNodeEdge { Nexts = new() };

        this.StateNode!.NodeType = MetaNodeTypes.Branch;

        IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
        IMetaNode transitionU = typeof(U).ToMetaNode(configureU);

        if(thenT is not null) transitionT.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenT) };
        if(thenU is not null) transitionU.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenU) };

        this.StateNode!.NodeEdge!.Nexts!.Add(transitionT);
        this.StateNode!.NodeEdge!.Nexts!.Add(transitionU);

        return this;
    }

    public IStateManager Branch<T, U, V>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<INodeConfigurationBuilder>? configureV = null,
        Action<IStateManager>? thenT = null,
        Action<IStateManager>? thenU = null,
        Action<IStateManager>? thenV = null
    )
    {
        if(this.StateNode!.NodeEdge is null) this.StateNode!.NodeEdge = new MetaNodeEdge { Nexts = new() };

        this.StateNode!.NodeType = MetaNodeTypes.Branch;

        IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
        IMetaNode transitionU = typeof(U).ToMetaNode(configureU);
        IMetaNode transitionV = typeof(V).ToMetaNode(configureV);

        if(thenT is not null) transitionT.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenT) };
        if(thenU is not null) transitionU.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenU) };
        if(thenV is not null) transitionV.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenV) };

        this.StateNode!.NodeEdge!.Nexts!.Add(transitionT);
        this.StateNode!.NodeEdge!.Nexts!.Add(transitionU);
        this.StateNode!.NodeEdge!.Nexts!.Add(transitionV);

        return this;
    }

    // todo: remaining thens...
    public IStateManager Hash<T, U, V>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<INodeConfigurationBuilder>? configureV = null,
        Action<IStateManager>? thenT = null
    )
    {
        IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
        IMetaNode transitionU = typeof(U).ToMetaNode(configureU);
        IMetaNode transitionV = typeof(V).ToMetaNode(configureV);

        this.StateNode!.NodeEdge!.Nexts!.Add(transitionT);
        this.StateNode!.NodeEdge!.Nexts!.Add(transitionU);
        this.StateNode!.NodeEdge!.Nexts!.Add(transitionV);

        return this;
    }

    public IStateManager Path<T, U, V>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<INodeConfigurationBuilder>? configureV = null
    )
    {
        if(this.StateNode!.NodeEdge is null) this.StateNode!.NodeEdge = new MetaNodeEdge();

        IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
        IMetaNode transitionU = typeof(U).ToMetaNode(configureU);
        IMetaNode transitionV = typeof(V).ToMetaNode(configureV);

        transitionT.NodeEdge = new MetaNodeEdge { Next = transitionU };
        transitionU.NodeEdge = new MetaNodeEdge { Next = transitionV };

        this.StateNode!.NodeEdge!.Next = transitionT;

        this.StateNode = transitionV;

        return this;
    }

    public INode Build() => this._metaNodeMapper.Map(this.RootNode!);

    private IStateManager Transition(
        // IMetaNode? @ref,
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
            var root = this.NestedThen(then);

            transition.NodeEdge = new MetaNodeEdge { Next = root };

            // todo: should this.StateNode not be set to the lower levels root?
            // IMetaNode state = manager.StateNode;
            // this.StateNode = state;
        }

        // if(@ref is not null) @ref = this.StateNode = transition;
        this.StateNode = transition;

        return this;
    }

    private IMetaNode NestedThen(
        Action<IStateManager>? then
    )
    {
        if(then is null) throw new InvalidOperationException();

        // NEW LEVEL
        IStateManager manager = new StateManager(this._metaNodeMapper);
        then(manager);
        IMetaNode root = manager.RootNode!;

        return root;
    }

    public StateManager(IMetaNodeMapper metaNodeMapper) 
        => this._metaNodeMapper = metaNodeMapper ?? throw new ArgumentNullException(nameof(metaNodeMapper));
}