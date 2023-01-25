﻿using SymbolicExecution.Control;
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
		var test = @"";

		await VerifyCS.VerifyAnalyzerAsync(test);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestUnmarkedMethodNotAnalyzed()
	{
		var test = @"using System;

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
			.WithLocation(10, 3)
			.WithMessage("The statement 'throw new InvalidOperationException();' may throw unhandled exceptions");
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