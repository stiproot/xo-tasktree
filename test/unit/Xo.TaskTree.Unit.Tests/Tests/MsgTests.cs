namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class MsgTests
{
	private readonly IMsgFactory _msgFactory;

	public MsgTests(IMsgFactory msgFactory)
		=> this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));

	[Fact]
	public void Msg_ProvidedType_CastsData()
	{
		// Arrange
		const string paramName = "now";
		var data = DateTime.UtcNow;
		IMsg msg = this._msgFactory.Create<DateTime>(data, paramName);

		// Act
		var castedData = ((Msg<DateTime>)msg).GetData();

		// Assert
		Assert.Equal(data, castedData);
		Assert.Equal(paramName, msg.ParamName);
	}
}
