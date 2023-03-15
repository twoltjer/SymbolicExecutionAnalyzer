using System.Collections;

namespace SymbolicExecution.Test.UnitTests;

public class AssignmentExpressionSyntaxAbstractionTests
{
    [Theory]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void TestAnalyzeNode_WithNotTwoChildren_ReturnsAnalysisFailure(int childCount)
    {
        var children = Enumerable.Range(0, childCount).Select(_ => Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)).ToImmutableArray();
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Assignment expression must have exactly two children");
        result.T2Value.Location.Should().BeSameAs(location);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_WithFirstChildNotIdentifierName_ReturnsAnalysisFailure()
    {
        var children = new[]
        {
            Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict),
            Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
        }.ToImmutableArray();
        
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Cannot assign to a non-identifier expression");
        result.T2Value.Location.Should().BeSameAs(location);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_WithIdentifierSymbolNull_ReturnsAnalysisFailure()
    {
        var identifierName = Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict);
        Mock.Get(identifierName).SetupGet(x => x.Symbol).Returns((ISymbol)null);
        var children = new[]
        {
            identifierName,
            Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
        }.ToImmutableArray();
        
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Cannot assign to a non-identifier expression");
        result.T2Value.Location.Should().BeSameAs(location);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_WithIdentifierSymbolNotLocal_ReturnsAnalysisFailure()
    {
        var identifierName = Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict);
        Mock.Get(identifierName).SetupGet(x => x.Symbol).Returns(Mock.Of<ISymbol>(MockBehavior.Strict));
        var children = new[]
        {
            identifierName,
            Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
        }.ToImmutableArray();
        
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Cannot assign to a non-identifier expression");
        result.T2Value.Location.Should().BeSameAs(location);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_WithNonExpressionSyntaxAsSecondChild_ReturnsAnalysisFailure()
    {
        var identifierName = Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict);
        Mock.Get(identifierName).SetupGet(x => x.Symbol).Returns(Mock.Of<ILocalSymbol>(MockBehavior.Strict));
        var children = new[]
        {
            identifierName,
            Mock.Of<ISyntaxNodeAbstraction>(MockBehavior.Strict)
        }.ToImmutableArray();
        
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Assignment expression must have an expression as its second child");
        result.T2Value.Location.Should().BeSameAs(location);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_WithExpressionResultNotSuccess_ReturnsAnalysisFailure()
    {
        var identifierName = Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict);
        Mock.Get(identifierName).SetupGet(x => x.Symbol).Returns(Mock.Of<ILocalSymbol>(MockBehavior.Strict));
        var expression = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
        var failureLocation = Mock.Of<Location>(MockBehavior.Strict);
        var analysisFailure = new AnalysisFailure("reason", failureLocation);
        Mock.Get(expression).Setup(x => x.GetExpressionResults(It.IsAny<IAnalysisState>())).Returns(analysisFailure);
        var children = new ISyntaxNodeAbstraction[]
        {
            identifierName,
            expression
        }.ToImmutableArray();
        
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("reason");
        result.T2Value.Location.Should().BeSameAs(failureLocation);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_IfSetSymbolFailsOnFirst_ShortcutsOut()
    {
        var initialState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
        var identifierName = Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict);
        Mock.Get(identifierName).SetupGet(x => x.Symbol).Returns(Mock.Of<ILocalSymbol>(MockBehavior.Strict));
        var expression = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
        var expressionResult1 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
        var expressionResult2 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
        var expressionResultState1 = Mock.Of<IAnalysisState>(x => x.IsReturning == false && x.CurrentException == null, MockBehavior.Strict);
        var expressionResultState2 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
        var expressionResults = new[]
        {
            (expressionResult1, expressionResultState1),
            (expressionResult2, expressionResultState2)
        }.ToImmutableArray();
        
