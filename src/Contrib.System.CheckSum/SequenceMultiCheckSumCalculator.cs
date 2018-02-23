﻿/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::JetBrains.Annotations;

  public partial class SequenceMultiCheckSumCalculator : ICheckSumCalculator<IEnumerable>
  {
    /// <exception cref="ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/></exception>
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

    /// <inheritdoc />
    public virtual string CalculateCheckSum(IEnumerable input)
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