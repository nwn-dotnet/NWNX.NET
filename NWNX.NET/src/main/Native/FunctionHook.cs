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
    /// <summary>
    /// void* m_originalFunction;
    /// </summary>
    public readonly void* m_originalFunction;

    /// <summary>
    /// void* m_newFunction;
    /// </summary>
    public readonly void* m_newFunction;

    /// <summary>
    /// int32_t m_order;
    /// </summary>
    public readonly int m_order;

    /// <summary>
    /// void* m_funchook;
    /// </summary>
    public readonly void* m_funchook;

    /// <summary>
    /// void* m_trampoline;
    /// </summary>
    public readonly void* m_trampoline;
  }
}
