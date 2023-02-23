namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class VariableDeclarationSyntaxAbstractionHelper : IVariableDeclarationSyntaxAbstractionHelper
{
	private readonly Location _location;

	public VariableDeclarationSyntaxAbstractionHelper(Location location)
	{
		_location = location;
	}

	public TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNodeWithNoDeclaratorChild(
		IAnalysisState previous,
		ILocalSymbol localSymbol
		)
	{
		var stateWithVar = previous.AddLocalVariable(localSymbol);
		return new[] { stateWithVar };
	}

	public TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNodeWithOneDeclaratorChild(
		IAnalysisState previous,
		ILocalSymbol localSymbol,
		ISyntaxNodeAbstraction declaratorChild
		)
	{
		if (declaratorChild is not IEqualsValueClauseSyntaxAbstraction equalsValueClauseSyntaxAbstraction)
		{
			return new AnalysisFailure("Variable declarator must have an equals value clause as its child", _location);
		}

		var equalsValueClauseChildren = equalsValueClauseSyntaxAbstraction.Children;
		if (equalsValueClauseChildren.Length != 1)
		{
			return new AnalysisFailure("Equals value clause must have exactly one child", _location);
		}
		
		if (equalsValueClauseChildren[0] is not IExpressionSyntaxAbstraction expressionSyntaxAbstraction)
		{
			return new AnalysisFailure("Equals value clause must have an expression as its child", _location);
		}

		var valueRefsOrFailure = expressionSyntaxAbstraction.GetResults(previous);
		if (!valueRefsOrFailure.IsT1)
			return valueRefsOrFailure.T2Value;

		var valueResults = valueRefsOrFailure.T1Value;
		var returnStates = new List<IAnalysisState>(valueResults.Length);
		foreach (var (value, state) in valueResults)
		{
			var stateWithVar = state.AddLocalVariable(localSymbol);
			var result = stateWithVar.SetSymbolReference(localSymbol, value);
			if (result.IsT1)
				returnStates.Add(result.T1Value);
			else
				return result.T2Value;
		}

		return returnStates;
	}
}