using System.Reflection;

namespace Xo.TaskTree.Abstractions;

public class Args : IArgs
{
	private IList<IMsg?>? _args;

	public IMsg? this[string? key]
	{
		get => this._args?.First(a => a?.ParamName == key);
	}

	public IArgs Init(IList<IMsg?> args)
	{
		this._args = args;

		return this;
	}

	public object[] ToObjArray() => this._args!.Select(a => a!.ObjectData).ToArray();

	public object[] ToObjArray(IEnumerable<ParameterInfo> @params) => this._args?.Select(a => this[a?.ParamName]).ToArray()!;

	public Args() { }
	public Args(IList<IMsg?> args) => this._args = args;
}