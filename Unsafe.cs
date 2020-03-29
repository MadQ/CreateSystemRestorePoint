using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CreateSystemRestorePoint
{
	internal enum AttachParentConsoleResult
	{
		Success             = 0,
		AlreadyAttched      = 5, // ERROR_ACCESS_DENIED 
		ParentHasNoConsole  = 6, // ERROR_INVALID_HANDLE 
		ProcessDoesNotExist = 87 // ERROR_INVALID_PARAMETER 
	}
	
	internal class Unsafe
	{
		public const int AttachParentProcess = -1;

		public static bool AttachParentConsole()
		{
			var success = AttachConsole(AttachParentProcess);
			
			if(success)
				return true;
			
			var result = (AttachParentConsoleResult) Marshal.GetLastWin32Error();
			
			return result == AttachParentConsoleResult.AlreadyAttched;
		}

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool AttachConsole(int processId);
		
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool AllocConsole();
		
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool FreeConsole();
		
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool SetLastError(int error);
	}
}
