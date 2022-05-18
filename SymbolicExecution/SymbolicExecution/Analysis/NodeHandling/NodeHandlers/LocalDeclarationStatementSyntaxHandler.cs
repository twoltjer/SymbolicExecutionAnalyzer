using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SymbolicExecution.Analysis.NodeHandling.NodeHandlers
{
	public class LocalDeclarationStatementSyntaxHandler : NodeHandlerBase<LocalDeclarationStatementSyntax>
	{
		protected override SymbolicAnalysisContext ProcessNode(
			LocalDeclarationStatementSyntax node,
			SymbolicAnalysisContext analysisContext,
			CodeBlockAnalysisContext codeBlockContext
			)
		{
			var mutatedContext = analysisContext;
			foreach (var variable in node.Declaration.Variables)
			{
				var variableName = variable.Identifier.Text;
				var variableSymbol = codeBlockContext.SemanticModel.GetDeclaredSymbol(variable) as ILocalSymbol;
				var type = variableSymbol?.Type;
				var specialType = type?.SpecialType;
				var initializer = variable.Initializer;
				var initializerValue = initializer?.Value;
				var initializedValue = initializerValue?.ToString();
				switch (specialType)
				{
					case SpecialType.System_Int32:
						var valueScope = initializedValue != null
							? new ConcreteValueScope<int>(int.Parse(initializedValue))
							: (IValueScope) UninitializedValueScope.Instance;
						mutatedContext = mutatedContext.WithDeclaration(new VariableInfo(variableName, specialType.Value, valueScope));
						break;
					default:
						throw new UnhandledSyntaxException();
				}
			}
			return mutatedContext;
		}
	}
}