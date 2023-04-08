namespace SymbolicExecution;

/// <summary>
/// An abstraction of a syntax node from the compiler's syntax tree. These nodes are simplified, use interfaces which
/// allow for mocking, and include information from the semantic model that is needed for symbolic execution.
/// </summary>
public interface ISyntaxNodeAbstraction
{
	/// <summary>
	/// Children node abstractions
	/// </summary>
	ImmutableArray<ISyntaxNodeAbstraction> Children { get; }
	
	/// <summary>
	/// If this node defines a symbol, this is the symbol. Otherwise, this is null.
	/// </summary>
	ISymbol? Symbol { get; }
	
	/// <summary>
	/// A flat list of all descendant nodes, including this node if <paramref name="includeSelf"/> is true.
	/// </summary>
	/// <param name="includeSelf">Whether to include this node in the list</param>
	/// <returns>A flat list of all descendant nodes</returns>
	IEnumerable<ISyntaxNodeAbstraction> GetDescendantNodes(bool includeSelf);
	
	/// <summary>
	/// Analyze the node, returning either a list of result analysis states or an analysis failure.
	/// </summary>
	/// <param name="previous">The previous analysis state</param>
	/// <returns>A list of result analysis states or an analysis failure</returns>
	TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous);
	
	/// <summary>
	/// Get the results of the expression represented by this node, returning either a list of result analysis states
	/// or an analysis failure. This is only valid for nodes that represent expressions.
	/// </summary>
	/// <param name="state">The analysis state</param>
	/// <returns>A list of result analysis states and expression results or an analysis failure</returns>
	TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state);
	
	/// <summary>
	/// The location of the node in the source code.
	/// </summary>
	Location Location { get; }
	
	/// <summary>
	/// A function that returns the parent node abstraction. Returns null if there is no parent node (root node).
	/// </summary>
	Func<ISyntaxNodeAbstraction>? ParentResolver { get; set; }
}