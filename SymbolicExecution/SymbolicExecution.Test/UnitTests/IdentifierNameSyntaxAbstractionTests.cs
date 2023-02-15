namespace SymbolicExecution.Test.UnitTests;

public class IdentifierNameSyntaxAbstractionTests
{
	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(false, false)]
	[InlineData(false, true)]
	[InlineData(true, false)]
	[InlineData(true, true)]
	public void TestAnalyzeNode(bool hasChildren, bool hasSymbol)
	{
		var children = hasChildren
			? new[] { Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict) }
			: Array.Empty<IdentifierNameSyntaxAbstraction>();
		var symbol = hasSymbol
			? Mock.Of<ISymbol>(MockBehavior.Strict)
			: null;
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new IdentifierNameSyntaxAbstraction(children.ToImmutableArray(), symbol, location, default, default);
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeTrue();
		result.T1Value.Single().Should().BeSameAs(previous);
	}
}