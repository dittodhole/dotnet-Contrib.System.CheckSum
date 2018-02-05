/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Security.Cryptography;
  using global::System.Text;
  using global::JetBrains.Annotations;

  public partial class StringCheckSumCalculator : ICheckSumCalculator<string>
  {
    /// <inheritdoc />
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

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    protected virtual byte[] GetBytes([NotNull] string str)
    {
      var encoding = this.GetEncoding();
      var bytes = encoding.GetBytes(str);

      return bytes;
    }

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    protected virtual Encoding GetEncoding()
    {
      return Encoding.UTF8;
    }

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    protected virtual byte[] ComputeCheckSum([NotNull] byte[] buffer)
    {
      using (var md5 = MD5.Create())
      {
        var checkSum = md5.ComputeHash(buffer);

        return checkSum;
      }
    }
  }
}
