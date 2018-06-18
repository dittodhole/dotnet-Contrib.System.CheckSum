/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.IO;
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
    /// <summary>
    ///   Initializes a new instance of the <see cref="StringCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="checkSumCalculator"/>
    /// <exception cref="ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/>.</exception>
    public StringCheckSumCalculator([NotNull] ICheckSumCalculator<Stream> checkSumCalculator)
    {
      if (checkSumCalculator == null)
      {
        throw new ArgumentNullException("checkSumCalculator");
      }
      this._checkSumCalculator = checkSumCalculator;
    }

    [NotNull]
    private readonly ICheckSumCalculator<Stream> _checkSumCalculator;

    /// <inheritdoc/>
    public virtual string CalculateCheckSum(string input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      string result;

      var encoding = this.GetEncoding();
      var bytes = encoding.GetBytes(input);
      using (var memoryStream = new MemoryStream(bytes))
      {
        result = this._checkSumCalculator.CalculateCheckSum(memoryStream);
      }

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
  }
}
