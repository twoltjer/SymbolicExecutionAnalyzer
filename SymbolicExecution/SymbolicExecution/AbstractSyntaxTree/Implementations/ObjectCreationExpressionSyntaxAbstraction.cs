namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ObjectCreationExpressionSyntaxAbstraction : BaseObjectCreationExpressionSyntaxAbstraction, IObjectCreationExpressionSyntaxAbstraction
{
	private readonly ITypeSymbol? _actualTypeSymbol;
	private readonly ITypeSymbol? _convertedTypeSymbol;

	public ObjectCreationExpressionSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol,
		Location location, ITypeSymbol? actualTypeSymbol, ITypeSymbol? convertedTypeSymbol) : base(children, symbol, location)
	{
		_actualTypeSymbol = actualTypeSymbol;
		_convertedTypeSymbol = convertedTypeSymbol;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Object creation syntax should not be traversed, but evaluated as an expression", Location);
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
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

		var objectInstance = new ObjectInstance(_actualTypeSymbol, _convertedTypeSymbol, Location);
		return objectInstance;
	}
}