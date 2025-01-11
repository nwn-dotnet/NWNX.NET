using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NUnit.Framework;
using NWNX.NET.Native;
using NWNX.NET.Tests.Async;
using NWNX.NET.Tests.Constants;

namespace NWNX.NET.Tests.Hooks
{
  [TestFixture(Category = "Hooks")]
  public sealed class HookTests
  {
    private static readonly IntPtr CExoDebugInternalWriteToLogFile = NativeLibrary.GetExport(NativeLibrary.GetMainProgramHandle(), "_ZN17CExoDebugInternal14WriteToLogFileERK10CExoString");

    private const int HookOrder = -2000005;

    private unsafe delegate void WriteToLogFile(void* pExoDebugInternal, void* pMessage);

    private static unsafe FunctionHook* currentHook;
    private static int callCount;
    private static string? logMessage;

    [Test]
    public async Task WriteLogFileDelegateHookTest()
    {
      unsafe
      {
        WriteToLogFile handler = WriteLogFileDelegateHandler;
        IntPtr managedFuncPtr = Marshal.GetFunctionPointerForDelegate(handler);
        currentHook = NWNXAPI.RequestFunctionHook(CExoDebugInternalWriteToLogFile, managedFuncPtr, HookOrder);

        Assert.That((IntPtr)currentHook->m_originalFunction, Is.EqualTo(CExoDebugInternalWriteToLogFile));
        Assert.That((IntPtr)currentHook->m_newFunction, Is.EqualTo(managedFuncPtr));
        Assert.That(currentHook->m_order, Is.EqualTo(HookOrder));
      }

      const string expectedMessage = "Delegate hook test message";
      NWNXAPI.StackPushString(expectedMessage);
      NWNXAPI.CallBuiltIn(VMFunctions.PrintString);

      await NwTask.WaitUntil(() => callCount > 0);

      Assert.That(callCount, Is.EqualTo(1));
      Assert.That(logMessage, Is.EqualTo(expectedMessage + "\n"));
    }

    private static unsafe void WriteLogFileDelegateHandler(void* pExoDebugInternal, void* pMessage)
    {
      callCount++;
      logMessage = StringUtils.ReadNullTerminatedString(*(byte**)pMessage);
    }

    [Test]
    public async Task WriteLogFileUnmanagedHookTest()
    {
      unsafe
      {
        delegate* unmanaged<void*, void*, void> managedFuncPtr = &WriteLogFileUnmanagedHandler;
        currentHook = NWNXAPI.RequestFunctionHook(CExoDebugInternalWriteToLogFile, (IntPtr)managedFuncPtr, HookOrder);

        Assert.That((IntPtr)currentHook->m_originalFunction, Is.EqualTo(CExoDebugInternalWriteToLogFile));
        Assert.That((IntPtr)currentHook->m_newFunction, Is.EqualTo((IntPtr)managedFuncPtr));
        Assert.That(currentHook->m_order, Is.EqualTo(HookOrder));
      }

      const string expectedMessage = "Delegate hook test message";
      NWNXAPI.StackPushString(expectedMessage);
      NWNXAPI.CallBuiltIn(VMFunctions.PrintString);

      await NwTask.WaitUntil(() => callCount > 0);

      Assert.That(callCount, Is.EqualTo(1));
      Assert.That(logMessage, Is.EqualTo(expectedMessage + "\n"));
    }

    [UnmanagedCallersOnly]
    private static unsafe void WriteLogFileUnmanagedHandler(void* pExoDebugInternal, void* pMessage)
    {
      callCount++;
      logMessage = StringUtils.ReadNullTerminatedString(*(byte**)pMessage);
    }

    [TearDown]
    public unsafe void TearDown()
    {
      NWNXAPI.ReturnFunctionHook(currentHook);
      currentHook = null;
      callCount = 0;
    }
  }
}
