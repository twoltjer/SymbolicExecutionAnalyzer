namespace SymbolicExecution;

public interface IAnalysisState
{
	IExceptionThrownState? CurrentException { get; }
	bool IsReachable { get; }
	IAnalysisState ThrowException(IObjectInstance exception, Location location);
	IAnalysisState AddLocalVariable(string variableName, ITypeSymbol variableType);
}