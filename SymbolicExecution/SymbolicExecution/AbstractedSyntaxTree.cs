namespace SymbolicExecution;

public class AbstractedSyntaxTree : IAbstractedSyntaxTree
{
	private readonly SemanticModel _semanticModel;
	private ISyntaxNodeAbstraction? _abstraction;

	public AbstractedSyntaxTree(SemanticModel semanticModel)
	{
		_semanticModel = semanticModel;
	}

	public TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure> GetRoot()
	{
		if (_abstraction == null)
		{
			var abstractionOrFailure = ProduceAbstraction(
				_semanticModel.SyntaxTree.GetRoot()
				);
			if (abstractionOrFailure.IsT1)
				_abstraction = abstractionOrFailure.T1Value;
			else
				return abstractionOrFailure.T2Value;
		}
		return new TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure>(_abstraction);
	}

	public TaggedUnion<SyntaxNodeAbstraction, AnalysisFailure> ProduceAbstraction(SyntaxNode node)
	{
		var childrenOrFailures = node.ChildNodes().Select(ProduceAbstraction).ToImmutableArray();

		if (childrenOrFailures.FirstOrNull(x => !x.IsT1) is TaggedUnion<SyntaxNodeAbstraction, AnalysisFailure> potentialFailure)
		{
			return potentialFailure.T2Value;
		}

		var children = childrenOrFailures.Select(x => x.T1Value as ISyntaxNodeAbstraction).ToImmutableArray();

		var symbol = _semanticModel.GetDeclaredSymbol(node);
		var typeInfo = _semanticModel.GetTypeInfo(node);
		var convertedTypeSymbol = typeInfo.ConvertedType;
		var actualTypeSymbol = typeInfo.Type;
		var location = node.GetLocation();

		var result = node switch
		{
			BlockSyntax => new BlockSyntaxAbstraction(children, symbol, location),
			IdentifierNameSyntax => new IdentifierNameSyntaxAbstraction(children, symbol, location),
			UsingDirectiveSyntax => new UsingDirectiveSyntaxAbstraction(children, symbol, location),
			CompilationUnitSyntax => new CompilationUnitSyntaxAbstraction(children, symbol, location),
			QualifiedNameSyntax => new QualifiedNameSyntaxAbstraction(children, symbol, location),
			AttributeSyntax => new AttributeSyntaxAbstraction(children, symbol, location),
			AttributeListSyntax => new AttributeListSyntaxAbstraction(children, symbol, location),
			PredefinedTypeSyntax => new PredefinedTypeSyntaxAbstraction(children, symbol, location),
			ParameterListSyntax => new ParameterListSyntaxAbstraction(children, symbol, location),
			ArgumentListSyntax => new ArgumentListSyntaxAbstraction(children, symbol, location),
			ObjectCreationExpressionSyntax => new ObjectCreationExpressionSyntaxAbstraction(children, symbol, location, actualTypeSymbol, convertedTypeSymbol),
			ThrowStatementSyntax => new ThrowStatementSyntaxAbstraction(children, symbol, location),
			MethodDeclarationSyntax => new MethodDeclarationSyntaxAbstraction(children, symbol, location),
			ClassDeclarationSyntax => new ClassDeclarationSyntaxAbstraction(children, symbol, location) as SyntaxNodeAbstraction,
			_ => null,
		};

		if (result == null)
			return new AnalysisFailure($"No abstraction for {node.GetType().Name}", node.GetLocation());

		return result;
	}
}