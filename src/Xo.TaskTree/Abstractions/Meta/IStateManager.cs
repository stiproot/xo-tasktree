namespace Xo.TaskTree.Abstractions;

public interface IStateManager : 
    IRootBranch, 
    IIfBranch, 
    IThenBranch, 
    IElseBranch, 
    IKeyBranch, 
    IHashBranch,
    IBranchBranch,
    IPathBranch
{
    IMetaNode? RootNode { get; set; }
    IMetaNode? StateNode { get; set; }
}