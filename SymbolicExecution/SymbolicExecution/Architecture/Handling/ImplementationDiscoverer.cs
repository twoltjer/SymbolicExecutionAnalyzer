namespace SymbolicExecution.Architecture.Handling;

public static class ImplementationDiscoverer
{
	public static Lazy<Type[]> AssemblyTypes = new Lazy<Type[]>(
		() =>
		{
			var assembly = typeof(ImplementationDiscoverer).Assembly;
			return assembly.GetTypes();
		}
		);
	public static object[] CreateImplementationInstances(Type interfaceType)
	{
		var instances = AssemblyTypes.Value.Where(interfaceType.IsAssignableFrom)
			.Where(x => !x.ContainsGenericParameters)
			.Select(
			x => typeof(ImplementationDiscoverer).Assembly.CreateInstance(x.FullName!)
			).ToArray();
		return instances;
	}
}