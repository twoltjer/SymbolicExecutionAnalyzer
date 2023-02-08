namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class VariableDeclarationSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IVariableDeclarationSyntaxAbstraction
{
	public VariableDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		
		// Return failure if not exactly two children
		if (Children.Length != 2)
		{
			return new AnalysisFailure("Local declaration statement must have exactly two children", Location);
		}
		
		// First child should be a predefined type
		if (!(Children[0] is IPredefinedTypeSyntaxAbstraction predefinedTypeSyntaxAbstraction))
		{
			return new AnalysisFailure("Local declaration statement must have a predefined type as its first child", Location);
		}
		
		// Second child should be a variable declarator
		if (!(Children[1] is IVariableDeclaratorSyntaxAbstraction variableDeclaratorSyntaxAbstraction))
		{
			return new AnalysisFailure("Local declaration statement must have a variable declarator as its second child", Location);
		}

		// Variable declarator should have an associated symbol which is
		if (variableDeclaratorSyntaxAbstraction.Symbol is not ILocalSymbol localSymbol)
		{
			return new AnalysisFailure("Variable declarator must have an associated symbol", Location);
		}

		// Put variable in the state
		var variableName = localSymbol.Name;
		var variableType = localSymbol.Type;
		return new[] { previous.AddLocalVariable(variableName, variableType) };
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an identifier name", Location.None);
	}
}

public class LocalDeclarationStatementSyntaxAbstraction : StatementSyntaxAbstraction, ILocalDeclarationStatementSyntaxAbstraction
{
	public LocalDeclarationStatementSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 1)
		{
			return new AnalysisFailure("Local declaration statement must have exactly one child", Location);
		}

		if (!(Children[0] is IVariableDeclarationSyntaxAbstraction variableDeclarationSyntaxAbstraction))
		{
			return new AnalysisFailure("Local declaration statement must have a variable declaration as its child", Location);
		}

		return variableDeclarationSyntaxAbstraction.AnalyzeNode(previous);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an identifier name", Location.None);
	}
}