namespace SymbolicExecution.Test.Verifiers;

public static partial class CSharpAnalyzerVerifier<TAnalyzer>
	where TAnalyzer : DiagnosticAnalyzer, new()
{
	/// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic()" />
	public static DiagnosticResult Diagnostic()
	{
		return CSharpAnalyzerVerifier<TAnalyzer, MSTestVerifier>.Diagnostic();
	}

	/// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(string)" />
	public static DiagnosticResult Diagnostic(string diagnosticId)
	{
		return CSharpAnalyzerVerifier<TAnalyzer, MSTestVerifier>.Diagnostic(diagnosticId);
	}

	/// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)" />
	public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
	{
		return CSharpAnalyzerVerifier<TAnalyzer, MSTestVerifier>.Diagnostic(descriptor);
	}

	/// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])" />
	public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
	{
		var test = new Test
		{
			TestCode = source,
		};
		var nugetFilePath = Path.Combine(
			new DirectoryInfo(Environment.CurrentDirectory).Parent?.Parent?.Parent?.FullName ?? string.Empty,
			"NuGet.Config.Test"
			);
		new FileInfo(nugetFilePath).Exists.Should().BeTrue("NuGet.Config file should exist");
		var testReferenceAssemblies = test.ReferenceAssemblies.WithNuGetConfigFilePath(nugetFilePath).WithPackages(
			new[] { new PackageIdentity("SymbolicExecution.Control", "0.0.2.5") }.ToImmutableArray()
			);
		test.ReferenceAssemblies = testReferenceAssemblies;
		test.ExpectedDiagnostics.AddRange(expected);
		await test.RunAsync(CancellationToken.None);
	}
}