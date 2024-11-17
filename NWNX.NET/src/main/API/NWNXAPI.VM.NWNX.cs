using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using NWNX.NET.Native.Interop;

namespace NWNX.NET
{
  public static partial class NWNXAPI
  {
    /// <summary>
    /// NWNX VM Function. Prepares the argument stack for the specified NWNX function.
    /// </summary>
    /// <param name="plugin">The name of the NWNX plugin containing the function to be called.</param>
    /// <param name="function">The name of the function to call.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXSetFunction")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NWNXSetFunction([MarshalUsing(typeof(Utf16StringMarshaller))] string plugin, [MarshalUsing(typeof(Utf16StringMarshaller))] string function);

    /// <summary>
    /// NWNX VM Function. Push an integer to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPushInt")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NWNXPushInt(int value);

    /// <summary>
    /// NWNX VM Function. Push a float to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPushFloat")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NWNXPushFloat(float value);

    /// <summary>
    /// NWNX VM Function. Push a string to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPushRawString")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NWNXPushString([MarshalUsing(typeof(NwStringMarshaller))] string? value);

    /// <summary>
    /// NWNX VM Function. Push an object (ID) to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPushObject")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NWNXPushObject(uint value);

    /// <summary>
    /// NWNX VM Function. Push an effect game structure pointer to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPushEffect")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void NWNXPushEffect(IntPtr value);

    /// <summary>
    /// NWNX VM Function. Push an item property game structure pointer to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPushItemProperty")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void NWNXPushItemProperty(IntPtr value);

    /// <summary>
    /// NWNX VM Function. Call the function set with <see cref="NWNXSetFunction"/> with the current argument stack.
    /// </summary>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXCallFunction")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NWNXCallFunction();

    /// <summary>
    /// NWNX VM Function. Pop an integer return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPopInt")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int NWNXPopInt();

    /// <summary>
    /// NWNX VM Function. Pop a float return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPopFloat")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial float NWNXPopFloat();

    /// <summary>
    /// NWNX VM Function. Pop a string return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPopRawString")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalUsing(typeof(NwStringMarshaller))]
    public static partial string? NWNXPopString();

    /// <summary>
    /// NWNX VM Function. Pop an object (ID) from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPopObject")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial uint NWNXPopObject();

    /// <summary>
    /// NWNX VM Function. Pop an effect game structure return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPopEffect")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr NWNXPopEffect();

    /// <summary>
    /// NWNX VM Function. Pop an item property game structure return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "NWNXPopItemProperty")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr NWNXPopItemProperty();
  }
}
