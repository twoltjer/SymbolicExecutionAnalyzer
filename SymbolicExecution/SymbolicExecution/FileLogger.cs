using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SymbolicExecution
{
	public class FileLogger : IDisposable
	{
		private readonly StreamWriter _writer;

		public FileLogger(string filePath)
		{
			_writer = new StreamWriter(filePath, true);
		}

		public void Dispose()
		{
			_writer.Dispose();
		}

		[SuppressMessage(category: "ReSharper", checkId: "UnusedMember.Global")]
		public void WriteLine(string message)
		{
			_writer.WriteLine(message);
			_writer.Flush();
		}
	}
}