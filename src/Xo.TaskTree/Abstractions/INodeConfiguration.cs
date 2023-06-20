namespace Xo.TaskTree.Abstractions;

public interface INodeConfiguration
{
    bool RequiresResult { get; set; }
    string? NextParamName{ get; set; }
    List<IMsg> Args { get; init; }
    List<IMetaNode> PromisedArgs { get; init; }
}