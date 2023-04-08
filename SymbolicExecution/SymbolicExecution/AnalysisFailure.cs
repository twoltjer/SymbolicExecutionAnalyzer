namespace SymbolicExecution;

/// <summary>
/// Represents an unhanded failure in the analysis engine, often due to an unrecognized node or unsupported operation
/// </summary>
public readonly struct AnalysisFailure
{
	/// <summary>
	/// A description of the failure
	/// </summary>
	public string Reason { get; }
	/// <summary>
	/// The location of the failure
	/// </summary>
	public Location? Location { get; }

	public AnalysisFailure(string reason, Location? location)
	{
		Reason = reason;
		Location = location;
	}

	/// <summary>
	/// Automatically converts an <see cref="AnalysisFailure"/> to a <see cref="Diagnostic"/>
	/// </summary>
	/// <param name="failure">The failure to convert</param>
	/// <returns>A diagnostic representing the failure</returns>
	public static implicit operator Diagnostic(AnalysisFailure failure)
	{
			return Diagnostic.Create(
			AnalysisFailureDiagnosticDescriptor.DiagnosticDescriptor,
			failure.Location,
			failure.Reason);
	}
}