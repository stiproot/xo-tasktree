namespace Xo.TaskTree.Abstractions;

public interface IArgs
{
	IMsg? this[string? key] { get; }
	object[] ToObjArray();
	object[] ToObjArray(IEnumerable<ParameterInfo> @params);
	string?[] Params();
	IMsg? First();
	int Count();
}