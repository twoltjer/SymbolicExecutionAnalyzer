using Microsoft.CodeAnalysis.Text;

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
		var child = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var children = ImmutableArray.Create(child);
		var subject = new ThrowStatementSyntaxAbstraction(children, null, location);
		var failure = new AnalysisFailure("Reason", location);
		Mock.Get(child)
			.Setup(x => x.GetExpressionResult(state))
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
			.Setup(state => state.ThrowException(exceptionObject, exceptionLocation))
			.Returns(modifiedState);
		Mock.Get(initialState)
			.Setup(state => state.ThrowException(exceptionObject, throwLocation))
			.Returns(modifiedState);
		var child = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		Mock.Get(child)
			.Setup(x => x.GetExpressionResult(initialState))
			.Returns(new TaggedUnion<IObjectInstance, AnalysisFailure>(exceptionObject));
		var children = ImmutableArray.Create(child);
		var subject = new ThrowStatementSyntaxAbstraction(children, null, throwLocation);
		var result = subject.AnalyzeNode(initialState);
		result.IsT1.Should().BeTrue();
		result.T1Value.Count().Should().Be(1);
		result.T1Value.Single().Should().BeSameAs(modifiedState);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_Always_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var subject = new ThrowStatementSyntaxAbstraction(children, null, location);
		var result = subject.GetExpressionResult(state);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(location);
		result.T2Value.Reason.Should().Be("Cannot analyze throw statements");
	}
}