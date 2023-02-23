namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public interface IExpressionSyntaxAbstraction : IExpressionOrPatternSyntaxAbstraction
{
    TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state);
}