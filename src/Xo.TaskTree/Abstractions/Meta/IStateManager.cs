namespace Xo.TaskTree.Abstractions;

public interface IRootBranch
{
    IStateManager Root<T>(Action<INodeConfigurationBuilder>? configure = null);
}

public interface IIfBranch
{
    IStateManager If<T>(Action<INodeConfigurationBuilder>? configure = null);
    IStateManager RootIf<T>(Action<INodeConfigurationBuilder>? configure = null);
}

public interface IThenBranch
{
    IStateManager Then<T>(
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    );
}

public interface IKeyBranch
{
    IStateManager Key<T>(
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    );
}

public interface IHashBranch
{
    IStateManager Hash<T, U>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<IStateManager>? then = null
    );

    IStateManager Hash<T, U, V>(
        Action<INodeConfigurationBuilder>? configureT = null,
        Action<INodeConfigurationBuilder>? configureU = null,
        Action<INodeConfigurationBuilder>? configureV = null,
        Action<IStateManager>? then = null
    );
}

public interface IElseBranch
{
    IStateManager Else<T>(
        Action<INodeConfigurationBuilder>? configure = null,
        Action<IStateManager>? then = null
    );
}

public interface IStateManager : 
    IRootBranch, 
    IIfBranch, 
    IThenBranch, 
    IElseBranch, 
    IKeyBranch, 
    IHashBranch
{
    IMetaNode? RootNode { get; set; }
    IMetaNode? StateNode { get; set; }
}

public interface IMetaNode
{
    Type FunctoryType { get; init; }
    MetaNodeTypes NodeType { get; set; }
    IMetaNodeEdge? NodeEdge { get; set; }
    List<IMetaNode> PromisedArgs { get; init; }
    List<IMsg> Args { get; init; }
    INodeConfiguration? NodeConfiguration { get; set; }
}

public class MetaNode : IMetaNode
{
    public Type FunctoryType { get; init; }
    public MetaNodeTypes NodeType { get; set; }
    public IMetaNodeEdge? NodeEdge { get; set; }
    public List<IMetaNode> PromisedArgs { get; init; } = new();
    public List<IMsg> Args { get; init; } = new();
    public INodeConfiguration? NodeConfiguration { get; set; }

    public MetaNode(Type functoryType) => this.FunctoryType = functoryType;
}

public interface IMetaNodeEdge
{
    IMetaNode? True { get; set; }
    IMetaNode? False { get; set; }
    IMetaNode? Next { get; set; }
    List<IMetaNode?>? Nexts { get; set; }
}

public class MetaNodeEdge : IMetaNodeEdge
{
    public IMetaNode? True { get; set; }
    public IMetaNode? False { get; set; }
    public IMetaNode? Next { get; set; }
    public List<IMetaNode?>? Nexts { get; set; }
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