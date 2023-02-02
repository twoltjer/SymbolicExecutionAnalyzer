namespace SymbolicExecution.Test.UnitTests;

public class ObjectCreationExpressionSyntaxAbstractionTests
{
	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(3)]
	public void TestGetExpressionResult_ReturnsError_WhenNumberOfChildrenIsIncorrect(int numberOfChildren)
	{
		var children = Enumerable.Range(0, numberOfChildren)
			.Select(_ => Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict))
			.ToArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new ObjectCreationExpressionSyntaxAbstraction(
			children: children.ToImmutableArray(),
			symbol: default,
			location: location,
			actualTypeSymbol: default,
			convertedTypeSymbol: default
			);
		var results = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		results.IsT1.Should().BeFalse();
		results.T2Value.Reason.Should().Be("Expected object creation syntax to have two children (type and argument list)");
		results.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_ReturnsError_IfFirstChildIsIncorrectType()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new ObjectCreationExpressionSyntaxAbstraction(
			children: new[]
			{
				Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict),
				Mock.Of<IArgumentListSyntaxAbstraction>(MockBehavior.Strict)
			}.ToImmutableArray(),
			symbol: default,
			location: location,
			actualTypeSymbol: default,
			convertedTypeSymbol: default
			);
		var results = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		results.IsT1.Should().BeFalse();
		results.T2Value.Reason.Should().Be("Expected object creation syntax to have an identifier name as its first child");
		results.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_ReturnsError_IfSecondChildIsIncorrectType()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new ObjectCreationExpressionSyntaxAbstraction(
			children: new[]
			{
				Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict),
				Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
			}.ToImmutableArray(),
			symbol: default,
			location: location,
			actualTypeSymbol: default,
			convertedTypeSymbol: default
			);
		var results = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		results.IsT1.Should().BeFalse();
		results.T2Value.Reason.Should().Be("Expected object creation syntax to have an argument list as its second child");
		results.T2Value.Location.Should().BeSameAs(location);
	}

	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(true, true)]
	[InlineData(false, true)]
	[InlineData(true, false)]
	public void TestGetExpressionResult_ReturnsError_IfEitherSymbolIsNull(bool actualTypeSymbolIsNull, bool convertedTypeSymbolIsNull)
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new ObjectCreationExpressionSyntaxAbstraction(
			children: new ISyntaxNodeAbstraction[]
			{
				Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict),
				Mock.Of<IArgumentListSyntaxAbstraction>(MockBehavior.Strict)
			}.ToImmutableArray(),
			symbol: null,
			location: location,
			actualTypeSymbol: actualTypeSymbolIsNull ? null : Mock.Of<ITypeSymbol>(MockBehavior.Strict),
			convertedTypeSymbol: convertedTypeSymbolIsNull ? null : Mock.Of<ITypeSymbol>(MockBehavior.Strict)
			);
		var results = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		results.IsT1.Should().BeFalse();
		var errorReason = actualTypeSymbolIsNull
			? "Expected object creation syntax to have an actual type symbol"
			: "Expected object creation syntax to have a converted type symbol";
		results.T2Value.Reason.Should().Be(errorReason);
		results.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResult_Succeeds_WithGoodParameters()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var actualTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		var convertedTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		var subject = new ObjectCreationExpressionSyntaxAbstraction(
			children: new ISyntaxNodeAbstraction[]
			{
				Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict),
				Mock.Of<IArgumentListSyntaxAbstraction>(MockBehavior.Strict)
			}.ToImmutableArray(),
			symbol: Mock.Of<IMethodSymbol>(MockBehavior.Strict),
			location: location,
			actualTypeSymbol: actualTypeSymbol,
			convertedTypeSymbol: convertedTypeSymbol
			);
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var results = subject.GetExpressionResults(state);
		results.IsT1.Should().BeTrue();
		var (resultValue, resultState) = results.T1Value.Single();
		resultValue.Location.Should().BeSameAs(location);
		resultValue.ActualTypeSymbol.Should().BeSameAs(actualTypeSymbol);
		resultValue.ConvertedTypeSymbol.Should().BeSameAs(convertedTypeSymbol);
		resultState.Should().BeSameAs(state);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_Fails_Always()
	{
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var subject = new ObjectCreationExpressionSyntaxAbstraction(
			children: default,
			symbol: default,
			location: location,
			actualTypeSymbol: default,
			convertedTypeSymbol: default
			);
		var state = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(state);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Object creation syntax should not be traversed, but evaluated as an expression");
		result.T2Value.Location.Should().BeSameAs(location);
	}
}