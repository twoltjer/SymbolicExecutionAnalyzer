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
}