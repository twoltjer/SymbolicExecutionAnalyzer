namespace SymbolicExecution.Diagnostics;

public static class AnalysisFailureDiagnosticDescriptor
{
	private const string DiagnosticId = "SE0001";
	private const string Category = "CodeQuality";

	// You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
	// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization

	public static DiagnosticDescriptor DiagnosticDescriptor { get; } = new DiagnosticDescriptor(
		DiagnosticId,
		title: "Symbolic Execution",
		messageFormat: "Symbolic Execution Failed: {0}",
		Category,
		DiagnosticSeverity.Warning,
		true,
		description: "Symbolic Execution Failed"
		);
}