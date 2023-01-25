namespace SymbolicExecution;

public readonly struct AnalysisFailure
{
	public string Reason { get; }
	public Location? Location { get; }

	public AnalysisFailure(string reason, Location? location)
	{
		Reason = reason;
		Location = location;
	}
}