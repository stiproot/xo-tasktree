namespace Xo.TaskTree.Abstractions;

public interface INodeConfiguration
{
	bool RequiresResult { get; set; }
	string? NextParamName { get; set; }
	string? Key { get; set; }
	IWorkflowContext? WorkflowContext { get; set; }

	List<IMsg> Args { get; init; }
	List<INode> PromisedArgs { get; init; }
	List<IMetaNode> MetaPromisedArgs { get; init; }
	List<Func<IWorkflowContext, IMsg>> ContextArgs { get; init; }

	string Id { get; set; }
	bool IgnoresPromisedResults { get; set; }
}