using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CreateSystemRestorePoint
{
	/*
		An HRESULT value consists of the following fields:
			• A 1-bit code indicating severity, where zero represents success and 1 represents failure.
			• A 4-bit reserved value.
			• An 11-bit code indicating responsibility for the error or warning, also known as a facility code.
			• A 16-bit code describing the error or warning.
	
		Severity: 10000000000000000000000000000000
		Reserved: 01111000000000000000000000000000
		Facility: 00000111111111110000000000000000
		Code:     00000000000000001111111111111111
	*/
	
	[StructLayout(LayoutKind.Sequential, Size = 4, Pack = 4)]
	public readonly struct HResult : IEquatable<HResult>
	{
		private const uint severity = 0x8000_0000u;
		private const uint reserved = 0x7800_0000u;
		private const uint facility = 0x07FF_0000u;
		private const uint code     = 0x0000_ffffu;
		
		private const int reservedShift = 32 -  5;
		private const int facilityShift = 32 - 16;
		
		private readonly uint value;
		
		public HResult(int hr) => this.value = (uint) hr;
		
		
		public int	Value    => unchecked( (int) value );
		public bool	Success  => 0 == (this.value & severity);
		public int	Reserved => unchecked( (int) ((this.value & reserved) >> reservedShift) );
		public int	Facility => unchecked( (int) ((this.value & facility) >> facilityShift) );
		public int	Code     => unchecked( (int) (this.value & code) );
		
		public void ThrowOnFailure()
		{
			if( !Success )
				Marshal.ThrowExceptionForHR(Value);
		}
		
		public override string ToString() => $"0x{this.value:x8} ({nameof(Facility)} = {Facility}, {nameof(Code)}={Code})";
		
		public static explicit operator HResult(int hr) => new HResult(hr);
		
		public static HResult FromInt32(int hr) => new HResult(hr);
		
		public static implicit operator int(HResult hResult) => hResult.Value;
		
		public int ToInt32() => Value;

		public override bool Equals(object obj) => obj is HResult other ? this.value == other.value : false;

		public override int GetHashCode() => Value;

		public static bool operator ==(HResult l, HResult r) => l.value == r.value;
		
		public static bool operator !=(HResult l, HResult r) => l.value != r.value;
		
		public bool Equals(HResult other) => this.value == other.value;
	}
}
