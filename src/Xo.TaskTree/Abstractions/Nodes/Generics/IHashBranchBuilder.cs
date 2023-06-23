namespace Xo.TaskTree.Abstractions;

public interface IHashBranchBuilder : IBranchBuilder
{
	IHashBranchBuilder AddNext<T>(string key);
	IHashBranchBuilder AddNext(string key, INode node);
}