using System.Collections.Generic;

namespace SymbolicExecution;

public interface ISyntaxNodeAbstraction
{
	ImmutableArray<SyntaxNodeAbstraction> Children { get; }
	ISymbol? Symbol { get; }
	IEnumerable<ISyntaxNodeAbstraction> GetDescendantNodes(bool includeSelf);
}