namespace NWNX.NET.Native
{
  /// <summary>
  /// Default hook order constants for <see cref="NWNXAPI.RequestFunctionHook"/>
  /// </summary>
  public class HookOrder
  {
    /// <summary>
    /// Default hook order priority.
    /// </summary>
    public const int Default = 0;

    /// <summary>
    /// Earliest hook order priority.
    /// </summary>
    public const int Earliest = -3000000;

    /// <summary>
    /// Early hook order priority.
    /// </summary>
    public const int Early = -1000000;

    /// <summary>
    /// Final hook order priority.
    /// </summary>
    public const int Final = int.MaxValue;

    /// <summary>
    /// Late hook order priority.
    /// </summary>
    public const int Late = 1000000;

    /// <summary>
    /// Latest hook order priority.
    /// </summary>
    public const int Latest = 3000000;

    /// <summary>
    /// SharedHook hook order priority.
    /// </summary>
    public const int SharedHook = int.MinValue;

    /// <summary>
    /// VeryEarly hook order priority.
    /// </summary>
    public const int VeryEarly = -2000000;

    /// <summary>
    /// VeryLate hook order priority.
    /// </summary>
    public const int VeryLate = 2000000;
  }
}