        var failureLocation = Mock.Of<Location>(MockBehavior.Strict);
        var failure = new AnalysisFailure("Reason", failureLocation);
        Mock.Get(expressionResultState1).Setup(x => x.SetSymbolValue(identifierName.Symbol, expressionResult1, It.IsAny<Location>())).Returns(failure);
        
        Mock.Get(expression).Setup(x => x.GetExpressionResults(initialState)).Returns(expressionResults);
        var children = new ISyntaxNodeAbstraction[]
        {
            identifierName,
            expression
        }.ToImmutableArray();
        
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(initialState);
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Reason");
        result.T2Value.Location.Should().BeSameAs(failureLocation);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestAnalyzeNode_GoodPathWithTwoResults_ReturnsSuccessfully()
    {
        var initialState = Mock.Of<IAnalysisState>(MockBehavior.Strict);
        var identifierName = Mock.Of<IIdentifierNameSyntaxAbstraction>(MockBehavior.Strict);
        Mock.Get(identifierName).SetupGet(x => x.Symbol).Returns(Mock.Of<ILocalSymbol>(MockBehavior.Strict));
        var expression = Mock.Of<IExpressionSyntaxAbstraction>(MockBehavior.Strict);
        var expressionResult1 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
        var expressionResult2 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
        var expressionResultState1 = Mock.Of<IAnalysisState>(x => x.IsReturning == false && x.CurrentException == null, MockBehavior.Strict);
        var expressionResultState2 = Mock.Of<IAnalysisState>(x => x.IsReturning == false && x.CurrentException == null, MockBehavior.Strict);
        var expressionResultModifiedState1 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
        var expressionResultModifiedState2 = Mock.Of<IAnalysisState>(MockBehavior.Strict);
        var expressionResults = new[]
        {
            (expressionResult1, expressionResultState1),
            (expressionResult2, expressionResultState2)
        }.ToImmutableArray();

        Mock.Get(expressionResultState1)
            .Setup(x => x.SetSymbolValue(identifierName.Symbol, expressionResult1, It.IsAny<Location>()))
            .Returns(new TaggedUnion<IAnalysisState, AnalysisFailure>(expressionResultModifiedState1));
        
        Mock.Get(expressionResultState2)
            .Setup(x => x.SetSymbolValue(identifierName.Symbol, expressionResult2, It.IsAny<Location>()))
            .Returns(new TaggedUnion<IAnalysisState, AnalysisFailure>(expressionResultModifiedState2));
        
        Mock.Get(expression).Setup(x => x.GetExpressionResults(initialState)).Returns(expressionResults);
        var children = new ISyntaxNodeAbstraction[]
        {
            identifierName,
            expression
        }.ToImmutableArray();
        
        var location = Mock.Of<Location>(MockBehavior.Strict);
        var symbol = Mock.Of<ISymbol>(MockBehavior.Strict);
        var actualType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var convertedType = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
        var subject = new AssignmentExpressionSyntaxAbstraction(children, symbol, location, actualType, convertedType);
        var result = subject.AnalyzeNode(initialState);
        result.IsT1.Should().BeTrue();
        var resultsArray = result.T1Value.ToArray();
        resultsArray.Length.Should().Be(2);
        resultsArray[0].Should().BeSameAs(expressionResultModifiedState1);
        resultsArray[1].Should().BeSameAs(expressionResultModifiedState2);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetExpressionResults_Always_ReturnsFailure()
    {
        var subject = new AssignmentExpressionSyntaxAbstraction(
            ImmutableArray<ISyntaxNodeAbstraction>.Empty,
            Mock.Of<ISymbol>(MockBehavior.Strict),
            Mock.Of<Location>(MockBehavior.Strict),
            Mock.Of<ITypeSymbol>(MockBehavior.Strict),
            Mock.Of<ITypeSymbol>(MockBehavior.Strict));
        
        var result = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
        result.IsT1.Should().BeFalse();
        result.T2Value.Reason.Should().Be("Cannot get the result of an assignment expression");
        result.T2Value.Location.Should().BeSameAs(subject.Location);
    }
}