using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateSystemRestorePoint
{
	class Program
	{
		[STAThread]
		static int Main(string[] args)
		{
			if(HelpShown(args))
				return 0;
			
			var restorePointName   = args.Length < 1 ? CreateNewName() : string.Join(" ", args);
			var managementPath     = new ManagementPath(@"\\.\ROOT\DEFAULT:SystemRestore");
			var systemRestoreClass = new ManagementClass(managementPath);
			var methodParameters   = systemRestoreClass.Methods["CreateRestorePoint"].InParameters;
			
			methodParameters.Properties["Description"].Value      = restorePointName;
			methodParameters.Properties["EventType"].Value        = 100u;
			methodParameters.Properties["RestorePointType"].Value = 16u;
			
			var outParameters = systemRestoreClass.InvokeMethod("CreateRestorePoint", methodParameters, null);
			var hresult       = unchecked( (int) (uint) outParameters["ReturnValue"]);
			
			// I suppose you could do this, but I'm not sure if it's necessary.
			// Marshal.ThrowExceptionForHR(hresult);
			
			return hresult;
		}
		
		private static bool HelpShown(string[] args)
		{
			if(args.Length < 1 || !args[0].Contains("?"))
				return false;
			
			MessageBox.Show($@"Creates a System Restore Point

usage: {Path.GetFileName(typeof(Program).Assembly.Location)} [<Restore Point Name>]

The default <Restore Point Name> is ""RP_yy-MM-dd_HH:mm:ss.fff"".

Got it?"
	, typeof(Program).Namespace, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
			
			return true;
		}
		
		private static string CreateNewName()
		{
			var ss = "yy-MM-dd_HH:mm:ss.fff";
			
			Console.WriteLine(ss);
			
			return $"RP_{DateTime.Now:yy-MM-dd_HH:mm:ss.fff}";
		}
	}
}
