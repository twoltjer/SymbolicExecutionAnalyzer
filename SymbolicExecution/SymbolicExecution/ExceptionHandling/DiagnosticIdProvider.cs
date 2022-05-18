using System;
using System.Collections.Generic;

namespace SymbolicExecution.ExceptionHandling;

public static class DiagnosticIdProvider
{
    public static Dictionary<Type, string> DiagnosticProviderTypeToDiagnosticIdMap { get; } =
        new()
        {

            {
                typeof(DiagnosticProviders.UncaughtExceptionDiagnosticProvider), "SE0001"
            }
        };
}