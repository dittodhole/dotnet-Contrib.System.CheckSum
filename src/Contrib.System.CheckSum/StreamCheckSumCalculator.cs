/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.IO;
  using global::System.Security.Cryptography;
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
  [PublicAPI]
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class StreamCheckSumCalculator : ICheckSumCalculator<Stream>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="StreamCheckSumCalculator"/> class.
    /// </summary>
    public StreamCheckSumCalculator()
    {
      this._hashAlgorithmFactory = MD5.Create;
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="StreamCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="hashAlgorithmFactory"/>
    /// <exception cref="ArgumentNullException"><paramref name="hashAlgorithmFactory"/> is <see langword="null"/>.</exception>
    public StreamCheckSumCalculator([NotNull] HashAlgorithmFactory hashAlgorithmFactory)
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
    public virtual string CalculateCheckSum(Stream input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      byte[] checkSum;
      using (var hashAlgorithm = this._hashAlgorithmFactory.Invoke())
      {
        checkSum = hashAlgorithm.ComputeHash(input);
      }

      var result = BitConverter.ToString(checkSum)
                               .Replace("-", string.Empty);

      return result;
    }
  }
}
