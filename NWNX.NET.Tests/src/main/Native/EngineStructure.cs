using System;

namespace NWNX.NET.Tests.Native
{
  public sealed class EngineStructure(int structureType, IntPtr pointer) : IDisposable
  {
    public IntPtr Pointer { get; private set; } = pointer;

    private void ReleaseUnmanagedResources()
    {
      NWNXAPI.FreeGameDefinedStructure(structureType, Pointer);
      Pointer = IntPtr.Zero;
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    public static implicit operator IntPtr(EngineStructure engineStructure)
    {
      return engineStructure.Pointer;
    }

    ~EngineStructure()
    {
      ReleaseUnmanagedResources();
    }
  }
}
