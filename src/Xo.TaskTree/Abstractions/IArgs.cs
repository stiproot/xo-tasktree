namespace Xo.TaskTree.Abstractions;

public interface IArgs
{
	IMsg? this[string? key] { get; }
	IArgs Init(IList<IMsg> args);
	object[] ToObjArray();
	object[] ToObjArray(IEnumerable<ParameterInfo> @params);
	string?[] Params();
	bool Any(Func<IMsg, bool> predicate);
	bool Exists(string arg);
	IMsg? First();
	int Count();
}