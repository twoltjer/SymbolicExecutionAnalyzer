using SymbolicExecution.Control;
using SymbolicExecution.Diagnostics;
using SymbolicExecution.Test.Verifiers;

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
			.WithLocation(9, 23)
			.WithMessage("Symbolic Execution Failed: No abstraction for OmittedArraySizeExpressionSyntax");
		await CSharpAnalyzerVerifier<SymbolicExecutionAnalyzer>.VerifyAnalyzerAsync(source, expected);
	}

	[Fact]
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
//
// 	[Fact]
// 	public async Task TestIfTrueCondition()
// 	{
// 		var test = @"using System;
//
// class SymbolicallyAnalyzeAttribute : Attribute { }
//
// class TestClass
// {
// 	[SymbolicallyAnalyze]
// 	void SayHello()
// 	{
// 		if (true)
// 		{
// 			throw new InvalidOperationException();
// 		}
// 	}
// }";
// 		var expected = VerifyCS.Diagnostic(MayThrowDiagnosticDescriptor.DiagnosticDescriptor)
// 			.WithLocation(12, 4)
// 			.WithMessage("The statement 'throw new InvalidOperationException();' may throw unhandled exceptions");
// 		await VerifyCS.VerifyAnalyzerAsync(test, expected);
// 	}
//
// 	[Fact]
// 	public async Task TestNotTrueIntConditionStopsAnalysis()
// 	{
// 		var test = @"using System;
//
// class SymbolicallyAnalyzeAttribute : Attribute { }
//
// class TestClass
// {
// 	[SymbolicallyAnalyze]
// 	void SayHello()
// 	{
// 		int x = 0;
// 		if (x != 0)
// 		{
// 			throw new InvalidOperationException();
// 		}
// 	}
// }";
// 		await VerifyCS.VerifyAnalyzerAsync(test);
// 	}
//
// 	[Fact]
// 	public async Task TestSurelyTrueConditionContinuesAnalysis()
// 	{
// 		var test = @"using System;
//
// class SymbolicallyAnalyzeAttribute : Attribute { }
//
// class TestClass
// {
// 	[SymbolicallyAnalyze]
// 	void SayHello()
// 	{
// 		int x = 0;
// 		if (x == 0)
// 		{
// 			throw new InvalidOperationException();
// 		}
// 	}
// }";
// 		var expected = VerifyCS.Diagnostic(diagnosticId: "SE0001")
// 			.WithLocation(13, 4)
// 			.WithMessage("The statement 'throw new InvalidOperationException();' may throw unhandled exceptions");
// 		await VerifyCS.VerifyAnalyzerAsync(test, expected);
// 	}
//
// 	[Fact]
// 	public async Task TestInaccessibleCodeDoesNotCreateDiagnostic()
// 	{
// 		var test = @"using System;
//
// class SymbolicallyAnalyzeAttribute : Attribute { }
//
// class TestClass
// {
// 	[SymbolicallyAnalyze]
// 	void SayHello()
// 	{
// 		if (false)
// 		{
// 			throw new InvalidOperationException();
// 		}
// 	}
// }";
// 		await VerifyCS.VerifyAnalyzerAsync(test);
// 	}
//
// 	[Fact]
// 	public async Task TestAnalyzeMethodThatDoesNotCompile()
// 	{
// 		var test = @"using System;
//
// class SymbolicallyAnalyzeAttribute : Attribute { }
//
// class TestClass
// {
// 	[SymbolicallyAnalyze]
// 	void SayHello()
// 	{
// 		row new InvalidOperationException();th
// 	}
// }";
// 		await VerifyCS.VerifyAnalyzerAsync(
// 			test,
// 			DiagnosticResult.CompilerError("CS0103").WithLocation(10, 3).WithMessage("The name 'row' does not exist in the current context"),
// 			DiagnosticResult.CompilerError("CS1002").WithLocation(10, 7).WithMessage("; expected"),
// 			DiagnosticResult.CompilerError("CS0103").WithLocation(10, 39).WithMessage("The name 'th' does not exist in the current context"),
// 			DiagnosticResult.CompilerError("CS1002").WithLocation(10, 41).WithMessage("; expected")
// 			);
// 	}
//
// 	[Fact(Skip="Unimplemented features")]
// 	public async Task TestAnalyzeGetMinMax()
// 	{
// 		var test = @"using System;
//
// class SymbolicallyAnalyzeAttribute : Attribute { }
//
// class TestClass
// {
// 	[SymbolicallyAnalyze]
// 	public static (float min, float max) GetMinMax(IEnumerable<float> data)
// 	{
// 		var (min, max) = (float.MaxValue, float.MinValue);
// 		foreach (var item in data)
// 		{
// 			if (Math.Abs(item) > MaxRange)
// 				continue;
//
// 			min = Math.Min(min, item);
// 			max = Math.Max(max, item);
// 		}
//
// 		return (min, max);
// 	}
// }";
// 		await VerifyCS.VerifyAnalyzerAsync(test);
// 	}
}