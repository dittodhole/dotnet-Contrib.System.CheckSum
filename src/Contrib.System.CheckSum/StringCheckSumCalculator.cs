/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Security.Cryptography;
  using global::System.Text;
  using global::JetBrains.Annotations;

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

    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    protected virtual byte[] GetBytes([NotNull] string str)
    {
      var encoding = this.GetEncoding();
      var result = encoding.GetBytes(str);

      return result;
    }

    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    protected virtual Encoding GetEncoding()
    {
      var result = Encoding.UTF8;

      return result;
    }

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
