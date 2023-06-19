namespace Xo.TaskTree.Abstractions;

public interface IHashBranchBuilder : INodeBuilder
{
	IHashBranchBuilder AddNext<T>(string key);
	IHashBranchBuilder AddNext(string key, INode node);
}