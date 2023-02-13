namespace SymbolicExecution.Test.UnitTests;

public class VariableDeclaratorSyntaxAbstractionTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_Always_ReturnsFailure()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclaratorSyntaxAbstraction(
			ImmutableArray<ISyntaxNodeAbstraction>.Empty,
			Mock.Of<ISymbol>(MockBehavior.Strict),
			location
			);
		var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Cannot analyze a variable declarator");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResults_Always_ReturnsAnalysisFailure()
	{
		var nodeLocation = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclaratorSyntaxAbstraction(
			ImmutableArray<ISyntaxNodeAbstraction>.Empty,
			Mock.Of<ISymbol>(MockBehavior.Strict),
			nodeLocation
			);
		var result = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Cannot get the expression results of a variable declarator");
		result.T2Value.Location.Should().BeSameAs(nodeLocation);
	}
}