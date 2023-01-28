namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class MethodDeclarationSyntaxAbstraction : BaseMethodDeclarationSyntaxAbstraction, IMethodDeclarationSyntaxAbstraction
{
	public Location? SourceLocation { get; }

	public MethodDeclarationSyntaxAbstraction(
		ImmutableArray<SyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location? sourceLocation
		) : base(children, symbol)
	{
		SourceLocation = sourceLocation;
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