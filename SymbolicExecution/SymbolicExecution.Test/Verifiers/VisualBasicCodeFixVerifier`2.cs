﻿using Microsoft.CodeAnalysis.CodeFixes;

namespace SymbolicExecution.Test.Verifiers;

public static partial class VisualBasicCodeFixVerifier<TAnalyzer, TCodeFix>
	where TAnalyzer : DiagnosticAnalyzer, new()
	where TCodeFix : CodeFixProvider, new()
{
	/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic()" />
	public static DiagnosticResult Diagnostic()
	{
		return VisualBasicCodeFixVerifier<TAnalyzer, TCodeFix, MSTestVerifier>.Diagnostic();
	}

	/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(string)" />
	public static DiagnosticResult Diagnostic(string diagnosticId)
	{
		return VisualBasicCodeFixVerifier<TAnalyzer, TCodeFix, MSTestVerifier>.Diagnostic(diagnosticId);
	}

	/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)" />
	public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
	{
		return VisualBasicCodeFixVerifier<TAnalyzer, TCodeFix, MSTestVerifier>.Diagnostic(descriptor);
	}

	/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])" />
	public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
	{
		var test = new Test
		{
			TestCode = source,
		};

		test.ExpectedDiagnostics.AddRange(expected);
		await test.RunAsync(CancellationToken.None);
	}

	/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, string)" />
	public static async Task VerifyCodeFixAsync(string source, string fixedSource)
	{
		await VerifyCodeFixAsync(source, DiagnosticResult.EmptyDiagnosticResults, fixedSource);
	}

	/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult, string)" />
	public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
	{
		await VerifyCodeFixAsync(source, new[] { expected }, fixedSource);
	}

	/// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult[], string)" />
	public static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource)
	{
		var test = new Test
		{
			TestCode = source,
			FixedCode = fixedSource,
		};

		test.ExpectedDiagnostics.AddRange(expected);
		await test.RunAsync(CancellationToken.None);
	}
}