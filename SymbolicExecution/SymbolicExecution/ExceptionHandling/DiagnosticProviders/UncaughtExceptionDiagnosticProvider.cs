namespace SymbolicExecution.ExceptionHandling.DiagnosticProviders;

public class UncaughtExceptionDiagnosticProvider
{
    private const string Category = "Naming";
    
    private UncaughtExceptionDiagnosticProvider()
    {
    }
    
    private readonly LocalizableString _title = new LocalizableResourceString(
        nameof(UncaughtExceptionDiagnosticProviderStrings.AnalyzerTitle),
        UncaughtExceptionDiagnosticProviderStrings.ResourceManager,
        typeof(UncaughtExceptionDiagnosticProviderStrings)
    );

    private readonly LocalizableString _messageFormat = new LocalizableResourceString(
        nameof(UncaughtExceptionDiagnosticProviderStrings.AnalyzerMessageFormat),
        UncaughtExceptionDiagnosticProviderStrings.ResourceManager,
        typeof(UncaughtExceptionDiagnosticProviderStrings)
    );

    private readonly LocalizableString _description = new LocalizableResourceString(
        nameof(UncaughtExceptionDiagnosticProviderStrings.AnalyzerDescription),
        UncaughtExceptionDiagnosticProviderStrings.ResourceManager,
        typeof(UncaughtExceptionDiagnosticProviderStrings)
    );

    private DiagnosticDescriptor Descriptor => new(
        DiagnosticIdProvider.DiagnosticProviderTypeToDiagnosticIdMap[GetType()],
        _title,
        _messageFormat,
        Category,
        DiagnosticSeverity.Warning,
        true,
        _description
    );
    
    private static UncaughtExceptionDiagnosticProvider? _instance;
    
    public static DiagnosticDescriptor CreateDiagnosticDescriptor()
    {
        _instance ??= new UncaughtExceptionDiagnosticProvider();
        return _instance.Descriptor;
    }
}