namespace Xo.TaskTree.Unit.Tests;

[ExcludeFromCodeCoverage]
public class MsgFactoryTests
{
	private readonly IMsgFactory _msgFactory;

	public MsgFactoryTests(
		IMsgFactory msgFactory
	)
	{
		this._msgFactory = msgFactory ?? throw new ArgumentNullException(nameof(msgFactory));
	}

	[Fact]
	public void MsgFactory_ProvidedParamNameAndData_ProducesMsg()
	{
		// Act
		var msg = this._msgFactory.Create<object>(new object(), "paramName");

		// Assert
		Assert.NotNull(msg);
	}

	[Fact]
	public void MsgFactory_ProvidedParamNameAndData_ProducesValueTypeMsg()
	{
		// Act
		var msg = this._msgFactory.Create<int>(10, "paramName");

		// Assert
		Assert.NotNull(msg);
	}
}
