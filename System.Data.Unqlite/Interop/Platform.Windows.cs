#if !UNIX

#region Usings
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

#endregion

// Taken from clrZMQ

namespace System.Data.Unqlite.Interop
{
	internal static partial class Platform
	{
		#region Costants
		private const string KernelLib = "kernel32";

		public const string LibSuffix = ".dll";
		#endregion


		#region Public Methods
		public static SafeLibraryHandle OpenHandle(string filename)
		{
			return LoadLibrary(filename);
		}

		public static IntPtr LoadProcedure(SafeLibraryHandle handle, string functionName)
		{
			return GetProcAddress(handle, functionName);
		}

		public static bool ReleaseHandle(IntPtr handle)
		{
			return FreeLibrary(handle);
		}

		public static Exception GetLastLibraryError()
		{
			return Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
		}
		#endregion


		#region Private Methods
		[DllImport(KernelLib, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
		private static extern SafeLibraryHandle LoadLibrary(string fileName);

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport(KernelLib, SetLastError = true)]
		[return : MarshalAs(UnmanagedType.Bool)]
		private static extern bool FreeLibrary(IntPtr moduleHandle);

		[DllImport(KernelLib)]
		private static extern IntPtr GetProcAddress(SafeLibraryHandle moduleHandle, string procname);
		#endregion
	}
}


#endif