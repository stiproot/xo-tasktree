namespace Xo.TaskTree.Unit.Tests;

public class NodeConfigurationBuilderTests
{
	[Fact]
	public void AddArg_MatchArg()
	{
		// Arrange
		var builder = new NodeConfigurationBuilder(typeof(IY_InBoolStr_OutConstInt_AsyncService));
		Action<INodeConfigurationBuilder> addArgWithParamName = b => b.AddArg(true, "flag");
		Action<INodeConfigurationBuilder> matchArg = b => b.MatchArg("<<args>>");

		// Act
		addArgWithParamName(builder);
		matchArg(builder);

		// Assert
		var config = builder.Build();

		Assert.True(config.Args.Exists(a => a.ParamName == "flag" && (a as Msg<bool>)!.GetData() is true));
		Assert.True(config.Args.Exists(a => a.ParamName == "args" && (a as Msg<string>)!.GetData() == "<<args>>"));
	}

	[Fact]
	public void MatchArg_MatchArg()
	{
		// Arrange
		var builder = new NodeConfigurationBuilder(typeof(IY_InBoolStr_OutConstInt_AsyncService));
		Action<INodeConfigurationBuilder> matchArg = b => b.AddArg(true);
		Action<INodeConfigurationBuilder> matchArg1 = b => b.MatchArg("<<args>>");

		// Act
		matchArg(builder);
		matchArg1(builder);

		// Assert
		var config = builder.Build();

		Assert.True(config.Args.Exists(a => a.ParamName == "flag" && (a as Msg<bool>)!.GetData() is true));
		Assert.True(config.Args.Exists(a => a.ParamName == "args" && (a as Msg<string>)!.GetData() == "<<args>>"));
	}

	[Fact]
	public void MatchArgs_MatchArgs_MatchArgs()
	{
		// Arrange
		var builder = new NodeConfigurationBuilder(typeof(IY_InStrStrStr_OutConstInt_AsyncService));
		Action<INodeConfigurationBuilder> matchArg0 = b => b.MatchArgs("<<args-0>>");
		Action<INodeConfigurationBuilder> matchArg1 = b => b.MatchArgs("<<args-1>>");
		Action<INodeConfigurationBuilder> matchArg2 = b => b.MatchArgs("<<args-2>>");

		// Act
		matchArg0(builder);
		matchArg1(builder);
		matchArg2(builder);

		// Assert
		var config = builder.Build();

		Assert.True(config.Args.Exists(a => a.ParamName == "arg1" && (a as Msg<string>)!.GetData() == "<<args-0>>"));
		Assert.True(config.Args.Exists(a => a.ParamName == "arg2" && (a as Msg<string>)!.GetData() == "<<args-1>>"));
		Assert.True(config.Args.Exists(a => a.ParamName == "arg3" && (a as Msg<string>)!.GetData() == "<<args-2>>"));
	}
}