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

public class DemonstrationSystemTests
{
	[Fact]
	[Trait("Category", "System")]
	public async Task TestPascalsTriangleRowPrinting()
	{
		var source = @$"using System;
using {typeof(SymbolicallyAnalyzeAttribute).Namespace};

class TestClass
{{
	[SymbolicallyAnalyze]
	void PrintRowTen()
	{{
		PrintRow(10);
	}}

	[SymbolicallyAnalyze]
	void PrintRowTenThousand()
	{{
		PrintRow(10000);
	}}

	void PrintRow(int n)
	{{
		
	}}

	int[] GetRow(int n)
	{{
		var row = new int[n + 1];
		var previousRow = GetRow(n - 1);
		row[0] = 1;
		row[n] = 1;
		for (int i = 1; i < n; i++)
		{{
			row[i] = previousRow[i - 1] + previousRow[i];
		}}

		return row;
	}}
}}
";

		await VerifyCS.VerifyAnalyzerAsync(source);
	}
}