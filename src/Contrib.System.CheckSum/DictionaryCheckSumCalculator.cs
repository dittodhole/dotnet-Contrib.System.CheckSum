/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::System.Collections.Generic;
  using global::JetBrains.Annotations;

#if CHECKSUM_PUBLIC
  public
#else
  internal
#endif
  partial class DictionaryCheckSumCalculator : ICheckSumCalculator<IDictionary>
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

      var sequence = this.GetSequence(input);
      var result = this._sequenceCheckSumCalculator.CalculateCheckSum(sequence);

      return result;
    }

    /// <exception cref="Exception" />
    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable GetSequence([NotNull] IDictionary dictionary)
    {
      var sortedDictionary = new SortedDictionary<object, string>();

      foreach (var key in dictionary.Keys)
      {
        if (!this.IterateKey(key))
        {
          continue;
        }

        var value = this.GetValue(dictionary,
                                  key);
        var part = this.GetPart(key,
                                value);

        sortedDictionary[key] = part;
      }

      return sortedDictionary.Values;
    }

    /// <exception cref="Exception" />
    [Pure]
    protected virtual bool IterateKey([NotNull] object key)
    {
      var result = true;

      return result;
    }

    /// <exception cref="Exception" />
    [Pure]
    [CanBeNull]
    protected virtual object GetValue([NotNull] IDictionary dictionary,
                                      [NotNull] object key)
    {
      object result;
      if (dictionary.Contains(key))
      {
        result = dictionary[key];
      }
      else
      {
        result = null;
      }

      return result;
    }

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    protected virtual string GetPart([NotNull] object key,
                                     [CanBeNull] object value)
    {
      var result = string.Concat(key,
                                 "=",
                                 value);

      return result;
    }
  }

#if CHECKSUM_PUBLIC
  public
#else
  internal
#endif
  partial class DictionaryCheckSumCalculatorEx : DictionaryCheckSumCalculator,
                                                 ICheckSumCalculatorEx<IDictionary>
  {
    /// <inheritdoc />
    public DictionaryCheckSumCalculatorEx([NotNull] ICheckSumCalculator<IEnumerable> sequenceCheckSumCalculator)
      : base(sequenceCheckSumCalculator) { }

    [CanBeNull]
    public virtual object CheckSumKey { get; set; }

    /// <inheritdoc />
    protected override bool IterateKey(object key)
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

      var key = this.CheckSumKey;
      if (key == null)
      {
        throw new Exception("Key is null");
      }

      var checkSum = this.GetValue(input,
                                   key);

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

      var key = this.CheckSumKey;
      if (key == null)
      {
        throw new Exception("Key is null");
      }

      var checkSum = this.CalculateCheckSum(input);

      this.SetValue(input,
                    key,
                    checkSum);

      return checkSum;
    }

    /// <exception cref="Exception" />
    protected virtual void SetValue([NotNull] IDictionary input,
                                    [NotNull] object key,
                                    [CanBeNull] object value)
    {
      input[key] = value;
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
