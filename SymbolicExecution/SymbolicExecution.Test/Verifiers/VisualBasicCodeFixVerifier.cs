using Microsoft.CodeAnalysis.CodeFixes;

namespace SymbolicExecution.Test.Verifiers;

[ExcludeFromCodeCoverage]
public static partial class VisualBasicCodeFixVerifier<TAnalyzer, TCodeFix>
	where TAnalyzer : DiagnosticAnalyzer, new()
	where TCodeFix : CodeFixProvider, new()
{
	public class Test : VisualBasicCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier>
	{
	}
}