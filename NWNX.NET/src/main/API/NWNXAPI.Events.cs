using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NWNX.NET.Native;

namespace NWNX.NET
{
  public static unsafe partial class NWNXAPI
  {
    /// <summary>
    /// Register the main loop handler. This event is called once every server update/loop.<br/>
    /// NwScript functions are available.
    /// </summary>
    /// <param name="handler">The method to invoke per server update.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "RegisterMainLoopHandler")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RegisterMainLoopHandler(delegate* unmanaged<ulong, void> handler);

    /// <summary>
    /// Register the run script handler. This event is called every time the virtual machine will execute a script.<br/>
    /// NwScript functions are available.
    /// </summary>
    /// <param name="handler">The method to handle VM run script requests.</param>
    /// <remarks>
    /// Use <see cref="StringUtils.ReadNullTerminatedString(byte*)"/> to convert the script name to a C# compatible string.<br/>
    /// The return value of this function should reflect if the script execution was handled in C# and the native VM execution should be skipped.<br/>
    ///   -1: Not handled. If the script resource (ncs) is defined in the module, it will now be executed.<br/>
    ///   0: FALSE/Handled - For conditional scripts, sets the result to FALSE. If the script resource (ncs) is defined in the module, it will not be executed.<br/>
    ///   1: TRUE - For conditional scripts, sets the result to TRUE. If the script resource (ncs) is defined in the module, it will not be executed.
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "RegisterRunScriptHandler")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RegisterRunScriptHandler(delegate* unmanaged<IntPtr, uint, int> handler);

    /// <summary>
    /// Register the closure handler. This event is called internally by the virtual machine to handle closure-based executions (AssignCommand, DelayCommand, etc.)<br/>
    /// NwScript functions are available.
    /// </summary>
    /// <param name="handler">The method to handle VM closure requests.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "RegisterClosureHandler")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RegisterClosureHandler(delegate* unmanaged<ulong, uint, void> handler);

    /// <summary>
    /// Register the NWNX signal handler. This event is called during significant server events.<br/>
    /// NwScript functions are available in the ON_MODULE_LOAD_FINISH, ON_DESTROY_SERVER events.
    /// </summary>
    /// <param name="handler">The method to handle NWNX Signal events.</param>
    /// <remarks>
    /// Use <see cref="StringUtils.ReadNullTerminatedString(byte*)"/> to convert the script name to a C# compatible string.
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "RegisterSignalHandler")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RegisterSignalHandler(delegate* unmanaged<IntPtr, void> handler);

    /// <summary>
    /// Register the NWNX assertion handler. This is called after an assertion failure in native code.
    /// </summary>
    /// <param name="handler">The method to handle assertion failures.</param>
    /// <remarks>
    /// Use <see cref="StringUtils.ReadNullTerminatedString(byte*)"/> to convert the script name to a C# compatible string.
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "RegisterAssertHandler")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RegisterAssertHandler(delegate* unmanaged<IntPtr, IntPtr, void> handler);

    /// <summary>
    /// Register the NWNX crash handler. NWNX will attempt to call this during a server crash.
    /// </summary>
    /// <param name="handler">The method to handle server crashes.</param>
    /// <remarks>
    /// At this point, the server is in an undefined state and this handler may or may not execute.<br/>
    /// This method should only attempt to log additional information, and NEVER attempt to save/modify existing state.<br/>
    /// Anything invoked by this function is considered undefined behaviour.
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "RegisterCrashHandler")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RegisterCrashHandler(delegate* unmanaged<int, IntPtr, void> handler);
  }
}
