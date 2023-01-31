namespace SymbolicExecution.Test.UnitTests;

public class AbstractMethodAnalyzerTests
{
	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(0)]
	[InlineData(2)]
	public void TestAnalyze_WhenThereIsNoSingleBlockAbstraction_ReturnsFailure(int numberOfBlockSyntaxAbstractions)
	{
		var methodBlock = Enumerable.Range(0, numberOfBlockSyntaxAbstractions)
			.Select(_ => Mock.Of<IBlockSyntaxAbstraction>(MockBehavior.Strict) as ISyntaxNodeAbstraction)
			.ToImmutableArray();
		var location = Mock.Of<Location>(
			loc => loc.ToString() == "MyLocation" && loc.Equals(It.Is<Location>(l => ReferenceEquals(l, loc))),
			MockBehavior.Strict
			);
		var methodDeclaration = Mock.Of<IMethodDeclarationSyntaxAbstraction>(
			node => node.Children == methodBlock && node.Location == location,
			MockBehavior.Strict
			);
		var subject = new AbstractMethodAnalyzer();
		var result = subject.Analyze(methodDeclaration);
		result.UnhandledExceptions.Should().BeEmpty();
		result.AnalysisFailures.Should().HaveCount(1);
		result.AnalysisFailures.Single().Location.Should().Be(location);
		result.AnalysisFailures.Single().Reason.Should().Be(
			$"Could not get an {nameof(IBlockSyntaxAbstraction)} from the method {methodDeclaration}"
			);
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestAnalyze_WhenAnalyzeNodeFails_ReturnsFailure()
	{
		var failureLocation = Mock.Of<Location>(
			loc => loc.ToString() == "MyLocation" && loc.Equals(It.Is<Location>(l => ReferenceEquals(l, loc))),
			MockBehavior.Strict
			);
		var failure = new AnalysisFailure("No good reason", failureLocation);
		var methodBlock = Mock.Of<IBlockSyntaxAbstraction>(MockBehavior.Strict);
		Mock.Get(methodBlock)
			.Setup(node => node.AnalyzeNode(It.IsAny<IAnalysisState>()))
			.Returns(new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(failure));
		var methodDeclaration = Mock.Of<IMethodDeclarationSyntaxAbstraction>(
			node => node.Children == new ISyntaxNodeAbstraction[] { methodBlock }.ToImmutableArray(),
			MockBehavior.Strict
			);
		var subject = new AbstractMethodAnalyzer();
		var result = subject.Analyze(methodDeclaration);
		result.AnalysisFailures.Should().HaveCount(1);
		result.AnalysisFailures.Single().Location.Should().Be(failureLocation);
		result.AnalysisFailures.Single().Reason.Should().Be(failure.Reason);
		result.UnhandledExceptions.Should().BeEmpty();
	}

	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(true)]
	[InlineData(false)]
	public void TestAnalyzer_WhenAnalyzeNodeSucceeds_ReturnsSuccess(bool analyzeFoundUnhandledException)
	{
		var methodBlock = Mock.Of<IBlockSyntaxAbstraction>(MockBehavior.Strict);
		var methodDeclaration = Mock.Of<IMethodDeclarationSyntaxAbstraction>(
			node => node.Children == new ISyntaxNodeAbstraction[] { methodBlock }.ToImmutableArray(),
			MockBehavior.Strict
			);
		var exceptionLocation = Mock.Of<Location>(
			loc => loc.ToString() == "MyLocation" && loc.Equals(It.Is<Location>(l => ReferenceEquals(l, loc))),
			MockBehavior.Strict
			);
		var exceptionTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		var exceptionConvertedTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		var exceptionObject = new ObjectInstance(exceptionTypeSymbol, exceptionConvertedTypeSymbol, exceptionLocation);
		var exceptionState = Mock.Of<IExceptionThrownState>(exception => exception.Location == exceptionLocation, MockBehavior.Strict);
		Mock.Get(exceptionState)
			.Setup(state => state.Exception)
			.Returns(exceptionObject);
		IAnalysisState resultState;
		if (analyzeFoundUnhandledException)
		{
			resultState = Mock.Of<IAnalysisState>(
				state => state.CurrentException == exceptionState,
				MockBehavior.Strict
				);
		}
		else
		{
			resultState = Mock.Of<IAnalysisState>(
				state => state.CurrentException == null,
				MockBehavior.Strict
				);
		}
		Mock.Get(methodBlock)
			.Setup(node => node.AnalyzeNode(It.IsAny<IAnalysisState>()))
			.Returns(new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(new[] { resultState }));
		var subject = new AbstractMethodAnalyzer();
		var result = subject.Analyze(methodDeclaration);
		result.AnalysisFailures.Should().BeEmpty();
		if (analyzeFoundUnhandledException)
		{
			result.UnhandledExceptions.Should().HaveCount(1);
		}
		else
		{
			result.UnhandledExceptions.Should().BeEmpty();
		}
	}
}