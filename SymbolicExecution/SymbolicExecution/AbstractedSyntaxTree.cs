namespace SymbolicExecution;

public class AbstractedSyntaxTree : IAbstractedSyntaxTree
{
	private readonly SemanticModel _semanticModel;
	private ISyntaxNodeAbstraction? _abstraction;

	private readonly Dictionary<SyntaxNode, ISyntaxNodeAbstraction> _abstractionCache =
		new Dictionary<SyntaxNode, ISyntaxNodeAbstraction>();
	private readonly Dictionary<ISyntaxNodeAbstraction, SyntaxNode> _syntaxNodeCache =
		new Dictionary<ISyntaxNodeAbstraction, SyntaxNode>();

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

	public TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure> ProduceAbstraction(SyntaxNode node)
	{
		var childrenOrFailures = node.ChildNodes().Select(ProduceAbstraction).ToImmutableArray();

		if (childrenOrFailures.FirstOrNull(x => !x.IsT1) is TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure> potentialFailure)
		{
			return potentialFailure.T2Value;
		}

		var children = childrenOrFailures.Select(x => x.T1Value).ToImmutableArray();

		var symbol = _semanticModel.GetDeclaredSymbol(node);
		var typeInfo = _semanticModel.GetTypeInfo(node);
		var convertedTypeSymbol = typeInfo.ConvertedType;
		var actualTypeSymbol = typeInfo.Type;
		var location = node.GetLocation();
		var constantValue = _semanticModel.GetConstantValue(node);
		var symbolInfo = _semanticModel.GetSymbolInfo(node);

		var result = node switch
		{
			BlockSyntax => new BlockSyntaxAbstraction(children, symbol, location) as ISyntaxNodeAbstraction,
			IdentifierNameSyntax => new IdentifierNameSyntaxAbstraction(
				children,
				symbol ?? symbolInfo.Symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			UsingDirectiveSyntax => new UsingDirectiveSyntaxAbstraction(children, symbol, location),
			CompilationUnitSyntax => new CompilationUnitSyntaxAbstraction(children, symbol, location),
			QualifiedNameSyntax => new QualifiedNameSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			AttributeSyntax => new AttributeSyntaxAbstraction(children, symbol, location),
			AttributeListSyntax => new AttributeListSyntaxAbstraction(children, symbol, location),
			PredefinedTypeSyntax => new PredefinedTypeSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			ParameterListSyntax => new ParameterListSyntaxAbstraction(children, symbol, location),
			ArgumentListSyntax => new ArgumentListSyntaxAbstraction(children, symbol, location),
			ObjectCreationExpressionSyntax => new ObjectCreationExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			ThrowStatementSyntax => new ThrowStatementSyntaxAbstraction(children, symbol, location),
			MethodDeclarationSyntax => new MethodDeclarationSyntaxAbstraction(children, symbol, location),
			ClassDeclarationSyntax => new ClassDeclarationSyntaxAbstraction(children, symbol, location),
			LiteralExpressionSyntax => new LiteralExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				constantValue,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			ArgumentSyntax => new ArgumentSyntaxAbstraction(children, symbol, location),
			IfStatementSyntax ifStatementSyntax => IfStatementSyntaxAbstraction.BuildFrom(
				_abstractionCache,
				ifStatementSyntax.Condition,
				ifStatementSyntax.Statement,
				ifStatementSyntax.Else,
				children,
				symbol,
				location
				),
			VariableDeclaratorSyntax => new VariableDeclaratorSyntaxAbstraction(
				children,
				symbol,
				location
				),
			VariableDeclarationSyntax => new VariableDeclarationSyntaxAbstraction(
				children,
				symbol,
				location,
				new VariableDeclarationSyntaxAbstractionHelper(location)
				),
			LocalDeclarationStatementSyntax => new LocalDeclarationStatementSyntaxAbstraction(
				children,
				symbol,
				location
				),
			AssignmentExpressionSyntax => new AssignmentExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			ExpressionStatementSyntax => new ExpressionStatementSyntaxAbstraction(
				children,
				symbol,
				location
				),
			EqualsValueClauseSyntax => new EqualsValueClauseSyntaxAbstraction(
				children,
				symbol,
				location
				),
			_ => null,
		};

		if (result == null)
			return new AnalysisFailure($"No abstraction for {node.GetType().Name}", node.GetLocation());

		_abstractionCache[node] = result;
		_syntaxNodeCache[result] = node;
		return new TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure>(result);
	}
}