namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ObjectCreationExpressionSyntaxAbstraction : BaseObjectCreationExpressionSyntaxAbstraction, IObjectCreationExpressionSyntaxAbstraction
{
	private readonly Location _location;
	private readonly ITypeSymbol? _actualTypeSymbol;
	private readonly ITypeSymbol? _convertedTypeSymbol;

	public ObjectCreationExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol,
		Location location, ITypeSymbol? actualTypeSymbol, ITypeSymbol? convertedTypeSymbol) : base(children, symbol)
	{
		_location = location;
		_actualTypeSymbol = actualTypeSymbol;
		_convertedTypeSymbol = convertedTypeSymbol;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		if (Children.Length != 2)
		{
			return new AnalysisFailure("Expected object creation syntax to have two children (type and argument list)", _location);
		}

		if (Children[0] is not IIdentifierNameSyntaxAbstraction)
		{
			return new AnalysisFailure("Expected object creation syntax to have an identifier name as its first child", _location);
		}
		
		if (Children[1] is not IArgumentListSyntaxAbstraction)
		{
			return new AnalysisFailure("Expected object creation syntax to have an argument list as its second child", _location);
		}

		if (_actualTypeSymbol is null)
		{
			return new AnalysisFailure("Expected object creation syntax to have an actual type symbol", _location);
		}
		
		if (_convertedTypeSymbol is null)
		{
			return new AnalysisFailure("Expected object creation syntax to have a converted type symbol", _location);
		}

		var objectInstance = new ObjectInstance(_actualTypeSymbol, _convertedTypeSymbol, _location);
		return objectInstance;
	}
}