using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CreateSystemRestorePoint
{
	class Program
	{
		static int Main(string[] args)
		{
			var restorePointName   = args.Length < 1 ? CreateNewName() : string.Join(" ", args);
			var managementPath     = new ManagementPath(@"\\.\ROOT\DEFAULT:SystemRestore");
			var systemRestoreClass = new ManagementClass(managementPath);
			var methodParameters   = systemRestoreClass.Methods["CreateRestorePoint"].InParameters;
			
			methodParameters.Properties["Description"].Value = "Testing";
			methodParameters.Properties["EventType"].Value   = 100u;
			methodParameters.Properties["Description"].Value = 16u;
			
			var outParameters = systemRestoreClass.InvokeMethod("CreateRestorePoint", methodParameters, null);
			var hresult       = unchecked( (int) (uint) outParameters["ReturnValue"]);
			
			// I suppose you could do this, but I'm not sure if it's necessary.
			// Marshal.ThrowExceptionForHR(hresult);
			
			return hresult;
		}
		
		
		private static string CreateNewName()
		{
			var d = DateTime.Now;
			
			return $"RP_{d:yy-MM-dd_HH:mm:ss.fff}";
		}
	}
}
