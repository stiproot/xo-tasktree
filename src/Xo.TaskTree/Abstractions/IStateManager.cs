namespace Xo.TaskTree.Abstractions;

public interface IIfBranch
{
    IStateManager If<T>(Action<INodeConfigurationBuilder>? configure = null);
    IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null);
}

public interface IThenBranch
{
    IStateManager Then<T>(
        Action<IStateManager>? then = null,
        Action<INodeConfigurationBuilder>? configure = null
    );
}

public interface IElseBranch
{
    IStateManager Else<T>(Action<INodeConfigurationBuilder>? configure = null);
}

public interface IStateManager : IIfBranch, IThenBranch, IElseBranch
{
    IMetaNode Root { get; set; }
    IMetaNode State { get; set; }
}

public class StateManager : IStateManager 
{
    public IMetaNode Root { get; set; }
    public IMetaNode State { get; set; }

    public IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        // todo: double check pointers...
        this.Root = this.State = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Binary, NodeEdge = new MetaNodeEdge() };

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
        if(this.State.NodeType is MetaNodeTypes.Binary)
        {
            this.State.NodeEdge.True = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default }; // we do not presume to define a node-edge...

            /* PROCESS CONFIGURATION */ 
            if(configure is not null)
            {
                var configBuilder = new NodeConfigurationBuilder();
                configure(configBuilder);
                INodeConfiguration config = configBuilder.Build();

                this.State.NodeEdge.True.NodeConfiguration = config;

                // todo: should this not be removed?... as it is in config...
                this.State.NodeEdge.True.PromisedArgs.AddRange(config.PromisedArgs);
                this.State.NodeEdge.True.Args.AddRange(config.Args);
            }

            /* PROCESS THEN */
            if(then is not null)
            {
                // NEW LEVEL
                IStateManager manager = new StateManager();

                then(manager);

                IMetaNode root = manager.Root;

                this.State.NodeEdge.True.NodeEdge = new MetaNodeEdge { Next = root };
            }
        }
        else
        {
            // todo: update State to this then?
        }

        return this;
    }

    public IStateManager Else<T>(Action<INodeConfigurationBuilder>? configure = null)
    {
        if(this.State.NodeType is MetaNodeTypes.Binary)
        {
            this.State.NodeEdge.False = new MetaNode{ FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default }; // we do not presume to define a node-edge...

            // todo: process configuration...
        }
        else
        {
            // todo: update State to this else?
        }

        return this;
    }
}



public interface IMetaNode
{
    Type FunctoryType { get; init; }
    MetaNodeTypes NodeType { get; set; }
    IMetaNodeEdge NodeEdge { get; set; }
    List<IMetaNode> PromisedArgs { get; init; }
    List<IMsg> Args { get; init; }
    INodeConfiguration? NodeConfiguration { get; set; }
}

public class MetaNode : IMetaNode
{
    public Type FunctoryType { get; init; }
    public MetaNodeTypes NodeType { get; set; }
    public IMetaNodeEdge NodeEdge { get; set; }
    public List<IMetaNode> PromisedArgs { get; init; } = new();
    public List<IMsg> Args { get; init; } = new();
    public INodeConfiguration? NodeConfiguration { get; set; }
}

public interface IMetaNodeEdge
{
    IMetaNode? True { get; set; }
    IMetaNode? False { get; set; }
    IMetaNode? Next { get; set; }
    IMetaNode?[] Nexts { get; set; }
}

public class MetaNodeEdge : IMetaNodeEdge
{
    public IMetaNode? True { get; set; }
    public IMetaNode? False { get; set; }
    public IMetaNode? Next { get; set; }
    public IMetaNode?[] Nexts { get; set; }
}

public enum MetaNodeTypes
{
	Default = 0,
	Linked = 1,
	Pool = 2,
	Binary = 3,
	Hash = 4,
	PromisedArgMatch = 5,
}