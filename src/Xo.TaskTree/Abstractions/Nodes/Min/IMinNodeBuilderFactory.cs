namespace Xo.TaskTree.Abstractions;

public interface IMinNodeBuilderFactory
{
	ICoreMinNodeBuilder Create();
	ICoreMinNodeBuilder Create(NodeBuilderTypes nodeType);
	TBuilder Create<TBuilder>(NodeBuilderTypes nodeType);
	ICoreMinNodeBuilder Create(ILogger logger);
	ICoreMinNodeBuilder Create(IWorkflowContext context);
	ICoreMinNodeBuilder Create(string id);
	ICoreMinNodeBuilder Create(
		ILogger logger,
		string id
	);
}
