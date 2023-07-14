namespace Xo.TaskTree.Abstractions;

/// <summary>
///   The DTO used to transmit data within a workflow. 
/// </summary>
public interface IMsg
{
	IMsg? _Previous { get; init; }

	/// <summary>
	///   The name of the parameter that the data contained in this msg needs to be passed in as an argument.
	/// </summary>
	string? ParamName { get; set; }

	/// <summary>
	///   Sets <see cref="Msg.ParamName"/>. Call can be chained.
	/// </summary>
	IMsg SetParam(string paramName);

	/// <summary>
	///   Is the data contained in this msg of value-type? 
	/// </summary>
	/// <remarks>
	///   Returns false if the data is a type. 
	/// </remarks>
	bool IsValueType { get; }

	/// <summary>
	///   The type for the data contained in this msg.
	/// </summary>
	Type GetDataType();

	/// <summary>
	///   Is <see cref="IMsg.ParamName" /> set?
	/// </summary>
	bool HasParam { get; }

	/// <summary>
	///   Is there data? 
	/// </summary>
	bool HasData { get; }

	/// <summary>
	///   Boxes the data contained in this msg as object as returns. 
	/// </summary>
	/// <remarks>
	///   This is used by the <see cref="IFnFactory" />, where reflection is used to dynamically create a fn factory from a <see cref="System.Type" /> that is provided.
	/// </remarks>
	object ObjectData { get; }
}
