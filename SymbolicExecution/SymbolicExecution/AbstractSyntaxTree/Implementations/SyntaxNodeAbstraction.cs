namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class SyntaxNodeAbstraction : ISyntaxNodeAbstraction
{
	public SyntaxNodeAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol)
	{
		Children = children;
		Symbol = symbol;
	}

	public ImmutableArray<SyntaxNodeAbstraction> Children { get; }
	public IEnumerable<ISyntaxNodeAbstraction> GetDescendantNodes(bool includeSelf)
	{
		foreach (var child in Children)
		{
			yield return child;

			foreach (var descendant in child.GetDescendantNodes(includeSelf: false))
			{
				yield return descendant;
			}
		}

		if (includeSelf)
		{
			yield return this;
		}
	}

	public ISymbol? Symbol { get; }
}