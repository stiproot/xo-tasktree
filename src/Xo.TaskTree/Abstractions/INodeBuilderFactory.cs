namespace Xo.TaskTree.Abstractions;

public interface INodeBuilderFactory
{
    ICoreNodeBuilder Create(ILogger? logger = null);
}
