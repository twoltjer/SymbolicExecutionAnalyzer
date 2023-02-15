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

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalLong_BinaryGreaterThanAndLessThan_True()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		var i = 1L;
		if (i > 0 && i < 2)
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
	public async Task TestAnalyzeLocalLong_BinaryGreaterThanAndLessThan_False()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		long i = 1;
		if (i > 2 && i < 3)
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
	public async Task TestAnalyzeLocalChar_BinaryGeqAndLeq_True()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		char i = 'a';
		char j = 'b';
		char k = 'z';
		if (i >= 'a' && i <= 'z' && j >= 'a' && j <= 'z' && k >= 'a' && k <= 'z')
		{{
			throw new InvalidOperationException(""Message"");
		}}
	}}
}}
";
		var expected = VerifyCS.Diagnostic(descriptor: MayThrowDiagnosticDescriptor.DiagnosticDescriptor)
			.WithLocation(14, 4)
			.WithMessage("The exception 'InvalidOperationException' may be thrown here and not caught");
		await VerifyCS.VerifyAnalyzerAsync(source, expected);
	}

	[Fact]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalByte_BinaryBitXor_True()
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

	[Fact(Skip = "Not yet implemented")]
	[Trait("Category", "System")]
	public async Task TestAnalyzeLocalByte_BinaryBitXor_False()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello()
	{{
		byte b = 0b0101;
		byte c = 0b0011;
		if ((b ^ c) == 0b0110)
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
}