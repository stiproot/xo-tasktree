namespace Xo.TaskTree.Abstractions;

public interface IStateManager : 
    IRootBranch, 
    IIfBranch, 
    IHashBranch,
    IBranchBranch,
    IPathBranch
{
    IMetaNode? RootNode { get; set; }
    IMetaNode? StateNode { get; set; }
}