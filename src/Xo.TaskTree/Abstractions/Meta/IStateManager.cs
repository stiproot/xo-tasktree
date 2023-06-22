namespace Xo.TaskTree.Abstractions;

public interface IStateManager : 
    IRootBranch, 
    IIfBranch, 
    IThenBranch, 
    IElseBranch, 
    IKeyBranch, 
    IHashBranch,
    IBranchBranch
{
    IMetaNode? RootNode { get; set; }
    IMetaNode? StateNode { get; set; }
}