namespace SymbolicExecution.Test.UnitTests;

public class VariableDeclarationSyntaxAbstractionTests
{
	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(3)]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithNotTwoChildren_ReturnsFailure(int count)
	{
		var children = Enumerable.Range(0, count).Select(_ => Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)).ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Local declaration statement must have exactly two children");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithFirstChildNotPredefinedType_ReturnsFailure()
	{
		var children = new[]
		{
			Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict),
			Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
		}.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Local declaration statement must have a predefined type as its first child or an identifier name with a type symbol");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithSecondChildNotVariableDeclarator_ReturnsFailure()
	{
		var children = new[]
		{
			Mock.Of<IPredefinedTypeSyntaxAbstraction>(MockBehavior.Strict),
			Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
		}.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Local declaration statement must have a variable declarator as its second child");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithDeclaratorNotHavingSymbol_ReturnsFailure()
	{
		var declarator = Mock.Of<IVariableDeclaratorSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(declarator).SetupGet(x => x.Symbol).Returns((ISymbol)null);
		var children = new ISyntaxNodeAbstraction[]
		{
			Mock.Of<IPredefinedTypeSyntaxAbstraction>(MockBehavior.Strict),
			declarator
		}.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Variable declarator must have an associated symbol");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithDeclaratorHavingNonLocalSymbol_ReturnsFailure()
	{
		var declarator = Mock.Of<IVariableDeclaratorSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(declarator).SetupGet(x => x.Symbol).Returns(Mock.Of<ISymbol>(MockBehavior.Strict));
		var children = new ISyntaxNodeAbstraction[]
		{
			Mock.Of<IPredefinedTypeSyntaxAbstraction>(MockBehavior.Strict),
			declarator
		}.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Variable declarator must have an associated symbol");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithDeclaratorReturningMoreThanOneChild_ReturnsFailure()
	{
		var declaratorChildren = new[]
		{
			Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict),
			Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
		}.ToImmutableArray();
		var declarator = Mock.Of<IVariableDeclaratorSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(declarator).SetupGet(x => x.Symbol).Returns(Mock.Of<ILocalSymbol>(MockBehavior.Strict));
		Mock.Get(declarator).Setup(x => x.Children).Returns(declaratorChildren);
		var children = new ISyntaxNodeAbstraction[]
		{
			Mock.Of<IPredefinedTypeSyntaxAbstraction>(MockBehavior.Strict),
			declarator
		}.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Variable declarator must have less than two children");
		result.T2Value.Location.Should().BeSameAs(location);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithNoDeclaratorChildren_ReturnsHelperNoDeclaratorChildResult()
	{
		var declarator = Mock.Of<IVariableDeclaratorSyntaxAbstraction>(MockBehavior.Strict);
		var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		Mock.Get(declarator).SetupGet(x => x.Symbol).Returns(localSymbol);
		Mock.Get(declarator).Setup(x => x.Children).Returns(ImmutableArray<ISyntaxNodeAbstraction>.Empty);
		var children = new ISyntaxNodeAbstraction[]
		{
			Mock.Of<IPredefinedTypeSyntaxAbstraction>(MockBehavior.Strict),
			declarator
		}.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var helperResults = new[]
		{
			Mock.Of<IAnalysisState>(MockBehavior.Strict)
		}.ToImmutableArray();
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		Mock.Get(helper).Setup(x => x.AnalyzeNodeWithNoDeclaratorChild(previous, localSymbol)).Returns(new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(helperResults));
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeTrue();
		result.T1Value.Should().BeEquivalentTo(helperResults);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyzeNode_WithOneDeclaratorChild_ReturnsHelperOneDeclaratorChildResult()
	{
		var declarator = Mock.Of<IVariableDeclaratorSyntaxAbstraction>(MockBehavior.Strict);
		var localSymbol = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		Mock.Get(declarator).SetupGet(x => x.Symbol).Returns(localSymbol);
		var declaratorChild = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
		Mock.Get(declarator).Setup(x => x.Children).Returns(new[] { declaratorChild }.ToImmutableArray());
		var children = new ISyntaxNodeAbstraction[]
		{
			Mock.Of<IPredefinedTypeSyntaxAbstraction>(MockBehavior.Strict),
			declarator
		}.ToImmutableArray();
		var location = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var helperResults = new[]
		{
			Mock.Of<IAnalysisState>(MockBehavior.Strict)
		}.ToImmutableArray();
		var previous = Mock.Of<IAnalysisState>(MockBehavior.Strict);
		Mock.Get(helper).Setup(x => x.AnalyzeNodeWithOneDeclaratorChild(previous, localSymbol, declaratorChild)).Returns(new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(helperResults));
		var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(children, symbol, location, helper);
		var result = subject.AnalyzeNode(previous);
		result.IsT1.Should().BeTrue();
		result.T1Value.Should().BeEquivalentTo(helperResults);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestGetExpressionResults_Always_ReturnsAnalysisFailure()
	{
		var nodeLocation = Mock.Of<Location>(MockBehavior.Strict);
		var helper = Mock.Of<IVariableDeclarationSyntaxAbstractionHelper>(MockBehavior.Strict);
		var subject = new VariableDeclarationSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction>.Empty, Mock.Of<ISymbol>(MockBehavior.Strict), nodeLocation, helper);
		var result = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
		result.IsT1.Should().BeFalse();
		result.T2Value.Reason.Should().Be("Variable declaration syntax does not have an expression");
		result.T2Value.Location.Should().BeSameAs(nodeLocation);
	}
}