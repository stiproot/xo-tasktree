namespace Xo.TaskTree.Abstractions;

public interface IHashBranchNodeBuilder : INodeBuilder
{
	IHashBranchNodeBuilder AddNext<T>(string key);
	IHashBranchNodeBuilder AddNext(string key, INode node);
}