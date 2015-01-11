#region Usings
using System.Diagnostics;


#endregion


// Taken from clrZMQ


namespace System.Data.Unqlite.Interop
{
	internal class Tracer
	{
		#region Fields
		//private static readonly TraceSwitch _assemblySwitch = new TraceSwitch("clrzmq", "ZeroMQ C# Binding");
		private static readonly TraceSwitch _assemblySwitch = new TraceSwitch("unqlite", "Unqlite C# Binding");
		#endregion


		#region Public Methods
		public static void Verbose(string message, string category)
		{
			if (_assemblySwitch.TraceVerbose)
			{
				Trace.WriteLine(message, category);
			}
		}

		public static void Info(string message, string category)
		{
			if (_assemblySwitch.TraceInfo)
			{
				Trace.WriteLine(message, category);
			}
		}

		public static void Warning(string message, string category)
		{
			if (_assemblySwitch.TraceWarning)
			{
				Trace.WriteLine(message, category);
			}
		}

		public static void Error(string message, string category)
		{
			if (_assemblySwitch.TraceError)
			{
				Trace.WriteLine(message, category);
			}
		}

		public static void VerboseIf(bool condition, string message, string category)
		{
			if (_assemblySwitch.TraceVerbose && condition)
			{
				Trace.WriteLine(message, category);
			}
		}

		public static void InfoIf(bool condition, string message, string category)
		{
			if (_assemblySwitch.TraceInfo && condition)
			{
				Trace.WriteLine(message, category);
			}
		}

		public static void WarningIf(bool condition, string message, string category)
		{
			if (_assemblySwitch.TraceWarning && condition)
			{
				Trace.WriteLine(message, category);
			}
		}

		public static void ErrorIf(bool condition, string message, string category)
		{
			if (_assemblySwitch.TraceError && condition)
			{
				Trace.WriteLine(message, category);
			}
		}
		#endregion
	}
}