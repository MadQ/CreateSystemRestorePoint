using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateSystemRestorePoint
{
	/// <summary>
	/// The type of the restore point event. 
	/// </summary>
	public enum EventType
	{
		/// <summary>A system change has begun. A subsequent nested call does not create a new restore point.</summary>
		BeginNestedSystemChange = 102,
		/// <summary>A system change has begun.</summary>
		BeginSystemChange = 100,
		/// <summary>A system change has ended.</summary>
		EndNestedSystemChange = 103,
		///<summaY>A system change has ended.</summary>
		EndSystemChange = 101,
	}
}
