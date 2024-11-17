using System.Runtime.InteropServices;
using System.Text;

namespace NWNX.NET.Native
{
  /// <summary>
  /// Helper utilities for converting between native and managed strings.
  /// </summary>
  public static unsafe class StringUtils
  {
    private static Encoding encoding;

    /// <summary>
    /// Gets or sets the encoding to use for native/managed string conversion.<br/>
    /// Defaults to windows-1252.
    /// </summary>
    public static Encoding Encoding
    {
      get => encoding;
      set
      {
        if (!value.IsSingleByte)
        {
          throw new InvalidOperationException("Encoding must use single-byte code points.");
        }

        encoding = value;
      }
    }

    static StringUtils()
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      encoding = Encoding.GetEncoding("windows-1252");
    }

    /// <summary>
    /// Copies the specified C# string and allocates a null-terminated string in unmanaged memory with cp1252 encoding.
    /// </summary>
    /// <param name="value">The managed string to encode.</param>
    /// <returns>The pointer to the unmanaged char array.</returns>
    public static byte* GetNullTerminatedString(this string? value)
    {
      if (value == null)
      {
        return null;
      }

      byte* unmanaged = (byte*)Marshal.AllocHGlobal(value.Length + 1);

      // Write string to buffer.
      Encoding.GetBytes(value, new Span<byte>(unmanaged, value.Length));

      // Write null terminator
      unmanaged[value.Length] = 0;

      return unmanaged;
    }

    /// <summary>
    /// Copies the specified C# string and allocates a string in unmanaged memory with cp1252 encoding.
    /// </summary>
    /// <param name="value">The managed string to encode.</param>
    /// <param name="length">The max length of the string. If specified, and the specified string is smaller than the length, it will be null terminated.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the string value is larger than the length.</exception>
    /// <returns>The pointer to the unmanaged char array.</returns>
    public static byte* GetFixedLengthString(this string value, int? length = null)
    {
      if (length.HasValue)
      {
        if (value.Length > length)
        {
          throw new ArgumentOutOfRangeException(nameof(value), "value must be smaller than length.");
        }

        if (value.Length < length)
        {
          return GetNullTerminatedString(value);
        }
      }

      byte* unmanaged = (byte*)Marshal.AllocHGlobal(value.Length);
      Encoding.GetBytes(value, new Span<byte>(unmanaged, value.Length));

      return unmanaged;
    }

    /// <summary>
    /// Reads a null-terminated string from the specified pointer with cp1252 encoding.
    /// </summary>
    /// <param name="cString">A pointer to a C string.</param>
    /// <returns>The converted managed string.</returns>
    public static string? ReadNullTerminatedString(byte* cString)
    {
      return cString != null ? Encoding.GetString(cString, GetStringLength(cString)) : null;
    }

    /// <inheritdoc cref="ReadNullTerminatedString(byte*)"/>
    public static string? ReadNullTerminatedString(this IntPtr cString)
    {
      return ReadNullTerminatedString((byte*)cString);
    }

    /// <summary>
    /// Reads a string from the specified pointer with the specified length with cp1252 encoding.
    /// </summary>
    /// <param name="cString">A pointer to a C string.</param>
    /// <param name="length">The length of the string.</param>
    /// <returns>The converted managed string.</returns>
    public static string ReadFixedLengthString(byte* cString, int length)
    {
      return Encoding.GetString(cString, GetStringLength(cString, length));
    }

    private static int GetStringLength(byte* cString, int? maxLength = null)
    {
      byte* walk = cString;
      while (*walk != 0 && (maxLength == null || walk - cString < maxLength))
      {
        walk++;
      }

      return (int)(walk - cString);
    }
  }
}
