namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public interface IVariableDeclarationSyntaxAbstractionHelper
{
	TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNodeWithNoDeclaratorChild(
		IAnalysisState previous,
		ILocalSymbol localSymbol
		);

	TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNodeWithOneDeclaratorChild(IAnalysisState previous, ILocalSymbol localSymbol, ISyntaxNodeAbstraction declaratorChild);
}