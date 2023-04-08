namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class MemberDeclarationSyntaxAbstraction : CSharpSyntaxNodeAbstraction,
	IMemberDeclarationSyntaxAbstraction
{
	protected MemberDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}

public abstract class BaseNamespaceDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction
{
	protected BaseNamespaceDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}

public class FileScopedNamespaceDeclarationSyntaxAbstraction : BaseNamespaceDeclarationSyntaxAbstraction
{
	public FileScopedNamespaceDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new[] { previous };
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("FileScopedNamespaceDeclarationSyntaxAbstraction.GetExpressionResults", Location);
	}
}