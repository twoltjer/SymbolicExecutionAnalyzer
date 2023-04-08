namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public interface IMethodDeclarationSyntaxAbstraction : IBaseMethodDeclarationSyntaxAbstraction
{
    TaggedUnion<IEnumerable<(IObjectInstance? returned, IAnalysisState state)>, AnalysisFailure> AnalyzeMethodCall(IAnalysisState priorState, ImmutableArray<IObjectInstance> parameters);
}