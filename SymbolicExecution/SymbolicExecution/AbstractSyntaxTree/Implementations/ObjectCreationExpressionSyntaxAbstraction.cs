namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ObjectCreationExpressionSyntaxAbstraction : BaseObjectCreationExpressionSyntaxAbstraction, IObjectCreationExpressionSyntaxAbstraction
{
	public ObjectCreationExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? actualTypeSymbol,
		ITypeSymbol? convertedTypeSymbol
		) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Object creation syntax should not be traversed, but evaluated as an expression", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		if (Children.Length != 2)
		{
			return new AnalysisFailure("Expected object creation syntax to have two children (type and argument list)", Location);
		}

		if (Children[0] is not IIdentifierNameSyntaxAbstraction)
		{
			return new AnalysisFailure("Expected object creation syntax to have an identifier name as its first child", Location);
		}

		if (Children[1] is not IArgumentListSyntaxAbstraction)
		{
			return new AnalysisFailure("Expected object creation syntax to have an argument list as its second child", Location);
		}

		if (_actualTypeSymbol is null)
		{
			return new AnalysisFailure("Expected object creation syntax to have an actual type symbol", Location);
		}

		if (_convertedTypeSymbol is null)
		{
			return new AnalysisFailure("Expected object creation syntax to have a converted type symbol", Location);
		}

		var typeUnion = new TaggedUnion<ITypeSymbol, Type>(_actualTypeSymbol);
		var objectInstance = new ObjectInstance(
			typeUnion,
			typeUnion,
			Location,
			new ReferenceTypeScope(typeUnion)
			);
		return ImmutableArray.Create((objectInstance as IObjectInstance, state));
	}
}