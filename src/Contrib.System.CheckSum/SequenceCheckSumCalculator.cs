﻿/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::System.Text;
  using global::JetBrains.Annotations;

  /// <inheritdoc/>
  [PublicAPI]
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class SequenceCheckSumCalculator : ICheckSumCalculator<IEnumerable>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.SequenceCheckSumCalculator"/> class.
    /// </summary>
    public SequenceCheckSumCalculator()
    {
      this._checkSumCalculator = new StringCheckSumCalculator();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.SequenceCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="checkSumCalculator"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/>.</exception>
    public SequenceCheckSumCalculator([NotNull] ICheckSumCalculator<string> checkSumCalculator)
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
    public virtual string CalculateCheckSum(IEnumerable input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var str = this.GetString(input);
      var result = this._checkSumCalculator.CalculateCheckSum(str);

      return result;
    }

    /// <summary>
    ///   Converts <paramref name="sequence"/> to <see cref="T:System.String"/>.
    /// </summary>
    /// <param name="sequence"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    protected virtual string GetString([NotNull] [InstantHandle] IEnumerable sequence)
    {
      var stringBuilder = new StringBuilder();

      foreach (var element in sequence)
      {
        if (element == null)
        {
          stringBuilder.AppendLine();
        }
        else
        {
          var value = element.ToString();
          stringBuilder.AppendLine(value);
        }
      }

      var result = stringBuilder.ToString();

      return result;
    }
  }
}
