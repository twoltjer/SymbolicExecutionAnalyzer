namespace SymbolicExecution.Test.UnitTests;

public class UsingDirectiveSyntaxAbstractionTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_Always_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var subject = new UsingDirectiveSyntaxAbstraction(children, null, location);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(location);
		result.T2Value.Reason.Should().Be("Cannot analyze using directives");
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_Always_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var subject = new UsingDirectiveSyntaxAbstraction(children, null, location);
		var results = subject.GetExpressionResults(state);
		results.IsT1.Should().BeFalse();
		results.T2Value.Location.Should().BeSameAs(location);
		results.T2Value.Reason.Should().Be("Cannot analyze using directives");
	}
}