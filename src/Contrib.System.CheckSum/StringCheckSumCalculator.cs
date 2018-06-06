/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Security.Cryptography;
  using global::System.Text;
  using global::JetBrains.Annotations;

  /// <inheritdoc/>
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class StringCheckSumCalculator : ICheckSumCalculator<string>
  {
    /// <inheritdoc/>
    public virtual string CalculateCheckSum(string input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var bytes = this.GetBytes(input);
      var checkSum = this.ComputeCheckSum(bytes);

      var result = BitConverter.ToString(checkSum)
                               .Replace("-", string.Empty);

      return result;
    }

    /// <summary>
    ///   Converts <paramref name="str"/> to <see cref="byte[]"/>.
    /// </summary>
    /// <param name="str"/>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    protected virtual byte[] GetBytes([NotNull] string str)
    {
      var encoding = this.GetEncoding();
      var result = encoding.GetBytes(str);

      return result;
    }

    /// <summary>
    ///   Returns the encoding.
    /// </summary>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    protected virtual Encoding GetEncoding()
    {
      var result = Encoding.UTF8;

      return result;
    }

    /// <summary>
    ///   Computes the check sum for <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer"/>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    protected virtual byte[] ComputeCheckSum([NotNull] byte[] buffer)
    {
      byte[] result;
      using (var md5 = MD5.Create())
      {
        result = md5.ComputeHash(buffer);
      }

      return result;
    }
  }
}
