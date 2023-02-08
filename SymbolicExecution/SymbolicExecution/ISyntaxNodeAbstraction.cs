namespace SymbolicExecution;

public interface ISyntaxNodeAbstraction
{
	ImmutableArray<ISyntaxNodeAbstraction> Children { get; }
	ISymbol? Symbol { get; }
	IEnumerable<ISyntaxNodeAbstraction> GetDescendantNodes(bool includeSelf);
	TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous);
	TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state);
	Location Location { get; }
}