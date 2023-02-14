namespace SymbolicExecution.Test.SystemTests;

public class LocalVariableSystemTests
{
	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalVariableDeclarationWithNoDefinition()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		bool b;
	}}
}}
";
		await VerifyCS.VerifyAnalyzerAsync(source);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalVariableDeclarationThenDefinition()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		bool b;
		b = true;
	}}
}}
";
		await VerifyCS.VerifyAnalyzerAsync(source);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalVariableDeclarationAndDefinition()
	{
		var source = @$"using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		bool b = true;
	}}
}}
";
		await VerifyCS.VerifyAnalyzerAsync(source);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalVariableAsCondition_True()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		bool b = true;
		if (b)
		{{
			throw new InvalidOperationException(""Message"");
		}}
	}}
}}
";

		var expected = VerifyCS.Diagnostic(descriptor: MayThrowDiagnosticDescriptor.DiagnosticDescriptor)
			.WithLocation(12, 4)
			.WithMessage("The exception 'InvalidOperationException' may be thrown here and not caught");
		await CSharpAnalyzerVerifier<SymbolicExecutionAnalyzer>.VerifyAnalyzerAsync(source, expected);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalVariableAsCondition_False()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		bool b = false;
		if (b)
		{{
			throw new InvalidOperationException(""Message"");
		}}
	}}
}}
";

		await VerifyCS.VerifyAnalyzerAsync(source);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalInteger_BinaryEquality_True()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		int i = 1;
		if (i == 1)
		{{
			throw new InvalidOperationException(""Message"");
		}}
	}}
}}
";
		var expected = VerifyCS.Diagnostic(descriptor: MayThrowDiagnosticDescriptor.DiagnosticDescriptor)
			.WithLocation(12, 4)
			.WithMessage("The exception 'InvalidOperationException' may be thrown here and not caught");
		await VerifyCS.VerifyAnalyzerAsync(source, expected);
	}
	
	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalInteger_BinaryEquality_False()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		int i = 1;
		if (i == 2)
		{{
			throw new InvalidOperationException(""Message"");
		}}
	}}
}}
";
		await VerifyCS.VerifyAnalyzerAsync(source);
	}
}