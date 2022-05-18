using System;

namespace SymbolicExecution.ExceptionHandling;

internal class ExceptionStatementExceptionHandler : ExceptionHandlerBase<ExceptionStatementException>
{
    public override void Handle(ExceptionStatementException exception)
    {
        var statement = exception.ThrowStatement;
        var diagnostic = UncaughtExceptionDiagnosticProvider.CreateDiagnosticDescriptor()
        codeBlockContext.ReportDiagnostic(diagnostic);
    }
}