using System;

namespace SymbolicExecution.Control;

/// <summary>
/// Marks a method for symbolic execution analysis.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class SymbolicallyAnalyzeAttribute : Attribute
{
}