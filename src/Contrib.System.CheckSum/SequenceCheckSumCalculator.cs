/** @pp
 * rootnamespace: System.Contrib
 */
namespace System.Contrib.CheckSum
{
  using System;
  using System.Collections;
  using System.Text;
  using JetBrains.Annotations;

  public partial class SequenceCheckSumCalculator : ICheckSumCalculator<IEnumerable>
  {
    /// <exception cref="ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/></exception>
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

    /// <inheritdoc />
    public virtual string CalculateCheckSum(IEnumerable input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var stringBuilder = new StringBuilder();
      foreach (var element in input)
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

      var checkSum = this._checkSumCalculator.CalculateCheckSum(stringBuilder.ToString());

      return checkSum;
    }
  }
}
