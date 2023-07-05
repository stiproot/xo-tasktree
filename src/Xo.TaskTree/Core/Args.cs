namespace Xo.TaskTree.Abstractions;

public class Args : IArgs
{
	private IList<IMsg>? _args;

	public IMsg? this[string? key]
	{
		get => this._args?.First(a => a?.ParamName == key);
	}

	public IArgs Init(IList<IMsg> args)
	{
		this._args = args;

		return this;
	}

	public object[] ToObjArray() => this._args!.Select(a => a!.ObjectData).ToArray();

	public object[] ToObjArray(IEnumerable<ParameterInfo> @params) => @params.Select(p => this[p.Name]!.ObjectData).ToArray();

	public string?[] Params() => this._args!.Select(a => a?.ParamName).ToArray();

	public bool Any(Func<IMsg, bool> predicate) => this._args?.Any(predicate!) is true;

	public bool Exists(string arg) => this.Any(a => a.ParamName == arg);

	public IMsg? First() => this._args!.First();

	public int Count() => this._args?.Count() ?? 0;

	public Args() { }
	public Args(IList<IMsg> args) => this._args = args;
}