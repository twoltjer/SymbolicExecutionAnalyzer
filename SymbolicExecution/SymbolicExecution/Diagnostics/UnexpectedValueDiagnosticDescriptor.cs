namespace SymbolicExecution.Diagnostics;

internal static class UnexpectedValueDiagnosticDescriptor
{
	private const string DiagnosticId = "SE0002";
	private const string Category = "CodeQuality";
	internal static DiagnosticDescriptor DiagnosticDescriptor { get; } = new DiagnosticDescriptor(
		DiagnosticId,
		title: "Symbolic Execution",
		messageFormat: "There was an unexpected value in the analyzer",
		Category,
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true
		);
}