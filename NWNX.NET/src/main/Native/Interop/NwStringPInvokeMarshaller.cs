using System;
using System.Runtime.InteropServices;

namespace NWNX.NET.Native.Interop
{
  /// <summary>
  /// Marshals a string using <see cref="StringUtils"/>.<br/>
  /// This should be used with <see cref="DllImportAttribute"/> declarations.<br/>
  /// Encoding can be configured by setting the <see cref="StringUtils.Encoding"/> property.
  /// </summary>
  public sealed unsafe class NwStringPInvokeMarshaller : ICustomMarshaler
  {
    private static NwStringPInvokeMarshaller? instance;

    IntPtr ICustomMarshaler.MarshalManagedToNative(object? managed)
    {
      return managed switch
      {
        null => IntPtr.Zero,
        string data => (IntPtr)data.GetNullTerminatedString(),
        _ => throw new MarshalDirectiveException($"{nameof(NwStringPInvokeMarshaller)} must be used on a string."),
      };
    }

    object ICustomMarshaler.MarshalNativeToManaged(IntPtr unmanaged)
    {
      return unmanaged.ReadNullTerminatedString()!;
    }

    void ICustomMarshaler.CleanUpNativeData(IntPtr unmanaged)
    {
      Marshal.FreeHGlobal(unmanaged);
    }

    void ICustomMarshaler.CleanUpManagedData(object managed) {}

    int ICustomMarshaler.GetNativeDataSize()
    {
      return -1;
    }

    /// <summary>
    /// Internal method used by CLR runtime to create marshaller.
    /// </summary>
    /// <param name="cookie">pstrCookie</param>
    /// <returns>The marshaller instance.</returns>
    public static ICustomMarshaler GetInstance(string cookie)
    {
      instance ??= new NwStringPInvokeMarshaller();
      return instance;
    }
  }
}
