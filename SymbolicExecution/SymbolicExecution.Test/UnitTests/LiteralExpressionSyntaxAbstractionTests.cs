namespace SymbolicExecution.Test.UnitTests;

public class LiteralExpressionSyntaxAbstractionTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_Always_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var subject = new LiteralExpressionSyntaxAbstraction(children, null, location, true, default);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(location);
		result.T2Value.Reason.Should().Be("Cannot analyze literal expressions");
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_WhenBooleanTrue_ReturnsBooleanTrueConstant()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var typeSymbol = Mock.Of<ITypeSymbol>(type => type.Name == "Boolean", MockBehavior.Strict);
		var subject = new LiteralExpressionSyntaxAbstraction(children, null, location, true, typeSymbol);
		var results = subject.GetExpressionResults(state);
		results.IsT1.Should().BeTrue();
		results.T1Value.Length.Should().Be(1);
		var result = results.T1Value.Single();
		result.Item1.Type.Match(x => x.Name, y => y.Name).Should().Be("Boolean");
		result.Item1.Location.Should().BeSameAs(location);
		result.Item1.Value.Should().BeOfType<ConstantValueScope>();
		result.Item1.Value.IsAlways(true).Should().BeTrue();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_WhenConstantValueIsNull_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var typeSymbol = Mock.Of<ITypeSymbol>(type => type.Name == "Boolean", MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new LiteralExpressionSyntaxAbstraction(children, symbol, location, new Optional<object>(), typeSymbol);
		var results = subject.GetExpressionResults(state);
		results.IsT1.Should().BeFalse();
		results.T2Value.Location.Should().BeSameAs(location);
		results.T2Value.Reason.Should().Be("Cannot analyze literal expressions without constant values");
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_WhenActualTypeSymbolIsNull_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var subject = new LiteralExpressionSyntaxAbstraction(children, null, location, true, null);
		var results = subject.GetExpressionResults(state);
		results.IsT1.Should().BeFalse();
		results.T2Value.Location.Should().BeSameAs(location);
		results.T2Value.Reason.Should().Be("Cannot analyze literal expressions without actual type symbols");
	}
}