namespace SymbolicExecution.Test.Verifiers;

[ExcludeFromCodeCoverage]
public static partial class VisualBasicAnalyzerVerifier<TAnalyzer>
	where TAnalyzer : DiagnosticAnalyzer, new()
{
	public class Test : VisualBasicAnalyzerTest<TAnalyzer, MSTestVerifier>
	{
	}
}