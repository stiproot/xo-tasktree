namespace Xo.TaskTree.Abstractions;

public interface IXCoreNodeBuilder
{
    IXCoreNodeBuilder AddArg(IMsg arg);
}

public interface IXGenericNodeBuilder
{
    IXGenericNodeBuilder MatchArg<TSerice>(IXCoreNodeBuilder coreNodeBuilder);
}

public interface IXBuilderManager
{
    IXBuilderManager AddArg(IMsg arg);
    IXBuilderManager MatchArg<TSerice>();
}

public class XBuilderManager : IXBuilderManager
{
    private readonly IXCoreNodeBuilder _core = null!;
    private readonly IXGenericNodeBuilder _generic = null!;

    public IXBuilderManager AddArg(IMsg arg)
    {
        this._core.AddArg(arg);
        return this;
    }

    public IXBuilderManager MatchArg<TSerice>()
    { 
        this._generic.MatchArg<TSerice>(this._core);
        return this;
    }
}