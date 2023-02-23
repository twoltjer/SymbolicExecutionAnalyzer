namespace SymbolicExecution.Test.UnitTests;

public class EqualsValueClauseSyntaxAbstractionTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_Always_ReturnsFailure()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new EqualsValueClauseSyntaxAbstraction(
			ImmutableArray<ISyntaxNodeAbstraction>.Empty,
			Mock.Of<ISymbol>(MockBehavior.Strict),
			location
			);
		var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Cannot analyze an equals value clause");
		result.T2Value.Location.Should().BeSameAs(location);
	}
}