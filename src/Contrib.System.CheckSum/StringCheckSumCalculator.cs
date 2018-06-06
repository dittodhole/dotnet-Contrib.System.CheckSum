/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Security.Cryptography;
  using global::System.Text;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   
  /// </summary>
  /// <exception cref="Exception"/>
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  delegate HashAlgorithm HashAlgorithmFactory();

  /// <inheritdoc/>
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class StringCheckSumCalculator : ICheckSumCalculator<string>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="StringCheckSumCalculator"/> class.
    /// </summary>
    public StringCheckSumCalculator()
      : this(MD5.Create) { }

    /// <summary>
    ///   Initializes a new instance of the <see cref="StringCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="hashAlgorithmFactory"/>
    /// <exception cref="ArgumentNullException"><paramref name="hashAlgorithmFactory"/> is <see langword="null"/>.</exception>
    public StringCheckSumCalculator([NotNull] HashAlgorithmFactory hashAlgorithmFactory)
    {
      if (hashAlgorithmFactory == null)
      {
        throw new ArgumentNullException("hashAlgorithmFactory");
      }
      this._hashAlgorithmFactory = hashAlgorithmFactory;
    }

    [NotNull]
    private readonly HashAlgorithmFactory _hashAlgorithmFactory;

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
      using (var hashAlgorithm = _hashAlgorithmFactory.Invoke())
      {
        result = hashAlgorithm.ComputeHash(buffer);
      }

      return result;
    }
  }
}
