namespace SymbolicExecution.Test.UnitTests;

public class VariableDeclarationSyntaxAbstractionHelperTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNodeWithNoDeclaratorChild_Always_AddsLocalVariableToState()
	{
		var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var previousState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		Mock.Get(previousState).Setup(x => x.AddLocalVariable(localSymbol)).Returns(modifiedState);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstractionHelper(location);
		var result = subject.AnalyzeNodeWithNoDeclaratorChild(previousState, localSymbol);
		result.IsT1.Should().BeTrue();
		result.T1Value.Count().Should().Be(1);
		result.T1Value.Single().Should().Be(modifiedState);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNodeWithOneDeclaratorChild_WhenGivenANonEqualsValueClause_ReturnsAnalysisFailure()
	{
		var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstractionHelper(location);
		var declaratorChild = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		var result = subject.AnalyzeNodeWithOneDeclaratorChild(state, localSymbol, declaratorChild);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Variable declarator must have an equals value clause as its child");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(2)]
	[InlineData(3)]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNodeWithOneDeclaratorChild_WhenEqualsValueClauseHasNotOneChildren_ReturnsAnalysisFailure(int childCount)
	{
		var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstractionHelper(location);
		var equalsValueClauseSyntaxAbstraction = Mock.Of<IEqualsValueClauseSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(equalsValueClauseSyntaxAbstraction).Setup(x => x.Children).Returns(Enumerable.Repeat(Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict), childCount).ToImmutableArray());
		var result = subject.AnalyzeNodeWithOneDeclaratorChild(state, localSymbol, equalsValueClauseSyntaxAbstraction);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Equals value clause must have exactly one child");
		result.T2Value.Location.Should().BeSameAs(location);
	}
	
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNodeWithOneDeclaratorChild_WhenEqualsValueChildCannotGetExpressionResult_ReturnsAnalysisFailure()
	{
		var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstractionHelper(location);
		var equalsValueClauseSyntaxAbstraction = Mock.Of<IEqualsValueClauseSyntaxAbstraction>(MockBehavior.Strict);
		var equalsValueChild = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var failureLocation = Mock.Of<Location>(MockBehavior.Strict);
		var failure = new AnalysisFailure("Equals value child cannot get expression result", failureLocation);
		Mock.Get(equalsValueChild).Setup(x => x.GetResults(state)).Returns(failure);
		Mock.Get(equalsValueClauseSyntaxAbstraction).Setup(x => x.Children).Returns(new[] { equalsValueChild as ISyntaxNodeAbstraction }.ToImmutableArray());
		var result = subject.AnalyzeNodeWithOneDeclaratorChild(state, localSymbol, equalsValueClauseSyntaxAbstraction);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Equals value child cannot get expression result");
		result.T2Value.Location.Should().BeSameAs(failureLocation);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNodeWithOneDeclaratorChild_WhenGivenGoodCaseWithTwoResultStates_SucceedsWithTwoStates()
	{
		var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var previousState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState1 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState2 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var localVariableState1 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var localVariableState2 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var localVariableAssignedState1 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var localVariableAssignedState2 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var declarationLocation = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstractionHelper(declarationLocation);
		var equalsValueClauseSyntaxAbstraction = Mock.Of<IEqualsValueClauseSyntaxAbstraction>(MockBehavior.Strict);
		var equalsValueChild = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var objectInstance1 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var objectInstance2 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		Mock.Get(equalsValueChild).Setup(x => x.GetResults(previousState)).Returns(new[] { (objectInstance1.Reference, modifiedState1), (objectInstance2.Reference, modifiedState2) }.ToImmutableArray());
		Mock.Get(equalsValueClauseSyntaxAbstraction).Setup(x => x.Children).Returns(new ISyntaxNodeAbstraction[] { equalsValueChild }.ToImmutableArray());
		Mock.Get(modifiedState1).Setup(x => x.AddLocalVariable(localSymbol)).Returns(localVariableState1);
		Mock.Get(modifiedState2).Setup(x => x.AddLocalVariable(localSymbol)).Returns(localVariableState2);
		Mock.Get(localVariableState1).Setup(x => x.SetSymbolReference(localSymbol, objectInstance1.Reference)).Returns(new TaggedUnion<IAnalysisState, AnalysisFailure>(localVariableAssignedState1));
		Mock.Get(localVariableState2).Setup(x => x.SetSymbolReference(localSymbol, objectInstance2.Reference)).Returns(new TaggedUnion<IAnalysisState, AnalysisFailure>(localVariableAssignedState2));
		var result = subject.AnalyzeNodeWithOneDeclaratorChild(previousState, localSymbol, equalsValueClauseSyntaxAbstraction);
		result.IsT1.Should().BeTrue();
		result.T1Value.Should().BeEquivalentTo(new[] { localVariableAssignedState1, localVariableAssignedState2 });
	}
	
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNodeWithOneDeclaratorChild_WhenCannotSetSymbolValue_ReturnsAnalysisFailureAndShortcutsOut()
	{
	var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var previousState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState1 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState2 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var localVariableState1 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var localVariableState2 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var declarationLocation = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstractionHelper(declarationLocation);
		var equalsValueClauseSyntaxAbstraction = Mock.Of<IEqualsValueClauseSyntaxAbstraction>(MockBehavior.Strict);
		var equalsValueChild = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var objectInstance1 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var objectInstance2 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var failureLocation = Mock.Of<Location>(MockBehavior.Strict);
		var setFailure = new AnalysisFailure("Set failure", failureLocation);
		Mock.Get(equalsValueChild)
			.Setup(x => x.GetResults(previousState))
			.Returns(new[]
			{
				(objectInstance1.Reference, modifiedState1),
				(objectInstance2.Reference, modifiedState2)
			}.ToImmutableArray());
		Mock.Get(equalsValueClauseSyntaxAbstraction).Setup(x => x.Children).Returns(new[] { equalsValueChild as ISyntaxNodeAbstraction }.ToImmutableArray());
		Mock.Get(modifiedState1).Setup(x => x.AddLocalVariable(localSymbol)).Returns(localVariableState1);
		Mock.Get(modifiedState2).Setup(x => x.AddLocalVariable(localSymbol)).Returns(localVariableState2);
		Mock.Get(localVariableState1).Setup(x => x.SetSymbolReference(localSymbol, objectInstance1.Reference)).Returns(new TaggedUnion<IAnalysisState, AnalysisFailure>(setFailure));
		var result = subject.AnalyzeNodeWithOneDeclaratorChild(previousState, localSymbol, equalsValueClauseSyntaxAbstraction);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Set failure");
		result.T2Value.Location.Should().BeSameAs(failureLocation);
	}
}