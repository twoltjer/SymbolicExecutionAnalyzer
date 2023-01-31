namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class CompilationUnitSyntaxAbstraction : CSharpSyntaxNodeAbstraction, ICompilationUnitSyntaxAbstraction
{
	public CompilationUnitSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		throw new NotImplementedException();
	}
}