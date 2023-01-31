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
			var abstractionOrFailure = ProduceAbstraction<SyntaxNodeAbstraction, SyntaxNode>(
				_semanticModel,
				_semanticModel.SyntaxTree.GetRoot()
				);
			if (abstractionOrFailure.IsT1)
				_abstraction = abstractionOrFailure.T1Value;
			else
				return abstractionOrFailure.T2Value;
		}
		return new TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure>(_abstraction);
	}

	public TaggedUnion<TAbstractedType, AnalysisFailure> ProduceAbstraction<TAbstractedType, TCompileType>(SemanticModel model, TCompileType node)
		where TAbstractedType : SyntaxNodeAbstraction
		where TCompileType : SyntaxNode
	{
		var childrenOrFailures = node.ChildNodes().Select(x => ProduceAbstraction<SyntaxNodeAbstraction, SyntaxNode>(model, x)).ToImmutableArray();

		if (childrenOrFailures.FirstOrNull(x => !x.IsT1) is TaggedUnion<SyntaxNodeAbstraction, AnalysisFailure> potentialFailure)
		{
			return potentialFailure.T2Value;
		}

		var children = childrenOrFailures.Select(x => x.T1Value as ISyntaxNodeAbstraction).ToImmutableArray();

		var symbol = model.GetDeclaredSymbol(node);
		var typeInfo = model.GetTypeInfo(node);
		var convertedTypeSymbol = typeInfo.ConvertedType;
		var actualTypeSymbol = typeInfo.Type;
		var location = node.GetLocation();

		var result = node switch
		{
			BlockSyntax => new BlockSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			IdentifierNameSyntax => new IdentifierNameSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			UsingDirectiveSyntax => new UsingDirectiveSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			CompilationUnitSyntax => new CompilationUnitSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			QualifiedNameSyntax => new QualifiedNameSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			AttributeSyntax => new AttributeSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			AttributeListSyntax => new AttributeListSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			PredefinedTypeSyntax => new PredefinedTypeSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			ParameterListSyntax => new ParameterListSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			ArgumentListSyntax => new ArgumentListSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			ObjectCreationExpressionSyntax => new ObjectCreationExpressionSyntaxAbstraction(children, symbol, location, actualTypeSymbol, convertedTypeSymbol) as TAbstractedType,
			ThrowStatementSyntax => new ThrowStatementSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			MethodDeclarationSyntax => new MethodDeclarationSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			ClassDeclarationSyntax => new ClassDeclarationSyntaxAbstraction(children, symbol, location) as TAbstractedType,
			_ => null,
		};

		if (result == null)
			return new AnalysisFailure($"No abstraction for {node.GetType().Name}", node.GetLocation());

		return result;
	}
}