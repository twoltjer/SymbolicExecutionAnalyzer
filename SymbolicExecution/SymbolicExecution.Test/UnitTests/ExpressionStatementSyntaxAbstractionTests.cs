namespace SymbolicExecution.Test.UnitTests;

public class ExpressionStatementSyntaxAbstractionTests
{
	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(0)]
	[InlineData(2)]
	public void TestAnalyzeNode_WhenNotExactlyOneChild_ReturnsFailure(int childCount)
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = Enumerable.Range(0, childCount)
			.Select(_ => Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict))
			.ToImmutableArray();
		var subject = new ExpressionStatementSyntaxAbstraction(
			children,
			Mock.Of<ISymbol>(MockBehavior.Strict),
			location
			);
		var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Expression statement must have exactly one child");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithNonAssignmentExpressionSyntaxChild_ReturnsFailure()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var child = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var children = new[] { child }.ToImmutableArray();
		var subject = new ExpressionStatementSyntaxAbstraction(
			children,
			Mock.Of<ISymbol>(MockBehavior.Strict),
			location
			);
		var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Expression statement must have an assignment expression or invocation as its child");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithAssignmentExpressionSyntaxChild_AnalyzesChild()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var child = Mock.Of<IAssignmentExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var children = new ISyntaxNodeAbstraction[] { child }.ToImmutableArray();
		var subject = new ExpressionStatementSyntaxAbstraction(
			children,
			Mock.Of<ISymbol>(MockBehavior.Strict),
			location
			);
		var previousState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		Mock.Get(child)
			.Setup(x => x.AnalyzeNode(previousState))
			.Returns(new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(new[] { modifiedState }));
		var result = subject.AnalyzeNode(previousState);
		result.IsT1.Should().BeTrue();
		result.T1Value.Single().Should().BeSameAs(modifiedState);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResults_Always_ReturnsAnalysisFailure()
	{
		var nodeLocation = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new ExpressionStatementSyntaxAbstraction(
			ImmutableArray<ISyntaxNodeAbstraction>.Empty,
			Mock.Of<ISymbol>(MockBehavior.Strict),
			nodeLocation
			);
		var result = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Cannot get the result of an expression statement");
		result.T2Value.Location.Should().BeSameAs(nodeLocation);
	}
}