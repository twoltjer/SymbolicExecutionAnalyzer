namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public interface IMethodDeclarationSyntaxAbstraction : IBaseMethodDeclarationSyntaxAbstraction
{
    TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeMethodCall(IAnalysisState priorState, ImmutableArray<IObjectInstance> parameters);
}