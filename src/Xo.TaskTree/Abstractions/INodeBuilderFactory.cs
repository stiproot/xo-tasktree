namespace Xo.TaskTree.Abstractions;

public interface INodeBuilderFactory
{
	ICoreNodeBuilder Create();
	ICoreNodeBuilder Create(NodeBuilderTypes nodeType);
	TBuilder Create<TBuilder>(NodeBuilderTypes nodeType);
	ICoreNodeBuilder Create(ILogger logger);
	ICoreNodeBuilder Create(IWorkflowContext context);
	ICoreNodeBuilder Create(string id);
	ICoreNodeBuilder Create(
		ILogger logger,
		string id
	);
}
