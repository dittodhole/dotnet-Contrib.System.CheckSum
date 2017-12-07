/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Security.Cryptography;
  using global::System.Text;
  using global::JetBrains.Annotations;

  [NotNull]
  public delegate HashAlgorithm GetHashAlgorithm();

  public partial class StringCheckSumCalculator : ICheckSumCalculator<string>
  {
    public StringCheckSumCalculator()
      : this(MD5.Create) { }

    /// <exception cref="ArgumentNullException"><paramref name="getHashAlgorithm"/> is <see langword="null"/></exception>
    public StringCheckSumCalculator([NotNull] GetHashAlgorithm getHashAlgorithm)
    {
      if (getHashAlgorithm == null)
      {
        throw new ArgumentNullException("getHashAlgorithm");
      }
      this._getHashAlgorithm = getHashAlgorithm;
    }

    [NotNull]
    private readonly GetHashAlgorithm _getHashAlgorithm;

    /// <inheritdoc />
    public virtual string CalculateCheckSum(string input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      string checkSum;

      {
        var bytes = Encoding.UTF8.GetBytes(input);

        using (var hashAlgorithm = this._getHashAlgorithm.Invoke())
        {
          bytes = hashAlgorithm.ComputeHash(bytes);
          checkSum = BitConverter.ToString(bytes)
                                 .Replace("-", string.Empty);
        }
      }

      return checkSum;
    }
  }
}
