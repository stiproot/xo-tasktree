namespace Xo.TaskTree.Core;

public class StateManager : IStateManager
{
	private readonly IMetaNodeMapper _metaNodeMapper;
	public IMetaNode? RootNode { get; set; }
	public IMetaNode? StateNode { get; set; }

	public IStateManager Root<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		this.RootNode = this.StateNode = typeof(T).ToMetaNode(configure);

		return this;
	}

	public IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		var enriched = configure.Enrich(c => c.ControllerType(ControllerTypes.True));

		this.RootNode = this.StateNode = typeof(T).ToMetaNode(enriched, nodeType: MetaNodeTypes.Binary);

		return this;
	}

	public IStateManager IsNotNull<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		var enriched = configure.Enrich(c => c.ControllerType(ControllerTypes.IsNotNull));

		this.RootNode = this.StateNode = typeof(T).ToMetaNode(enriched, nodeType: MetaNodeTypes.Binary);

		return this;
	}

	public IStateManager If<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		var enriched = configure.Enrich(c => c.ControllerType(ControllerTypes.True));

		IMetaNode transition = typeof(T).ToMetaNode(enriched, MetaNodeTypes.Binary);

		if (this.RootNode is null)
		{
			this.RootNode = this.StateNode = transition;
			return this;
		}

		if (this.StateNode!.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge();

		this.StateNode = this.StateNode.NodeEdge.Next = transition;

		return this;
	}

	public IStateManager Then<T>(
			Action<INodeConfigurationBuilder>? configure = null,
			Action<IStateManager>? then = null
	)
	{
		IMetaNode transition = typeof(T).ToMetaNode(configure);

		IMetaNode? levelTransition = this.NestedThen(then);

		if (levelTransition is not null)
		{
			transition.NodeEdge = new MetaNodeEdge { Next = levelTransition };
		}

		if (this.RootNode is null)
		{
			this.RootNode = this.StateNode = transition;
			return this;
		}

		if (this.StateNode!.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge();

		if (this.StateNode.NodeType is MetaNodeTypes.Binary)
		{
			transition.NodeConfiguration.ControllerType = this.StateNode.NodeConfiguration.ControllerType;
			this.StateNode.NodeEdge.True = transition;
			return this;
		}

		this.StateNode = this.StateNode.NodeEdge.Next = transition;

		return this;
	}

	public IStateManager Else<T>(
			Action<INodeConfigurationBuilder>? configure = null,
			Action<IStateManager>? then = null
	)
	{
		IMetaNode transition = typeof(T).ToMetaNode(configure);

		IMetaNode? levelTransition = this.NestedThen(then);

		if (levelTransition is not null) transition.NodeEdge = new MetaNodeEdge { Next = levelTransition };

		if (this.StateNode!.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge();

		// Not sure what else this could be? But anyway...
		if (this.StateNode!.NodeType is MetaNodeTypes.Binary)
		{
			transition.NodeConfiguration.ControllerType = this.StateNode.NodeConfiguration.ControllerType;
			this.StateNode!.NodeEdge!.False = transition;
		}

		return this;
	}

	public IStateManager Key<T>(Action<INodeConfigurationBuilder>? configure = null)
	{
		IMetaNode transition = typeof(T).ToMetaNode(configure, MetaNodeTypes.Hash);

		transition.NodeEdge = new MetaNodeEdge { Nexts = new() };

		if (this.StateNode!.NodeEdge is null) this.StateNode.NodeEdge = new MetaNodeEdge { Next = transition };

		this.StateNode = this.StateNode.NodeEdge.Next;

		return this;
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

		IMetaNode? levelTransitionT = this.NestedThen(thenT);
		IMetaNode? levelTransitionU = this.NestedThen(thenU);

		if (levelTransitionT is not null)
		{
			transitionT.NodeEdge = new MetaNodeEdge { Next = levelTransitionT };
		}
		if (levelTransitionU is not null)
		{
			transitionU.NodeEdge = new MetaNodeEdge { Next = levelTransitionU };
		}

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
		if (this.StateNode!.NodeEdge is null) this.StateNode!.NodeEdge = new MetaNodeEdge { Nexts = new() };

		this.StateNode!.NodeType = MetaNodeTypes.Branch;

		IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
		IMetaNode transitionU = typeof(U).ToMetaNode(configureU);

		if (thenT is not null) transitionT.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenT) };
		if (thenU is not null) transitionU.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenU) };

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
		if (this.StateNode!.NodeEdge is null) this.StateNode!.NodeEdge = new MetaNodeEdge { Nexts = new() };

		this.StateNode!.NodeType = MetaNodeTypes.Branch;

		IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
		IMetaNode transitionU = typeof(U).ToMetaNode(configureU);
		IMetaNode transitionV = typeof(V).ToMetaNode(configureV);

		if (thenT is not null) transitionT.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenT) };
		if (thenU is not null) transitionU.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenU) };
		if (thenV is not null) transitionV.NodeEdge = new MetaNodeEdge { Next = this.NestedThen(thenV) };

		this.StateNode!.NodeEdge!.Nexts!.Add(transitionT);
		this.StateNode!.NodeEdge!.Nexts!.Add(transitionU);
		this.StateNode!.NodeEdge!.Nexts!.Add(transitionV);

		return this;
	}

	public IStateManager Hash<T, U, V>(
			Action<INodeConfigurationBuilder>? configureT = null,
			Action<INodeConfigurationBuilder>? configureU = null,
			Action<INodeConfigurationBuilder>? configureV = null,
			Action<IStateManager>? thenT = null,
			Action<IStateManager>? thenU = null,
			Action<IStateManager>? thenV = null
	)
	{
		IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
		IMetaNode transitionU = typeof(U).ToMetaNode(configureU);
		IMetaNode transitionV = typeof(V).ToMetaNode(configureV);

		IMetaNode? levelTransitionT = this.NestedThen(thenT);
		IMetaNode? levelTransitionU = this.NestedThen(thenU);
		IMetaNode? levelTransitionV = this.NestedThen(thenV);

		if (levelTransitionT is not null)
		{
			transitionT.NodeEdge = new MetaNodeEdge { Next = levelTransitionT };
		}
		if (levelTransitionU is not null)
		{
			transitionU.NodeEdge = new MetaNodeEdge { Next = levelTransitionU };
		}
		if (levelTransitionV is not null)
		{
			transitionV.NodeEdge = new MetaNodeEdge { Next = levelTransitionV };
		}

		this.StateNode!.NodeEdge!.Nexts!.Add(transitionT);
		this.StateNode!.NodeEdge!.Nexts!.Add(transitionU);
		this.StateNode!.NodeEdge!.Nexts!.Add(transitionV);

		return this;
	}

	public IStateManager Path<T, U>(
		Action<INodeConfigurationBuilder>? configureT = null,
		Action<INodeConfigurationBuilder>? configureU = null
	)
	{
		if (this.StateNode!.NodeEdge is null) this.StateNode!.NodeEdge = new MetaNodeEdge();

		IMetaNode transitionT = typeof(T).ToMetaNode(configureT);
		IMetaNode transitionU = typeof(U).ToMetaNode(configureU);

		transitionT.NodeEdge = new MetaNodeEdge { Next = transitionU };

		this.StateNode!.NodeEdge!.Next = transitionT;

		this.StateNode = transitionU;

		return this;
	}

	public IStateManager Path<T, U, V>(
			Action<INodeConfigurationBuilder>? configureT = null,
			Action<INodeConfigurationBuilder>? configureU = null,
			Action<INodeConfigurationBuilder>? configureV = null
	)
	{
		if (this.StateNode!.NodeEdge is null) this.StateNode!.NodeEdge = new MetaNodeEdge();

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

	private IMetaNode? NestedThen(
			Action<IStateManager>? then
	)
	{
		if (then is null) return null;

		// NEW LEVEL
		IStateManager manager = new StateManager(this._metaNodeMapper);
		then(manager);
		IMetaNode root = manager.RootNode!;

		return root;
	}

	public StateManager(IMetaNodeMapper metaNodeMapper)
			=> this._metaNodeMapper = metaNodeMapper ?? throw new ArgumentNullException(nameof(metaNodeMapper));
}