namespace SymbolicExecution.Test.UnitTests;

public class LocalDeclarationStatementSyntaxAbstractionTests
{
    [Theory]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(3)]
    public void TestAnalyzeNode_WithNotOneChild_ReturnsFailure(int childCount)
    {
        var children = Enumerable.Range(0, childCount).Select(x => Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)).ToImmutableArray();
        var subject = new LocalDeclarationStatementSyntaxAbstraction(children, Mock.Of<ISymbol>(MockBehavior.Strict), Mock.Of<Location>(MockBehavior.Strict));
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Local declaration statement must have exactly one child");
        result.T2Value.Location.Should().BeSameAs(subject.Location);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_WhenChildIsNotVariableDeclaration_ReturnsFailure()
    {
        var child = Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict);
        var children = new ISyntaxNodeAbstraction[]
        {
            child
        }.ToImmutableArray();
        var subject = new LocalDeclarationStatementSyntaxAbstraction(children, Mock.Of<ISymbol>(MockBehavior.Strict), Mock.Of<Location>(MockBehavior.Strict));
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Local declaration statement must have a variable declaration as its child");
        result.T2Value.Location.Should().BeSameAs(subject.Location);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_WithVariableDeclarationChild_AnalyzesChild()
    {
        var child = Mock.Of<IVariableDeclarationSyntaxAbstraction>(MockBehavior.Strict);
        var children = new ISyntaxNodeAbstraction[]
        {
            child
        }.ToImmutableArray();
        var initialState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
        var modifiedState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
        Mock.Get(child).Setup(x => x.AnalyzeNode(initialState)).Returns(new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(new[] { modifiedState }));
        var subject = new LocalDeclarationStatementSyntaxAbstraction(children, Mock.Of<ISymbol>(MockBehavior.Strict), Mock.Of<Location>(MockBehavior.Strict));
        var result = subject.AnalyzeNode(initialState);
        result.IsT1.Should().BeTrue();
        var resultsArray = result.T1Value.ToArray();
        resultsArray.Length.Should().Be(1);
        resultsArray[0].Should().BeSameAs(modifiedState);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetExpressionResults_Always_ReturnsFailure()
    {
        var subject = new LocalDeclarationStatementSyntaxAbstraction(
            ImmutableArray<ISyntaxNodeAbstraction>.Empty,
            Mock.Of<ISymbol>(MockBehavior.Strict),
            Mock.Of<Location>(MockBehavior.Strict));
        
        var result = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Cannot get the result of a local declaration statement");
        result.T2Value.Location.Should().BeSameAs(subject.Location);
    }
}