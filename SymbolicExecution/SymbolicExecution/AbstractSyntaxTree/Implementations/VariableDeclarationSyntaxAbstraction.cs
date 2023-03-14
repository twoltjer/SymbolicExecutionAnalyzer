namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class VariableDeclarationSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IVariableDeclarationSyntaxAbstraction
{
	private readonly IVariableDeclarationSyntaxAbstractionHelper _helper;

	public VariableDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		IVariableDeclarationSyntaxAbstractionHelper helper
		) : base(children, symbol, location)
	{
		_helper = helper;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		// Return failure if not exactly two children
		if (Children.Length != 2)
		{
			return new AnalysisFailure("Local declaration statement must have exactly two children", Location);
		}

		// First child should be a predefined type
		if (!(Children[0] is IPredefinedTypeSyntaxAbstraction) && !(Children[0] is IIdentifierNameSyntaxAbstraction
		    {
			    Symbol: ITypeSymbol
		    }))
		{
			return new AnalysisFailure(
				"Local declaration statement must have a predefined type as its first child or an identifier name with a type symbol",
				Location
				);
		}

		// Second child should be a variable declarator
		if (!(Children[1] is IVariableDeclaratorSyntaxAbstraction variableDeclaratorSyntaxAbstraction))
		{
			return new AnalysisFailure(
				"Local declaration statement must have a variable declarator as its second child",
				Location
				);
		}

		// Variable declarator should have an associated symbol which is
		if (variableDeclaratorSyntaxAbstraction.Symbol is not ILocalSymbol localSymbol)
		{
			return new AnalysisFailure("Variable declarator must have an associated symbol", Location);
		}

		var variableDeclaratorChildren = variableDeclaratorSyntaxAbstraction.Children;
		if (variableDeclaratorChildren.Length > 1)
		{
			return new AnalysisFailure("Variable declarator must have less than two children", Location);
		}
		else if (variableDeclaratorChildren.Length == 0)
		{
			return _helper.AnalyzeNodeWithNoDeclaratorChild(previous, localSymbol);
		}

		return _helper.AnalyzeNodeWithOneDeclaratorChild(previous, localSymbol, variableDeclaratorChildren[0]);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Variable declaration syntax does not have an expression", Location);
	}
}