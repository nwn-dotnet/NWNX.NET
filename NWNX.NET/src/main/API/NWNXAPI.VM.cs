using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using NWNX.NET.Native;
using NWNX.NET.Native.Interop;

namespace NWNX.NET
{
  /// <summary>
  /// NWNX exported API functions from native "NWNX_DotNET" library.
  /// </summary>
  public static partial class NWNXAPI
  {
    /// <summary>
    /// VM Function. Push an integer to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPushInteger")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void StackPushInteger(int value);

    /// <summary>
    /// VM Function. Push a float to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPushFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void StackPushFloat(float value);

    /// <summary>
    /// VM Function. Push a string to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPushRawString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void StackPushString([MarshalUsing(typeof(NwStringMarshaller))] string? value);

    /// <summary>
    /// VM Function. Push an object (ID) to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPushObject")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void StackPushObject(uint value);

    /// <summary>
    /// VM Function. Push a <see cref="Vector3"/> to the current argument stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPushVector")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void StackPushVector([MarshalUsing(typeof(Vector3Marshaller))] Vector3 value);

    /// <summary>
    /// VM Function. Push a game defined structure (pointer) to the current argument stack.
    /// </summary>
    /// <param name="type">The engine structure type.</param>
    /// <param name="value">The value to push.</param>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPushGameDefinedStructure")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void StackPushGameDefinedStructure(int type, IntPtr value);

    /// <summary>
    /// VM Function. Call the function with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the function to call.</param>
    /// <remarks>
    /// Function IDs can be found here: https://github.com/nwnxee/unified/blob/master/NWNXLib/API/Constants/VirtualMachine.hpp#L51
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "CallBuiltIn")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CallBuiltIn(int id);

    /// <summary>
    /// VM Function. Pop an integer return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPopInteger")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int StackPopInteger();

    /// <summary>
    /// VM Function. Pop a float return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPopFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float StackPopFloat();

    /// <summary>
    /// VM Function. Pop a string return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPopRawString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(NwStringMarshaller))]
    public static partial string? StackPopString();

    /// <summary>
    /// VM Function. Pop an object (ID) from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPopObject")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint StackPopObject();

    /// <summary>
    /// VM Function. Pop a <see cref="Vector3"/> return value from the current function call.
    /// </summary>
    /// <returns>The value/result from the function call.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPopVector")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(Vector3Marshaller))]
    public static partial Vector3 StackPopVector();

    /// <summary>
    /// VM Function. Pop a game defined structure return value from the current function call.
    /// </summary>
    /// <param name="type">The engine structure type.</param>
    /// <returns>The value/result from the function call.</returns>
    /// <remarks>
    /// See https://github.com/nwnxee/unified/blob/master/NWNXLib/API/Constants/VirtualMachine.hpp#L34 for engine structure type IDs.
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "StackPopGameDefinedStructure")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr StackPopGameDefinedStructure(int type);

    /// <summary>
    /// VM Function. Dispose of a game defined structure, freeing the underlying native memory.
    /// </summary>
    /// <param name="type">The engine structure type.</param>
    /// <param name="value">A pointer to the created engine structure.</param>
    /// <remarks>
    /// See https://github.com/nwnxee/unified/blob/master/NWNXLib/API/Constants/VirtualMachine.hpp#L34 for engine structure type IDs.
    /// </remarks>
    [LibraryImport("NWNX_DotNET", EntryPoint = "FreeGameDefinedStructure")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void FreeGameDefinedStructure(int type, IntPtr value);

    /// <summary>
    /// VM Function. Signals an AssignCommand closure event with the specified object id and event id.<br/>
    /// This will trigger a call to the method registered with <see cref="RegisterClosureHandler"/> with the matching event id.
    /// </summary>
    /// <param name="oid">The object (ID) to use for the event script context.</param>
    /// <param name="eventId">A unique identifier for this event.</param>
    /// <returns>1 if the event was successfully queued, 0 on a failure.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "ClosureAssignCommand")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int ClosureAssignCommand(uint oid, ulong eventId);

    /// <summary>
    /// VM Function. Signals a DelayCommand closure event with the specified object id, delay and event id.<br/>
    /// This will trigger a call to the method registered with <see cref="RegisterClosureHandler"/> with the matching event id.
    /// </summary>
    /// <param name="oid">The object (ID) to use for the event script context.</param>
    /// <param name="duration">The duration in seconds, before the event will be triggered.</param>
    /// <param name="eventId">A unique identifier for this event.</param>
    /// <returns>1 if the event was successfully queued, 0 on a failure.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "ClosureDelayCommand")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int ClosureDelayCommand(uint oid, float duration, ulong eventId);

    /// <summary>
    /// VM Function. Queues a closure event on the action queue of the specified object with the specified event id.<br/>
    /// When this item is reached on the action queue, it will trigger a call to the method registered with <see cref="RegisterClosureHandler"/> with the matching event id.
    /// </summary>
    /// <param name="oid">The object (ID) to receive the action queue item.</param>
    /// <param name="eventId">A unique identifier for this event.</param>
    /// <returns>1 if the event was successfully queued, 0 on a failure.</returns>
    [LibraryImport("NWNX_DotNET", EntryPoint = "ClosureActionDoCommand")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int ClosureActionDoCommand(uint oid, ulong eventId);
  }
}
