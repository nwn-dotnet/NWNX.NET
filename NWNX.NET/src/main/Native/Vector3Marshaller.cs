using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace NWNX.NET.Native
{
  /// <summary>
  /// Marshals a Vector3 structure.
  /// </summary>
  [CustomMarshaller(typeof(Vector3), MarshalMode.Default, typeof(Vector3Marshaller))]
  public static class Vector3Marshaller
  {
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Unmanaged(float x, float y, float z)
    {
      public readonly float X = x;
      public readonly float Y = y;
      public readonly float Z = z;
    }

    internal static Unmanaged ConvertToUnmanaged(Vector3 vector)
    {
      return new Unmanaged(vector.X, vector.Y, vector.Z);
    }

    internal static Vector3 ConvertToManaged(Unmanaged unmanaged)
    {
      return new Vector3(unmanaged.X, unmanaged.Y, unmanaged.Z);
    }
  }
}
