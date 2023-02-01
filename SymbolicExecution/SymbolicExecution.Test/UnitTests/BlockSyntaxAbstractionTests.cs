namespace SymbolicExecution.Test.UnitTests;

public class BlockSyntaxAbstractionTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithNoChildren_ReturnsSameState()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new BlockSyntaxAbstraction(children, null, location);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeTrue();
		result.T1Value.Should().HaveCount(1);
		result.T1Value.Single().Should().BeSameAs(state);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithChildren_IteratesThroughEach()
	{
		var initialState = Mock.Of<IAnalysisState>(state => state.CurrentException == null, MockBehavior.Strict);
		var child1 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var secondState = Mock.Of<IAnalysisState>(state => state.CurrentException == null, MockBehavior.Strict);
		var child2 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var thirdState = Mock.Of<IAnalysisState>(state => state.CurrentException == null, MockBehavior.Strict);
		var child3 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var fourthState = Mock.Of<IAnalysisState>(state => state.CurrentException == null, MockBehavior.Strict);
		Mock.Get(child1)
			.Setup(node => node.AnalyzeNode(initialState))
			.Returns(new[] { secondState });
		Mock.Get(child2)
			.Setup(node => node.AnalyzeNode(secondState))
			.Returns(new[] { thirdState });
		Mock.Get(child3)
			.Setup(node => node.AnalyzeNode(thirdState))
			.Returns(new[] { fourthState });
		var children = new[] { child1, child2, child3 }.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new BlockSyntaxAbstraction(children, null, location);
		var result = subject.AnalyzeNode(initialState);
		result.IsT1.Should().BeTrue();
		result.T1Value.Should().HaveCount(1);
		result.T1Value.Single().Should().BeSameAs(fourthState);
		Mock.Get(child1).Verify(x => x.AnalyzeNode(initialState), Times.Once);
		Mock.Get(child2).Verify(x => x.AnalyzeNode(secondState), Times.Once);
		Mock.Get(child3).Verify(x => x.AnalyzeNode(thirdState), Times.Once);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WhenExceptionThrown_ShortcutsFutureChildren()
	{
		var initialState = Mock.Of<IAnalysisState>(state => state.CurrentException == null, MockBehavior.Strict);
		var child1 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var exceptionThrownState = Mock.Of<IExceptionThrownState>(MockBehavior.Strict);
		var secondState = Mock.Of<IAnalysisState>(state => state.CurrentException == exceptionThrownState, MockBehavior.Strict);
		var child2 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var child3 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		Mock.Get(child1)
			.Setup(node => node.AnalyzeNode(initialState))
			.Returns(new[] { secondState });
		var children = new[] { child1, child2, child3 }.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new BlockSyntaxAbstraction(children, null, location);
		var result = subject.AnalyzeNode(initialState);
		result.IsT1.Should().BeTrue();
		result.T1Value.Should().HaveCount(1);
		result.T1Value.Single().Should().BeSameAs(secondState);
		Mock.Get(child1).Verify(x => x.AnalyzeNode(initialState), Times.Once);
		Mock.Get(child2).Verify(x => x.AnalyzeNode(secondState), Times.Never);
		Mock.Get(child3).Verify(x => x.AnalyzeNode(secondState), Times.Never);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_ChildNodeFailure_IsForwarded()
	{
		var initialState = Mock.Of<IAnalysisState>(state => state.CurrentException == null, MockBehavior.Strict);
		var child1 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var secondState = Mock.Of<IAnalysisState>(state => state.CurrentException == null, MockBehavior.Strict);
		Mock.Get(child1)
			.Setup(node => node.AnalyzeNode(initialState))
			.Returns(new[] { secondState });
		// Child 2 fails to analyze and we stop after that
		var child2 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var failureLocation = Mock.Of<Location>(MockBehavior.Strict);
		var failure = new AnalysisFailure("No reason", failureLocation);
		Mock.Get(child2)
			.Setup(node => node.AnalyzeNode(secondState))
			.Returns(failure);
		var child3 = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var children = new[] { child1, child2, child3 }.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new BlockSyntaxAbstraction(children, null, location);
		var result = subject.AnalyzeNode(initialState);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(failureLocation);
		result.T2Value.Reason.Should().Be("No reason");
		Mock.Get(child1).Verify(x => x.AnalyzeNode(initialState), Times.Once);
		Mock.Get(child2).Verify(x => x.AnalyzeNode(secondState), Times.Once);
		Mock.Get(child3).Verify(x => x.AnalyzeNode(secondState), Times.Never);
	}
}