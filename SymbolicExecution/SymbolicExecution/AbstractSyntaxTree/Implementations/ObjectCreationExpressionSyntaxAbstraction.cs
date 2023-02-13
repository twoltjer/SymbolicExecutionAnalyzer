namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ObjectCreationExpressionSyntaxAbstraction : BaseObjectCreationExpressionSyntaxAbstraction, IObjectCreationExpressionSyntaxAbstraction
{
	public ObjectCreationExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
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

		if (_type is null)
		{
			return new AnalysisFailure("Expected object creation syntax to have an actual type symbol", Location);
		}


		var objectInstance = new ObjectInstance(
			new TaggedUnion<ITypeSymbol, Type>(_type),
			Location,
			new ReferenceTypeScope(_type)
			);
		return ImmutableArray.Create((objectInstance as IObjectInstance, state));
	}
}