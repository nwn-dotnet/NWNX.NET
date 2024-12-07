using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NWNX.NET.Native;

namespace NWNX.NET
{
  public static unsafe partial class NWNXAPI
  {
    /// <summary>
    /// Request a function hook for the specified base game function.<br/>
    /// When this function would be called, the specified managed method will be called instead.
    /// </summary>
    /// <param name="address">A pointer to the base game function to hook.</param>
    /// <param name="managedFuncPtr">A pointer to the method that should replace the base game function.</param>
    /// <param name="priority">The priority of the hook. See remarks for best practices.</param>
    /// <returns>The created function hook.</returns>
    /// <remarks>
    /// Consider using the <see cref="HookOrder"/> constants for the priority value:<br/>
    ///   - Earliest for pure event notification hooks<br/>
    ///   - Early for skippable events, or events that modify state in before/after<br/>
    ///   - Late for things that provide alternative implementation in some cases, like the POS getters in creature<br/>
    ///   - Latest for things that almost never call the original<br/>
    ///   - Final for things that fully reimplement base game functions
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "RequestFunctionHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial FunctionHook* RequestFunctionHook(IntPtr address, IntPtr managedFuncPtr, int priority);

    /// <summary>
    /// Return a function hook created from <see cref="RequestFunctionHook"/>. This returns the function to the original behaviour prior to the hook.
    /// </summary>
    /// <param name="hook">The hook to return.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "ReturnFunctionHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void ReturnFunctionHook(FunctionHook* hook);
  }
}
