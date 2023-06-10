namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Factory for producing instances of <see cref="IWorkflowContext"/> implementations.
/// </summary>
public interface IWorkflowContextFactory
{
  /// <summary>
  ///   Creates an instance of an <see cref="IWorkflowContext"/> implementation.
  /// </summary>
  /// <returns><see cref="IWorkflowContext"/></returns>
  IWorkflowContext Create();
}
