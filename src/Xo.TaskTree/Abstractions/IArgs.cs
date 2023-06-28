using System.Reflection;

namespace Xo.TaskTree.Abstractions;

public interface IArgs
{
	IMsg? this[string? key] { get; }
	IArgs Init(IList<IMsg?> args);
	object[] ToObjArray();
	object[] ToObjArray(IEnumerable<ParameterInfo> @params);
}