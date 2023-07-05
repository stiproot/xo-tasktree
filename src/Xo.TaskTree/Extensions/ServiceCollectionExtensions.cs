using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Xo.TaskTree.DependencyInjection.Extensions;

/// <summary>
///   <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	///   Add all Xo.TaskTree services to <see cref="IServiceCollection"/>.
	/// </summary>
	/// <returns><see cref="IServiceCollection"/></returns>
	public static IServiceCollection AddTaskTreeServices(this IServiceCollection @this)
	{
		@this.TryAddSingleton<IFnFactory, FnFactory>();
		@this.TryAddSingleton<IMetaNodeMapper, MetaNodeMapper>();
		@this.TryAddSingleton<INodeBuilderFactory, NodeBuilderFactory>();
		@this.TryAddSingleton<IStateManager, StateManager>();
		@this.TryAddSingleton<INodeFactory, NodeFactory>();
		@this.TryAddSingleton<IMsgFactory, MsgFactory>();
		@this.TryAddSingleton<IWorkflowContextFactory, WorkflowContextFactory>();

		return @this;
	}
}
