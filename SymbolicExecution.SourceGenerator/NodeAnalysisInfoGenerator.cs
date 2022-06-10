using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace SymbolicExecution.SourceGenerator;

[Generator]
public class NodeAnalysisInfoGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context)
	{
	}

	public void Execute(GeneratorExecutionContext context)
	{
		var assembly = typeof(SyntaxNode).Assembly;
		var ass2 = typeof(CSharpSyntaxNode).Assembly;
		var exportedTypes = assembly.ExportedTypes.Concat(ass2.ExportedTypes).ToList();
		var syntaxNodeInfos = exportedTypes
			.Where(x => typeof(SyntaxNode).IsAssignableFrom(x))
			.Select(x => new SyntaxNodeInfo(x))
			.ToList();
		foreach (var syntaxNode in syntaxNodeInfos)
		{
			var nodeAnalysisInfoSource = new StringBuilder();
			nodeAnalysisInfoSource.AppendLine("// Generated");
			nodeAnalysisInfoSource.AppendLine("namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodes;");
			nodeAnalysisInfoSource.AppendLine();
			if (syntaxNode.NodeAnalysisInfoBase is not null)
				nodeAnalysisInfoSource.AppendLine($"public partial class {syntaxNode.NodeAnalysisInfoName} : {syntaxNode.NodeAnalysisInfoBase}");
			else
				nodeAnalysisInfoSource.AppendLine($"public partial class {syntaxNode.NodeAnalysisInfoName}");
			nodeAnalysisInfoSource.AppendLine("{");
			nodeAnalysisInfoSource.AppendLine("}");
			context.AddSource($"{syntaxNode.NodeAnalysisInfoName}.g.cs", SourceText.From(nodeAnalysisInfoSource.ToString(), Encoding.UTF8));
		}
	}
}

struct SyntaxNodeInfo
{
	public SyntaxNodeInfo(Type type)
	{
		Type = type;
		Name = Type.Name;
	}

	public Type Type { get; }
	public string Name { get; }
	public string HandlerName => Name + "Handler";

	public string NodeAnalysisInfoName => Name.Replace("SyntaxNode", "").Replace("Syntax", "") + "NodeAnalysisInfo";

	public string? NodeAnalysisInfoBase =>
		Type.BaseType != typeof(Object) && Type.BaseType != null ? new SyntaxNodeInfo(Type.BaseType).NodeAnalysisInfoName : null;
}