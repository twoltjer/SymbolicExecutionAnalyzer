using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;

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
					case null:
					case SpecialType.None:
					case SpecialType.System_Object:
					case SpecialType.System_Enum:
					case SpecialType.System_MulticastDelegate:
					case SpecialType.System_Delegate:
					case SpecialType.System_ValueType:
					case SpecialType.System_Void:
					case SpecialType.System_Boolean:
					case SpecialType.System_Char:
					case SpecialType.System_SByte:
					case SpecialType.System_Byte:
					case SpecialType.System_Int16:
					case SpecialType.System_UInt16:
					case SpecialType.System_UInt32:
					case SpecialType.System_Int64:
					case SpecialType.System_UInt64:
					case SpecialType.System_Decimal:
					case SpecialType.System_Single:
					case SpecialType.System_Double:
					case SpecialType.System_String:
					case SpecialType.System_IntPtr:
					case SpecialType.System_UIntPtr:
					case SpecialType.System_Array:
					case SpecialType.System_Collections_IEnumerable:
					case SpecialType.System_Collections_Generic_IEnumerable_T:
					case SpecialType.System_Collections_Generic_IList_T:
					case SpecialType.System_Collections_Generic_ICollection_T:
					case SpecialType.System_Collections_IEnumerator:
					case SpecialType.System_Collections_Generic_IEnumerator_T:
					case SpecialType.System_Collections_Generic_IReadOnlyList_T:
					case SpecialType.System_Collections_Generic_IReadOnlyCollection_T:
					case SpecialType.System_Nullable_T:
					case SpecialType.System_DateTime:
					case SpecialType.System_Runtime_CompilerServices_IsVolatile:
					case SpecialType.System_IDisposable:
					case SpecialType.System_TypedReference:
					case SpecialType.System_ArgIterator:
					case SpecialType.System_RuntimeArgumentHandle:
					case SpecialType.System_RuntimeFieldHandle:
					case SpecialType.System_RuntimeMethodHandle:
					case SpecialType.System_RuntimeTypeHandle:
					case SpecialType.System_IAsyncResult:
					case SpecialType.System_AsyncCallback:
					case SpecialType.System_Runtime_CompilerServices_RuntimeFeature:
					default:
						Debug.Fail($"Unexpected type: {type}");
						break;
				}
			}
			return mutatedContext;
		}
	}
}