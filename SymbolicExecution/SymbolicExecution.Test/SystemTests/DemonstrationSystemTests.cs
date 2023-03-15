namespace SymbolicExecution.Test.SystemTests;

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
    public static void Main()
    {{
        var testClass = new TestClass();
        testClass.PrintRowTen();
        testClass.PrintRowFifty();
    }}
    
    [SymbolicallyAnalyze]
    void PrintRowTen()
    {{
        PrintRow(10);
    }}

    [SymbolicallyAnalyze]
    void PrintRowFifty()
    {{
        PrintRow(50);
    }}

    void PrintRow(int n)
    {{
        Console.WriteLine(string.Join(' ', GetRow(n)));
    }}

    int[] GetRow(int n)
    {{
        var row = new int[n + 1];
        row[0] = 1;
        row[n] = 1;

        if (n <= 1)
            return row;
        
        var previousRow = GetRow(n - 1);

        for (int i = 1; i < n; i++)
        {{
            row[i] = previousRow[i - 1] + previousRow[i];
        }}
        return row;
    }}
}}
";
        var expected = VerifyCS.Diagnostic(descriptor: MayOverflowDiagnosticDescriptor.DiagnosticDescriptor)
            .WithLocation(43, 22)
            .WithMessage("This expression may overflow");
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }
}
