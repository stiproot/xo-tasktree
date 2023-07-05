namespace Xo.TaskTree.Abstractions;

/// <summary>
///   Factory for producing instances of <see cref="INode"/> implementations.
/// </summary>
public interface INodeFactory
{
	INode Create(
		ILogger? logger = null,
		INodeConfiguration? nodeConfiguration = null
	);
}
