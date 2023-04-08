namespace SymbolicExecution.Diagnostics;

/// <summary>
/// A diagnostic descriptor for the "MayOverflow" diagnostic. This is used to report a possible overflow in an expression.
/// </summary>
public static class MayOverflowDiagnosticDescriptor
{
    private const string DiagnosticId = "SE0003";
    private const string Category = "CodeQuality";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
    private static readonly LocalizableString _title = new LocalizableResourceString(
        nameof(SymbolicExecutionStrings.AnalyzerTitle),
        SymbolicExecutionStrings.ResourceManager,
        typeof(SymbolicExecutionStrings)
        );

    private static readonly LocalizableString _messageFormat = new LocalizableResourceString(
        nameof(SymbolicExecutionStrings.AnalyzerMayOverflowMessageFormat),
        SymbolicExecutionStrings.ResourceManager,
        typeof(SymbolicExecutionStrings)
        );

    private static readonly LocalizableString _description = new LocalizableResourceString(
        nameof(SymbolicExecutionStrings.AnalyzerDescription),
        SymbolicExecutionStrings.ResourceManager,
        typeof(SymbolicExecutionStrings)
        );

    public static DiagnosticDescriptor DiagnosticDescriptor { get; } = new DiagnosticDescriptor(
        DiagnosticId,
        _title,
        _messageFormat,
        Category,
        DiagnosticSeverity.Warning,
        true,
        _description
        );
}