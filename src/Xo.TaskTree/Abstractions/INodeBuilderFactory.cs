namespace Xo.TaskTree.Abstractions;

public interface INodeBuilderFactory
{
    INodeBuilder Create(ILogger? logger = null);
}
