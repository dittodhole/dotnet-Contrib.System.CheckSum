/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::JetBrains.Annotations;

  /// <inheritdoc/>
  [PublicAPI]
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class SequenceMultiCheckSumCalculator : ICheckSumCalculator<IEnumerable>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.SequenceMultiCheckSumCalculator"/> class.
    /// </summary>
    public SequenceMultiCheckSumCalculator()
    {
      this._checkSumCalculator = new StringCheckSumCalculator();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.SequenceMultiCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="checkSumCalculator"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/>.</exception>
    public SequenceMultiCheckSumCalculator([NotNull] ICheckSumCalculator<string> checkSumCalculator)
    {
      if (checkSumCalculator == null)
      {
        throw new ArgumentNullException("checkSumCalculator");
      }
      this._checkSumCalculator = checkSumCalculator;
    }

    [NotNull]
    private readonly ICheckSumCalculator<string> _checkSumCalculator;

    /// <inheritdoc/>
    public virtual string CalculateCheckSum([InstantHandle] IEnumerable input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var result = this._checkSumCalculator.CalculateCheckSum(string.Empty);

      foreach (var element in input)
      {
        result = string.Concat(result,
                               "_",
                               element);

        result = this._checkSumCalculator.CalculateCheckSum(result);
      }

      return result;
    }
  }
}
