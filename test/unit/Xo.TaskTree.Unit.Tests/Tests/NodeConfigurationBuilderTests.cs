namespace Xo.TaskTree.Unit.Tests;

public class NodeConfigurationBuilderTests
{
	[Fact]
	public void AddArg_MatchArg()
	{
		// ARRANGE...
		var builder = new NodeConfigurationBuilder(typeof(ISvc_InBoolStr_OutConstInt_AsyncService));
		Action<INodeConfigurationBuilder> addArgWithParamName = b => b.AddArg(true, "flag");
		Action<INodeConfigurationBuilder> matchArg = b => b.MatchArg("<<args>>");

		// ACT...
		addArgWithParamName(builder);
		matchArg(builder);

		// ASSERT......
		var config = builder.Configuration();

		Assert.True(config.Args.Matches<bool>("flag", true));
		Assert.True(config.Args.Matches<string>("args", "<<args>>"));
	}

	[Fact]
	public void MatchArg_MatchArg()
	{
		// ARRANGE...
		var builder = new NodeConfigurationBuilder(typeof(ISvc_InBoolStr_OutConstInt_AsyncService));
		Action<INodeConfigurationBuilder> matchArg = b => b.AddArg(true);
		Action<INodeConfigurationBuilder> matchArg1 = b => b.MatchArg("<<args>>");

		// ACT...
		matchArg(builder);
		matchArg1(builder);

		// ASSERT...
		var config = builder.Configuration();

		Assert.True(config.Args.Matches<bool>("flag", true));
		Assert.True(config.Args.Matches<string>("args", "<<args>>"));
	}

	[Fact]
	public void MatchArgs_MatchArgs_MatchArgs()
	{
		// ARRANGE...
		var builder = new NodeConfigurationBuilder(typeof(ISvc_InStrStrStr_OutConstInt_AsyncService));
		Action<INodeConfigurationBuilder> matchArg1 = b => b.MatchArgs("<<args-1>>");
		Action<INodeConfigurationBuilder> matchArg2 = b => b.MatchArgs("<<args-2>>");
		Action<INodeConfigurationBuilder> matchArg3 = b => b.MatchArgs("<<args-3>>");

		// ACT...
		matchArg1(builder);
		matchArg2(builder);
		matchArg3(builder);

		// ASSERT...
		var config = builder.Configuration();

		Assert.True(config.Args.Matches("arg1", "<<args-1>>"));
		Assert.True(config.Args.Matches("arg2", "<<args-2>>"));
		Assert.True(config.Args.Matches("arg3", "<<args-3>>"));
	}
}