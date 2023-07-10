namespace Xo.TaskTree.Core;

public class NodeConfiguration : INodeConfiguration
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public bool IgnoresPromisedResults { get; set; }
	public bool RequiresResult { get; set; }
	public string? NextParamName { get; set; }
	public string? Key { get; set; }
	public IWorkflowContext? WorkflowContext { get; set; }
	public List<IMsg> Args { get; init; } = new();
	public List<INode> PromisedArgs { get; init; } = new();
	public List<IMetaNode> MetaPromisedArgs { get; init; } = new();
	public List<Func<IWorkflowContext, IMsg>> ContextArgs { get; init; } = new();
	public ControllerTypes ControllerType { get; set; } = ControllerTypes.None;
}