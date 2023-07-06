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
		// ACT...
		var msg = this._msgFactory.Create<object>(new object(), "paramName");

		// ASSERT...
		Assert.NotNull(msg);
	}

	[Fact]
	public void MsgFactory_ProvidedParamNameAndData_ProducesValueTypeMsg()
	{
		// ACT...
		var msg = this._msgFactory.Create<int>(10, "paramName");

		// ASSERT...
		Assert.NotNull(msg);
	}
}
