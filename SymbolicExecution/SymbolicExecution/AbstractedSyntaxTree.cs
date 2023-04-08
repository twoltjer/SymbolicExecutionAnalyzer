namespace SymbolicExecution;

/// <summary>
/// Abstracts a compiler-provided syntax tree into a simpler syntax tree that is easier to work with and has mockable interfaces
/// </summary>
public class AbstractedSyntaxTree : IAbstractedSyntaxTree
{
	private readonly SemanticModel _semanticModel;
	private ISyntaxNodeAbstraction? _abstraction;

	private readonly Dictionary<SyntaxNode, ISyntaxNodeAbstraction> _abstractionCache =
		new Dictionary<SyntaxNode, ISyntaxNodeAbstraction>();
	private readonly Dictionary<ISyntaxNodeAbstraction, SyntaxNode> _syntaxNodeCache =
		new Dictionary<ISyntaxNodeAbstraction, SyntaxNode>();

	/// <summary>
	/// Creates a new abstracted syntax tree object, without actually converting a syntax tree. Holds on to a semantic
	/// model that will be used in converting syntax nodes and extracting relevant information for symbolic execution.
	/// </summary>
	/// <param name="semanticModel">A semantic model</param>
	public AbstractedSyntaxTree(SemanticModel semanticModel)
	{
		_semanticModel = semanticModel;
	}
	
	/// <summary>
	/// Tries to get the abstracted syntax node for a given syntax node. Returns true if the syntax node was found,
	/// false otherwise.
	/// </summary>
	/// <param name="syntaxNode">A syntax node</param>
	/// <param name="abstraction">The corresponding syntax node abstraction</param>
	/// <returns>True if the syntax node was found, false otherwise</returns>
	public bool TryGetAbstractedSyntaxNode(SyntaxNode syntaxNode, out ISyntaxNodeAbstraction abstraction)
	{
		return _abstractionCache.TryGetValue(syntaxNode, out abstraction);
	}

	/// <summary>
	/// Finds the root syntax node abstraction for the syntax tree. Returns an analysis failure if the root node
	/// cannot be determined.
	/// </summary>
	/// <returns>The root syntax node abstraction</returns>
	public TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure> GetRoot()
	{
		if (_abstraction == null)
		{
			var abstractionOrFailure = ProduceAbstraction(
				_semanticModel.SyntaxTree.GetRoot(),
				resolveNodeParent: null
				);
			
			if (abstractionOrFailure.IsT1)
				_abstraction = abstractionOrFailure.T1Value;
			else
				return abstractionOrFailure.T2Value;
		}
		return new TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure>(_abstraction);
	}

	/// <summary>
	/// Produces an abstraction for a given syntax node and (recursively) all of its children. Returns an analysis
	/// failure if an abstraction couldn't be produced (probably due to an unsupported syntax node type).
	/// </summary>
	/// <param name="node">A syntax node</param>
	/// <param name="resolveNodeParent">A function that resolves the parent syntax node abstraction</param>
	/// <returns>The syntax node abstraction</returns>
	public TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure> ProduceAbstraction(SyntaxNode node, Func<ISyntaxNodeAbstraction>? resolveNodeParent)
	{
		var childrenOrFailures = node.ChildNodes()
			.Select(child => ProduceAbstraction(child, resolveNodeParent: () => _abstractionCache[node]))
			.ToImmutableArray();

		if (childrenOrFailures.FirstOrNull(x => !x.IsT1) is { } potentialFailure)
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
			InvocationExpressionSyntax => new InvocationExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol,
				symbolInfo
				),
			ParameterSyntax => new ParameterSyntaxAbstraction(
				children,
				symbol,
				location
				),
			ArrayRankSpecifierSyntax => new ArrayRankSpecifierSyntaxAbstraction(
				children,
				symbol,
				location
				),
			ArrayTypeSyntax => new ArrayTypeSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			BinaryExpressionSyntax binaryExpressionSyntax => new BinaryExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol,
				binaryExpressionSyntax.Kind()
				),	
			ArrayCreationExpressionSyntax => new ArrayCreationExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			BracketedArgumentListSyntax => new BracketedArgumentListSyntaxAbstraction(
				children,
				symbol,
				location
				),
			ElementAccessExpressionSyntax => new ElementAccessExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax => new PostfixUnaryExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol,
				postfixUnaryExpressionSyntax.Kind()
				),
			ForStatementSyntax => new ForStatementSyntaxAbstraction(
				children,
				symbol,
				location
				),
			ReturnStatementSyntax => new ReturnStatementSyntaxAbstraction(
				children,
				symbol,
				location
				),
			MemberAccessExpressionSyntax => new MemberAccessExpressionSyntaxAbstraction(
				children,
				symbol,
				location,
				actualTypeSymbol,
				convertedTypeSymbol
				),
			FileScopedNamespaceDeclarationSyntax => new FileScopedNamespaceDeclarationSyntaxAbstraction(
				children,
				symbol,
				location
				),
			_ => null,
		};

		if (result == null)
		{
			// Internal types that are not public in Roslyn, so we check by name
			var nodeTypeName = node.GetType().Name;
			if (nodeTypeName == "OmittedArraySizeExpressionSyntax")
			{
				return new OmittedArraySizeExpressionSyntaxAbstraction(
					children,
					symbol,
					location,
					actualTypeSymbol,
					convertedTypeSymbol
					);
			}
			if (nodeTypeName == "SimpleMemberAccessExpressionSyntax")
			{
				return new SimpleMemberAccessExpressionSyntaxAbstraction(
					children,
					symbol,
					location,
					actualTypeSymbol,
					convertedTypeSymbol
					);
			}
			return new AnalysisFailure($"No abstraction for {nodeTypeName}", node.GetLocation());
		}

		result.ParentResolver = resolveNodeParent;
		_abstractionCache[node] = result;
		_syntaxNodeCache[result] = node;
		return new TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure>(result);
	}
}