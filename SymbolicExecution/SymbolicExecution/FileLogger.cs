using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SymbolicExecution
{
	public class FileLogger : IDisposable
	{
		private readonly string _filePath;
		private StreamWriter _writer;

		public FileLogger(string filePath)
		{
			_filePath = filePath;
		}

		public void Dispose()
		{
			_writer?.Dispose();
		}

		[SuppressMessage(category: "ReSharper", checkId: "UnusedMember.Global")]
		public void WriteLine(string message)
		{
			if (_writer == null)
				_writer = new StreamWriter(Path.Combine("C:\\", _filePath));
			_writer.WriteLine(message);
			_writer.Flush();
		}
	}
}