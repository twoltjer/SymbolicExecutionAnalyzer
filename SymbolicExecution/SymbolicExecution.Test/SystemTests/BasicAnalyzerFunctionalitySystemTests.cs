namespace SymbolicExecution.Test.SystemTests;

public class BasicAnalyzerFunctionalitySystemTests
{
	//No diagnostics expected to show up
	[Fact]
	[Trait("Category", "System")]
	public async Task TestNoAnalysis()
	{
		const string test = @"";

		await VerifyCS.VerifyAnalyzerAsync(test);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestUnmarkedMethodNotAnalyzed()
	{
		const string test = @"using System;

class TestClass
{
	void SayHello()
	{
		throw new InvalidOperationException();
	}
}";

		await VerifyCS.VerifyAnalyzerAsync(test);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeEmptyMethod()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};
class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
	}}
}}
";
		await CSharpAnalyzerVerifier<SymbolicExecutionAnalyzer>.VerifyAnalyzerAsync(source);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestNonCompilingCodeWillNotAnalyze()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};
class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		asdfasdf
	}}
}}
";
		await CSharpAnalyzerVerifier<SymbolicExecutionAnalyzer>.VerifyAnalyzerAsync(
			source,
			DiagnosticResult.CompilerError("CS0103").WithLocation(7, 3),
			DiagnosticResult.CompilerError("CS1002").WithLocation(7, 11));
	}

	/// <summary>
	/// Don't support pointers and fixed statements; produce a warning when they are attempted to be analyzed
	/// </summary>
	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeUnsupportedMethod()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};
using System;

public class Program
{{
	[SymbolicallyAnalyze]
	public static unsafe void Main()
	{{
		var array = new int[] {{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }};
		// pointer to the first element of the array using the fixed statement
		fixed (int* p = array)
		{{
			// pointer to the last element of the array
			int* pLast = p + array.Length - 1;
			// swap the first and the last element
			int temp = *p;
			*p = *pLast;
			*pLast = temp;
		}}

		Console.WriteLine(string.Join("", "", array));
	}}
}}";

		var expected = VerifyCS.Diagnostic(descriptor: AnalysisFailureDiagnosticDescriptor.DiagnosticDescriptor)
			.WithLocation(9, 25)
			.WithMessage("Symbolic Execution Failed: No abstraction for InitializerExpressionSyntax");
		await CSharpAnalyzerVerifier<SymbolicExecutionAnalyzer>.VerifyAnalyzerAsync(source, expected);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestThrowStatementCreatesDiagnostic()
	{
		var test = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};
class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		throw new InvalidOperationException();
	}}
}}
";
		var expected = VerifyCS.Diagnostic(descriptor: MayThrowDiagnosticDescriptor.DiagnosticDescriptor)
			.WithLocation(8, 3)
			.WithMessage("The exception 'InvalidOperationException' may be thrown here and not caught");
		await VerifyCS.VerifyAnalyzerAsync(test, expected);
	}
}