using Microsoft.CodeAnalysis;

namespace SymbolicExecution
{
	public struct VariableInfo
	{
		public VariableInfo(string name, SpecialType type, IValueScope valueScope)
		{
			Name = name;
			Type = type;
			ValueScope = valueScope;
		}

		public string Name { get; }
		public SpecialType Type { get; }
		public IValueScope ValueScope { get; }
	}
}