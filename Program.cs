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

			var restorePointName = args.Length < 1 ? $"RP_{DateTime.Now:yy-MM-dd_HH:mm:ss.fff}" : string.Join(" ", args);
			var eventType        = EventType.BeginSystemChange;
			var restorePointType = RestorePointType.ModifySettings;

			var hresult = CreateRestorePoint(restorePointName, eventType, restorePointType);
			
			if(hresult.Success)
				Console.WriteLine($"Restore point {restorePointName} created.");
			
			else
				Console.Error.WriteLine($"Failed to create restore point {restorePointName}. {nameof(HResult)}: {hresult}");

			// I suppose you could do this, but I'm not sure if it's necessary.
			// hresult.ThrowOnFailure();

			return hresult.Code;
		}

		public static HResult CreateRestorePoint(string restorePointName, EventType eventType, RestorePointType restorePointType)
		{
			// uint32 CreateRestorePoint([in] String Description, [in] uint32 RestorePointType, [in] uint32 EventType);
			
			var managementPath     = new ManagementPath(@"\\.\ROOT\DEFAULT:SystemRestore");
			var systemRestoreClass = new ManagementClass(managementPath);
			var methodParameters   = systemRestoreClass.Methods["CreateRestorePoint"].InParameters;

			methodParameters.Properties["Description"].Value = restorePointName;
			methodParameters.Properties["EventType"].Value = (uint) eventType;
			methodParameters.Properties["RestorePointType"].Value = (uint) restorePointType;

			var outParameters = systemRestoreClass.InvokeMethod("CreateRestorePoint", methodParameters, null);
			var hresult       = (HResult) unchecked((int) (uint) outParameters["ReturnValue"]);
			
			return hresult;
		}

		private static bool HelpShown(string[] args)
		{
			if(args.Length < 1 || !args[0].Contains("?"))
				return false;
			
			var success = Unsafe.AttachParentConsole();
			
			var helpText = $@"Creates a System Restore Point

usage: {Path.GetFileName(typeof(Program).Assembly.Location)} [<Restore Point Name>]

The default <Restore Point Name> is ""RP_yy-MM-dd_HH:mm:ss.fff"".
";

			if(Unsafe.AttachParentConsole()) {
				Console.WriteLine(helpText);
				Unsafe.FreeConsole();
			}
			else
				MessageBox.Show(helpText, typeof(Program).Namespace, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
			
			return true;
		}
		
	}
}
