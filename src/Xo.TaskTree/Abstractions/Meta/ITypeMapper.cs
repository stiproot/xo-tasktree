namespace Xo.TaskTree.Abstractions;

public interface ITypeMapper<TSource, TTarget>
{
    TTarget Map(TSource source);
}