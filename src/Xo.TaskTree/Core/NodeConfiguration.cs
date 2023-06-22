namespace Xo.TaskTree.Core;

public class NodeConfiguration : INodeConfiguration
{
    public bool RequiresResult { get; set; }
    public string? NextParamName { get; set; }
    public string? Key { get; set; }
    public List<IMsg> Args { get; init; } = new();
    public List<IMetaNode> PromisedArgs { get; init; } = new(); 
}