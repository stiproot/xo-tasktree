namespace Xo.TaskTree.Abstractions;

public interface IMinNodeConfiguration
{
	bool RequiresResult { get; set; }
	string? NextParamName { get; set; }
	string? Key { get; set; }
	IWorkflowContext? WorkflowContext { get; set; }

	List<IMsg> Args { get; init; }
	List<IMetaNode> PromisedArgs { get; init; }
	List<Func<IWorkflowContext, IMsg>> ContextArgs { get; init; }

	string Id { get; }
	bool IgnoresPromisedResults { get; }
	bool IsSync { get; }
}