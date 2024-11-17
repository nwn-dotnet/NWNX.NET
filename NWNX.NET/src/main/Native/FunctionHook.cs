using System.Runtime.InteropServices;

namespace NWNX.NET.Native
{
  /// <summary>
  /// Memory structure for NWNXLib::Hooks::FunctionHook<br/>
  /// https://github.com/nwnxee/unified/blob/master/NWNXLib/nwnx.hpp#L82-L112
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public readonly unsafe struct FunctionHook
  {
    public readonly void* m_originalFunction;
    public readonly void* m_newFunction;
    public readonly int m_order;
    public readonly void* m_funchook;
    public readonly void* m_trampoline;
  }
}
