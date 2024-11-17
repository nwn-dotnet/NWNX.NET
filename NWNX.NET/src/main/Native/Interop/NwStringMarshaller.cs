using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace NWNX.NET.Native.Interop
{
  /// <summary>
  /// Marshals a string using <see cref="StringUtils"/><br/>
  /// Encoding can be configured by setting the <see cref="StringUtils.Encoding"/> property.
  /// </summary>
  [CustomMarshaller(typeof(string), MarshalMode.Default, typeof(NwStringMarshaller))]
  [CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
  public static unsafe class NwStringMarshaller
  {
    internal ref struct ManagedToUnmanagedIn
    {
      private byte* unmanaged;
      private bool allocated;

      public void FromManaged(string? managed)
      {
        unmanaged = managed.GetNullTerminatedString();
        allocated = managed != null;
      }

      public byte* ToUnmanaged()
      {
        return unmanaged;
      }

      public void Free()
      {
        if (allocated)
        {
          Marshal.FreeHGlobal((IntPtr)unmanaged);
        }
      }
    }

    internal static string? ConvertToManaged(byte* unmanaged)
    {
      return StringUtils.ReadNullTerminatedString(unmanaged);
    }
  }
}
