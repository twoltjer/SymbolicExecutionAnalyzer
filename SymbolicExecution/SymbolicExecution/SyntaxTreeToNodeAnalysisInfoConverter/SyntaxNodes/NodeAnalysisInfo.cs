namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodes;

public partial class NodeAnalysisInfo : INodeAnalysisInfo
{
	public INodeAnalysisInfo[] Children { get; set; }
}