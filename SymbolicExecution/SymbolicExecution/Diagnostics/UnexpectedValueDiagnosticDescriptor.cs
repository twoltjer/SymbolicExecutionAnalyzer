namespace SymbolicExecution.Diagnostics;

internal static class UnexpectedValueDiagnosticDescriptor
{
	private const string DiagnosticId = "SE0001";
	private const string Category = "Naming";

	// You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
	// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
	private static readonly LocalizableString _title = new LocalizableResourceString(
		nameof(SymbolicExecutionStrings.AnalyzerTitle),
		SymbolicExecutionStrings.ResourceManager,
		typeof(SymbolicExecutionStrings)
		);

	private static readonly LocalizableString _messageFormat = new LocalizableResourceString(
		nameof(SymbolicExecutionStrings.AnalyzerMessageFormat),
		SymbolicExecutionStrings.ResourceManager,
		typeof(SymbolicExecutionStrings)
		);

	private static readonly LocalizableString _description = new LocalizableResourceString(
		nameof(SymbolicExecutionStrings.AnalyzerDescription),
		SymbolicExecutionStrings.ResourceManager,
		typeof(SymbolicExecutionStrings)
		);

	internal static DiagnosticDescriptor DiagnosticDescriptor { get; } = new DiagnosticDescriptor(
		DiagnosticId,
		_title,
		_messageFormat,
		Category,
		DiagnosticSeverity.Warning,
		true,
		_description
		);
}