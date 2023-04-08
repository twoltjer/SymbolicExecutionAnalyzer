namespace SymbolicExecution.Test.UnitTests;

public class IfStatementSyntaxAbstractionTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithNoElseAlwaysTrueCase_AnalyzesConditional()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var modifiedState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var condition = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var conditional = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(conditional)
			.Setup(x => x.AnalyzeNode(state))
			.Returns(new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(new[] { modifiedState }))
			.Verifiable();
		var conditionalResult = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		Mock.Get(conditionalResult).Setup(result => result.IsExactType(typeof(bool))).Returns(true);
		var valueScope = Mock.Of<IValueScope>(MockBehavior.Strict);
		Mock.Get(valueScope).Setup(scope => scope.CanBe(true)).Returns(true);
		Mock.Get(valueScope).Setup(scope => scope.CanBe(false)).Returns(false);
		Mock.Get(conditionalResult).Setup(result => result.Value).Returns(valueScope);
		Mock.Get(condition)
			.Setup(x => x.GetExpressionResults(state))
			.Returns(
				new TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure>(
					new[] { (conditionalResult, state) }.ToImmutableArray()
					)
				);

		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = new ISyntaxNodeAbstraction[] { condition, conditional }.ToImmutableArray();
		var subject = new IfStatementSyntaxAbstraction(condition, conditional, null, children, null, location);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeTrue();
		result.T1Value.Single().Should().BeSameAs(modifiedState);
		Mock.Get(conditional).Verify(node => node.AnalyzeNode(state), Times.Once);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithNoElseAlwaysFalseCase_DoesNotAnalyzeConditional()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var condition = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var conditional = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(conditional)
			.Setup(x => x.AnalyzeNode(state))
			.Verifiable();
		var conditionalResult = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		Mock.Get(conditionalResult).Setup(result => result.IsExactType(typeof(bool))).Returns(true);
		var valueScope = Mock.Of<IValueScope>(MockBehavior.Strict);
		Mock.Get(valueScope).Setup(scope => scope.CanBe(true)).Returns(false);
		Mock.Get(valueScope).Setup(scope => scope.CanBe(false)).Returns(true);
		Mock.Get(conditionalResult).Setup(result => result.Value).Returns(valueScope);
		Mock.Get(condition)
			.Setup(x => x.GetExpressionResults(state))
			.Returns(
				new TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure>(
					new[] { (conditionalResult, state) }.ToImmutableArray()
					)
				);

		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = new ISyntaxNodeAbstraction[] { condition, conditional }.ToImmutableArray();
		var subject = new IfStatementSyntaxAbstraction(condition, conditional, null, children, null, location);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeTrue();
		result.T1Value.Single().Should().BeSameAs(state);
		Mock.Get(conditional).Verify(node => node.AnalyzeNode(state), Times.Never);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WhenConditionExpressionReturnsFailure_ReturnsFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var condition = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var conditional = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		var failureLocation = Mock.Of<Location>(MockBehavior.Strict);
		Mock.Get(condition)
			.Setup(x => x.GetExpressionResults(state))
			.Returns(
				new TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure>(
					new AnalysisFailure("Test failure", failureLocation)
					)
				);

		var children = new ISyntaxNodeAbstraction[] { condition, conditional }.ToImmutableArray();
		var abstractionLocation = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new IfStatementSyntaxAbstraction(
			condition,
			conditional,
			null,
			children,
			null,
			abstractionLocation
			);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(failureLocation);
		result.T2Value.Reason.Should().Be("Test failure");
	}


	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WhenConditionReturnsNonBool_ReturnsFailure()
	{
		var initialState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var conditionResultObject = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		Mock.Get(conditionResultObject).Setup(result => result.IsExactType(typeof(bool))).Returns(false);
		var condition = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(condition)
			.Setup(node => node.GetExpressionResults(initialState))
			.Returns(ImmutableArray.Create((conditionResultObject, initialState)));
		var trueStatement = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		var falseStatement = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		var children = new ISyntaxNodeAbstraction[] { condition, trueStatement, falseStatement }.ToImmutableArray();
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var ifLocation = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new IfStatementSyntaxAbstraction(
			condition,
			trueStatement,
			falseStatement,
			children,
			symbol,
			ifLocation
			);
		var analysisResult = subject.AnalyzeNode(initialState);
		analysisResult.IsT1.Should().BeFalse();
		analysisResult.T2Value.Location.Should().BeSameAs(ifLocation);
		analysisResult.T2Value.Reason.Should().Be("Condition must be a boolean");
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WhenConditionalReturnsAnalysisFailure_ReturnsFailure()
	{
		var initialState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var valueScope = Mock.Of<IValueScope>(MockBehavior.Strict);
		Mock.Get(valueScope).Setup(scope => scope.CanBe(true)).Returns(true);
		var objectInstance = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		Mock.Get(objectInstance).Setup(result => result.IsExactType(typeof(bool))).Returns(true);
		Mock.Get(objectInstance).Setup(result => result.Value).Returns(valueScope);
		var condition = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(condition)
			.Setup(node => node.GetExpressionResults(initialState))
			.Returns(ImmutableArray.Create((objectInstance, initialState)));
		var conditionalLocation = Mock.Of<Location>(MockBehavior.Strict);
		const string conditionalReason = "Conditional reason";
		var conditional = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(conditional)
			.Setup(node => node.AnalyzeNode(initialState))
			.Returns(new AnalysisFailure(conditionalReason, conditionalLocation));
		var elseConditional = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var ifLocation = Mock.Of<Location>(MockBehavior.Strict);
		var children = new ISyntaxNodeAbstraction[] { condition, conditional, elseConditional }.ToImmutableArray();

		var subject = new IfStatementSyntaxAbstraction(
			condition,
			conditional,
			elseConditional,
			children,
			symbol,
			ifLocation
			);
		var result = subject.AnalyzeNode(initialState);
		result.IsT1.Should().BeFalse();
		result.T2Value.Location.Should().BeSameAs(conditionalLocation);
		result.T2Value.Reason.Should().Be(conditionalReason);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_Always_ReturnsAnalysisFailure()
	{
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var condition = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
		var conditional = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		var elseConditional = Mock.Of<IStatementSyntaxAbstraction>(MockBehavior.Strict);
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var children = ImmutableArray<ISyntaxNodeAbstraction>.Empty;
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new IfStatementSyntaxAbstraction(
			condition,
			conditional,
			elseConditional,
			children,
			symbol,
			location
			);
		var results = subject.GetExpressionResults(state);
		results.IsT1.Should().BeFalse();
		results.T2Value.Location.Should().BeSameAs(location);
		results.T2Value.Reason.Should().Be("Cannot analyze if statements");
	}
}