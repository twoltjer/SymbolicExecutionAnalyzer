﻿using System.Threading.Tasks;
using Xunit;
using VerifyCS = SymbolicExecution.Test.CSharpCodeFixVerifier<
	SymbolicExecution.SymbolicExecutionAnalyzer,
	SymbolicExecution.SymbolicExecutionCodeFixProvider>;

namespace SymbolicExecution.Test;

public class SymbolicExecutionAnalyzerTests
{
	//No diagnostics expected to show up
	[Fact]
	public async Task TestNoAnalysis()
	{
		var test = @"";

		await VerifyCS.VerifyAnalyzerAsync(test);
	}

	[Fact]
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
	public async Task TestAnalyzeEmptyMethod()
	{
		var test = @"using System;

class SymbolicallyAnalyzeAttribute : Attribute { }

class TestClass
{
	[SymbolicallyAnalyze]
	void SayHello()
	{
	}
}";

		await VerifyCS.VerifyAnalyzerAsync(test);
	}

	[Fact]
	public async Task TestThrowStatementCreatesDiagnostic()
	{
		var test = @"using System;

class SymbolicallyAnalyzeAttribute : Attribute { }

class TestClass
{
	[SymbolicallyAnalyze]
	void SayHello()
	{
		throw new InvalidOperationException();
	}
}";
		var expected = VerifyCS.Diagnostic(diagnosticId: "SymbolicExecution").WithLocation(10, 3);
		await VerifyCS.VerifyAnalyzerAsync(test, expected);
	}

	[Fact]
	public async Task TestInaccessibleCodeDoesNotCreateDiagnostic()
	{
		var test = @"using System;

class SymbolicallyAnalyzeAttribute : Attribute { }

class TestClass
{
	[SymbolicallyAnalyze]
	void SayHello()
	{
		if (false)
		{
			throw new InvalidOperationException();
		}
	}
}";
		await VerifyCS.VerifyAnalyzerAsync(test);
	}

	//Diagnostic and CodeFix both triggered and checked for
	[Fact(Skip = "Not testing code fixes")]
	public async Task TestCodeFix()
	{
		var test = @"
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Diagnostics;

	namespace ConsoleApplication1
	{
		class {|#0:TypeName|}
		{   
		}
	}";

		var fixtest = @"
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Diagnostics;

	namespace ConsoleApplication1
	{
		class TYPENAME
		{   
		}
	}";

		var expected = VerifyCS.Diagnostic(diagnosticId: "BraceAnalyzer").WithLocation(0).WithArguments("TypeName");
		await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
	}
}