using System;

namespace SymbolicExecution.Control;

[AttributeUsage(AttributeTargets.Method)]
public class SymbolicallyAnalyzeAttribute : Attribute
{
}