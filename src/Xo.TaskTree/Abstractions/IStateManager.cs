namespace Xo.TaskTree.Abstractions;

public interface IIfBranch
{
    IStateManager If<T>(Action<INodeConfigurationBuilder>? config = null);
    IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? config = null);
}

public interface IThenBranch
{
    IStateManager Then<T>(
        Action<IFlowBuilder> then,
        Action<INodeConfigurationBuilder>? config = null
    );
}

public interface IElseBranch
{
    IStateManager Else<T>(Action<INodeConfigurationBuilder>? config = null);
}

public interface IStateManager : IIfBranch, IThenBranch, IElseBranch
{
    IMetaNode State { get; set; }
}

public class StateManager : IStateManager 
{
    public IMetaNode State { get; set; }

    public IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? config = null)
    {
        this.State = new MetaNode { FunctoryType = typeof(T), NodeType = MetaNodeTypes.Binary, NodeEdge = new MetaNodeEdge() };

        // todo: process configuration...

        return this;
    }

    public IStateManager If<T>(Action<INodeConfigurationBuilder>? config = null)
    {
        throw new NotImplementedException();
    }

    public IStateManager Then<T>(
        Action<IFlowBuilder> then,
        Action<INodeConfigurationBuilder>? config = null
    )
    {
        if(this.State.NodeType is MetaNodeTypes.Binary)
        {
            this.State.NodeEdge.True = new MetaNode{ FunctoryType = typeof(T), NodeType = MetaNodeTypes.Default }; // we do not presume to define a node-edge...

            // todo: process configuration...
        }
        else
        {
            // todo: update State to this then?
        }

        return this;
    }

    public IStateManager Else<T>(Action<INodeConfigurationBuilder>? config = null)
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
    IMetaNode?[] ArgNodes { get; set; }
}

public class MetaNode : IMetaNode
{
    public Type FunctoryType { get; init; }
    public MetaNodeTypes NodeType { get; set; }
    public IMetaNodeEdge NodeEdge { get; set; }
    public IMetaNode?[] ArgNodes { get; set; }
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
	Hash = 4
}