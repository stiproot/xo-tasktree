using System.Reflection;

namespace Xo.TaskTree.Core;

/// <summary>
///   <see cref="Type"/> utility. 
/// </summary>
public static class TypeInspector
{
	private readonly static Type TaskType = typeof(Task);

	/// <summary>
	///   Determines if a method returns a Task or Task{T}. 
	/// </summary>
	/// <param name="methodInfo"><see cref="MethodInfo"/></param>
	/// <returns><see cref="bool"/></returns>
	public static bool MethodHasReturnTypeOfTask(MethodInfo methodInfo)
	{
		var returnParameterType = methodInfo.ReturnParameter.ParameterType;
		return returnParameterType == TaskType || returnParameterType.BaseType == TaskType;
	}
}
