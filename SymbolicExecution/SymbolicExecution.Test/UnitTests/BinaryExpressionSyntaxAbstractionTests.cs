// namespace SymbolicExecution.Test.UnitTests;
//
// public class BinaryExpressionSyntaxAbstractionTests
// {
// 	[Fact]
// 	[Trait("Category", "Unit")]
// 	public void TestAnalyzeNode_Always_ReturnsFailure()
// 	{
// 		var location = Mock.Of<Location>(MockBehavior.Strict);
// 		var actualTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
// 		var convertedTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
// 		var subject = new BinaryExpressionSyntaxAbstraction(
// 			ImmutableArray<ISyntaxNodeAbstraction>.Empty,
// 			Mock.Of<ISymbol>(MockBehavior.Strict),
// 			location,
// 			actualTypeSymbol,
// 			convertedTypeSymbol
// 			);
// 		var result = subject.AnalyzeNode(Mock.Of<IAnalysisState>(MockBehavior.Strict));
// 		result.IsT1.Should().BeFalse();
// 		result.T2Value.Reason.Should().Be("Cannot analyze a binary expression");
// 		result.T2Value.Location.Should().BeSameAs(location);
// 	}
//
// 	[Fact]
// 	[Trait("Category", "Unit")]
// 	public void TestGetExpressionResults_Always_ReturnsAnalysisFailure()
// 	{
// 		var nodeLocation = Mock.Of<Location>(MockBehavior.Strict);
// 		var actualTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
// 		var convertedTypeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
// 		var subject = new BinaryExpressionSyntaxAbstraction(
// 			ImmutableArray<ISyntaxNodeAbstraction>.Empty,
// 			Mock.Of<ISymbol>(MockBehavior.Strict),
// 			nodeLocation,
// 			actualTypeSymbol,
// 			convertedTypeSymbol
// 			);
// 		var result = subject.GetExpressionResults(Mock.Of<IAnalysisState>(MockBehavior.Strict));
// 		result.IsT1.Should().BeFalse();
// 		result.T2Value.Reason.Should().Be("Cannot get the expression results of a binary expression");
// 		result.T2Value.Location.Should().BeSameAs(nodeLocation);
// 	}
// }