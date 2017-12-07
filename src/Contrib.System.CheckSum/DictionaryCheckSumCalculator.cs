/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::System.Collections.Generic;
  using global::JetBrains.Annotations;

  public partial class DictionaryCheckSumCalculator : ICheckSumCalculator<IDictionary>
  {
    /// <exception cref="ArgumentNullException"><paramref name="sequenceCheckSumCalculator"/> is <see langword="null"/></exception>
    public DictionaryCheckSumCalculator([NotNull] ICheckSumCalculator<IEnumerable> sequenceCheckSumCalculator)
    {
      if (sequenceCheckSumCalculator == null)
      {
        throw new ArgumentNullException("sequenceCheckSumCalculator");
      }
      this._sequenceCheckSumCalculator = sequenceCheckSumCalculator;
    }

    [NotNull]
    private readonly ICheckSumCalculator<IEnumerable> _sequenceCheckSumCalculator;

    /// <inheritdoc />
    public virtual string CalculateCheckSum(IDictionary input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var sequence = this.DictionaryIterator(input);
      var checkSum = this._sequenceCheckSumCalculator.CalculateCheckSum(sequence);

      return checkSum;
    }

    /// <exception cref="Exception" />
    [NotNull]
    [ItemNotNull]
    private IEnumerable DictionaryIterator([NotNull] IDictionary dictionary)
    {
      var sortedDictionary = new SortedDictionary<object, string>();

      foreach (var key in dictionary.Keys)
      {
        if (!this.IterateKey(key))
        {
          continue;
        }

        var value = dictionary[key];
        var part = key + "=" + value;

        sortedDictionary.Add(key,
                             part);
      }

      return sortedDictionary.Values;
    }

    [Pure]
    public virtual bool IterateKey([NotNull] object key)
    {
      return true;
    }
  }

  public partial class DictionaryCheckSumCalculatorEx : DictionaryCheckSumCalculator,
                                                        ICheckSumCalculatorEx<IDictionary>
  {
    /// <inheritdoc />
    public DictionaryCheckSumCalculatorEx([NotNull] ICheckSumCalculator<IEnumerable> sequenceCheckSumCalculator)
      : base(sequenceCheckSumCalculator)
    {
      this.CheckSumKey = "CRC";
    }

    [CanBeNull]
    public object CheckSumKey { get; set; }

    /// <inheritdoc />
    public override bool IterateKey(object key)
    {
      if (key == this.CheckSumKey)
      {
        return false;
      }

      return base.IterateKey(key);
    }

    /// <inheritdoc />
    public virtual string GetStoredCheckSum(IDictionary input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      object checkSum;
      if (input.Contains(this.CheckSumKey))
      {
        checkSum = input[this.CheckSumKey];
      }
      else
      {
        checkSum = null;
      }

      string result;
      if (checkSum == null)
      {
        result = null;
      }
      else
      {
        result = checkSum.ToString();
      }

      return result;
    }

    /// <inheritdoc />
    public virtual string CalculateAndStoreCheckSum(IDictionary input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var checkSum = this.CalculateCheckSum(input);
      input[this.CheckSumKey] = checkSum;

      return checkSum;
    }

    /// <inheritdoc />
    public virtual bool IsStoredCheckSumValid(IDictionary input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var actualCheckSum = this.GetStoredCheckSum(input);
      var expectedCheckSum = this.CalculateCheckSum(input);

      var result = string.Equals(expectedCheckSum,
                                 actualCheckSum,
                                 StringComparison.Ordinal);

      return result;
    }
  }
}
