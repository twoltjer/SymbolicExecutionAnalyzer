namespace SymbolicExecution.Test.Verifiers;

[ExcludeFromCodeCoverage]
public static partial class VisualBasicCodeRefactoringVerifier<TCodeRefactoring>
	where TCodeRefactoring : CodeRefactoringProvider, new()
{
	public class Test : VisualBasicCodeRefactoringTest<TCodeRefactoring, MSTestVerifier>
	{
	}
}