using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateSystemRestorePoint
{
	/// <summary>
	/// The type of restore point.
	/// </summary>
	public enum RestorePointType
	{
		/// <summary>An application has been installed.</summary>
		ApplicationInstall = 0,
		/// <summary>An application has been uninstalled.</summary>
		ApplicationUninstall = 1,
		/// <summary>A device driver has been installed.</summary>
		DeviceDriverInstall = 10,
		/// <summary>An application has had features added or removed.</summary>
		ModifySettings = 12,
		/// <summary>An application needs to delete the restore point it created. For example, an application would use this flag when a user cancels an installation.</summary>
		CancelledOperation = 13
	}
}
