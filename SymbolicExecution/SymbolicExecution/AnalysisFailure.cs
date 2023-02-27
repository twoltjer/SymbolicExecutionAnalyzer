namespace SymbolicExecution;

public readonly struct AnalysisFailure
{
	public string Reason { get; }
	public Location? Location { get; }

	public AnalysisFailure(string reason, Location? location)
	{
		if (Debugger.IsAttached)
			Debugger.Break();	

		Reason = reason;
		Location = location;
	}

	public static implicit operator Diagnostic(AnalysisFailure failure)
	{
			return Diagnostic.Create(
			AnalysisFailureDiagnosticDescriptor.DiagnosticDescriptor,
			failure.Location,
			failure.Reason);
	}
}