namespace SymbolicExecution.Test.SystemTests;

public class IfConditionSystemTests
{
	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeIfLiteralFalse()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};
using System;

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		if (false)
			throw new InvalidOperationException(""ExceptionMessage"");
	}}
}}
";
		await VerifyCS.VerifyAnalyzerAsync(source);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeIfLiteralTrue()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};
using System;

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		if (true)
			throw new InvalidOperationException(""ExceptionMessage"");
	}}
}}
";
		var expected = VerifyCS.Diagnostic(descriptor: MayThrowDiagnosticDescriptor.DiagnosticDescriptor)
			.WithLocation(10, 4)
			.WithMessage("The exception 'InvalidOperationException' may be thrown here and not caught");
		await CSharpAnalyzerVerifier<SymbolicExecutionAnalyzer>.VerifyAnalyzerAsync(source, expected);
	}
}