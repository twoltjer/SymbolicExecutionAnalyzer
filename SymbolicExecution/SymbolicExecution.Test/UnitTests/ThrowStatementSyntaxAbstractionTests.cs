namespace SymbolicExecution.Test.UnitTests;

public class ThrowStatementSyntaxAbstractionTests
{
	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(0)]
	[InlineData(2)]
	public void TestAnalyzeNode_WithUnexpectedChildCount_ReturnsAnalysisFailure(int numberOfChildren)
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = Enumerable.Range(0, numberOfChildren)
			.Select(_ => Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict))
			.ToImmutableArray();
		var subject = new ThrowStatementSyntaxAbstraction(children, null, location);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(location);
		result.T2Value.Reason.Should().Be("Throw statement expected to have exactly one child");
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithChildExpressionResultFailure_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var child = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var children = ImmutableArray.Create(child as ISyntaxNodeAbstraction);
		var subject = new ThrowStatementSyntaxAbstraction(children, null, location);
		var failure = new AnalysisFailure("Reason", location);
		Mock.Get(child)
			.Setup(x => x.GetResults(state))
			.Returns(failure);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(location);
		result.T2Value.Reason.Should().Be("Reason");
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithChildExpressionSuccess_ReturnsThrownState()
	{
		var exceptionLocation = Location.Create("ExceptionFile.cs", new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 12)));
		var initialState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var throwLocation = Location.Create("Throw.cs", new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 12)));
		var exceptionObject = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		Mock.Get(initialState)
			.Setup(state => state.ThrowException(exceptionObject.Reference, exceptionLocation))
			.Returns(modifiedState);
		Mock.Get(initialState)
			.Setup(state => state.ThrowException(exceptionObject.Reference, throwLocation))
			.Returns(modifiedState);
		var child = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(child)
			.Setup(x => x.GetResults(initialState))
			.Returns(
				new TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure>(
					new[] { (exceptionObject.Reference, initialState) }.ToImmutableArray()
					)
				);
		var children = ImmutableArray.Create(child as ISyntaxNodeAbstraction);
		var subject = new ThrowStatementSyntaxAbstraction(children, null, throwLocation);
		var result = subject.AnalyzeNode(initialState);
		result.IsT1.Should().BeTrue();
		result.T1Value.Count().Should().Be(1);
		result.T1Value.Single().Should().BeSameAs(modifiedState);
	}
}