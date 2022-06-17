using System;
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
		// var assembly = typeof(SyntaxNode).Assembly;
		// var ass2 = typeof(CSharpSyntaxNode).Assembly;
		// // ass2: C:\Program Files\dotnet\sdk\6.0.105\Roslyn\bincore\Microsoft.CodeAnalysis.CSharp.dll
		// var exportedTypes = assembly.ExportedTypes.Concat(ass2.ExportedTypes).ToList();
		// var syntaxNodeInfos = exportedTypes
		// 	.Where(x => typeof(SyntaxNode).IsAssignableFrom(x))
		// 	.Select(x => new SyntaxNodeInfo(x))
		// 	.ToList();
		// foreach (var syntaxNode in syntaxNodeInfos)
		// {
		// 	var nodeAnalysisSourceText = GenerateNodeAnalysisSourceText(syntaxNode);
		// 	context.AddSource($"{syntaxNode.NodeAnalysisInfoName}.g.cs", nodeAnalysisSourceText);
		// 	var handlerSourceText = GenerateHandlerSourceText(syntaxNode);
		// 	context.AddSource($"{syntaxNode.Name}Handler.g.cs", handlerSourceText);
		// }
	}

	private static SourceText GenerateNodeAnalysisSourceText(SyntaxNodeInfo syntaxNode)
	{
		var nodeAnalysisInfoSource = new StringBuilder();
		nodeAnalysisInfoSource.AppendLine("// Generated");
		nodeAnalysisInfoSource.AppendLine("namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodes;");
		nodeAnalysisInfoSource.AppendLine();
		if (syntaxNode.NodeAnalysisInfoBase is not null)
			nodeAnalysisInfoSource.AppendLine(
				$"public partial class {syntaxNode.NodeAnalysisInfoName} : {syntaxNode.NodeAnalysisInfoBase}"
				);
		else
			nodeAnalysisInfoSource.AppendLine($"public partial class {syntaxNode.NodeAnalysisInfoName}");
		nodeAnalysisInfoSource.AppendLine("{");
		nodeAnalysisInfoSource.AppendLine("}");
		var nodeAnalysisSourceText = SourceText.From(nodeAnalysisInfoSource.ToString(), Encoding.UTF8);
		return nodeAnalysisSourceText;
	}

	private static SourceText GenerateHandlerSourceText(SyntaxNodeInfo syntaxNode)
	{
		var handlerSource = new StringBuilder();
		handlerSource.AppendLine("// Generated");
		handlerSource.AppendLine("namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;");
		handlerSource.AppendLine();
		handlerSource.AppendLine($"public partial class {syntaxNode.Name}Handler : SyntaxNodeConversionHandlerBase<{syntaxNode.Name}>");
		handlerSource.AppendLine("{");
		handlerSource.AppendLine($"\tprotected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync({syntaxNode.Name} node, CancellationToken token)");
		handlerSource.AppendLine("\t{");
		handlerSource.AppendLine("\t\tvar children = await ProcessChildNodesAsync(node, token);");
		handlerSource.AppendLine("\t\tif (children.IsFaulted)");
		handlerSource.AppendLine("\t\t\treturn new Result<INodeAnalysisInfo>(children.ErrorInfo);");
		handlerSource.AppendLine();
		handlerSource.AppendLine($"\t\tvar result = new {syntaxNode.NodeAnalysisInfoName}");
		handlerSource.AppendLine("\t\t{");
		handlerSource.AppendLine("\t\t\tChildren = children.Value,");
		handlerSource.AppendLine("\t\t};");
		handlerSource.AppendLine();
		handlerSource.AppendLine("\t\treturn new Result<INodeAnalysisInfo>(result);");
		handlerSource.AppendLine("\t}");
		handlerSource.AppendLine("}");
		var handlerSourceText = SourceText.From(handlerSource.ToString(), Encoding.UTF8);
		return handlerSourceText;
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

	public string NodeAnalysisInfoName => Name.Replace("SyntaxNode", "").Replace("Syntax", "") + "NodeAnalysisInfo";

	public string? NodeAnalysisInfoBase =>
		Type.BaseType != typeof(Object) && Type.BaseType != null ? new SyntaxNodeInfo(Type.BaseType).NodeAnalysisInfoName : null;
}