namespace SymbolicExecution.Test.SystemTests;

public class ConstraintSystemTests
{
    [Fact]
    [Trait("Category", "System")]
    public async Task TestBasicBooleanConstraint()
    {
        var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void SayHello(bool b)
	{{
		bool c = b;
		if (b)
		{{
			if (!c)
			{{
				throw new InvalidOperationException(""Message"");
			}}
		}}
	}}
}}
";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }
}