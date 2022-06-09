namespace SymbolicExecution;

public readonly struct AnalysisErrorInfo
{
	public AnalysisErrorInfo(string message, Location? location)
	{
		Message = $"{GetCallerInfo()}: {message}";
		Location = location is not null && location != Location.None ? location : default;
	}

	private static string GetCallerInfo()
	{
		return "";
	}

	public string Message { get; }
	public Location? Location { get; }

	public override bool Equals(object obj)
	{
		return obj is AnalysisErrorInfo analysisErrorInfo && Equals(analysisErrorInfo);
	}

	public bool Equals(AnalysisErrorInfo other)
	{
		return Message == other.Message && Equals(Location, other.Location);
	}

	public override int GetHashCode()
	{
		return (Message, Location).GetHashCode();
	}

	public static bool operator ==(AnalysisErrorInfo left, AnalysisErrorInfo right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(AnalysisErrorInfo left, AnalysisErrorInfo right)
	{
		return !left.Equals(right);
	}
}