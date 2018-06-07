/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::System.Collections.Generic;
  using global::JetBrains.Annotations;

  /// <inheritdoc/>
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class DictionaryCheckSumCalculator : ICheckSumCalculator<IDictionary>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="DictionaryCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="sequenceCheckSumCalculator"/>
    /// <exception cref="ArgumentNullException"><paramref name="sequenceCheckSumCalculator"/> is <see langword="null"/>.</exception>
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

    /// <inheritdoc/>
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

    /// <summary>
    ///   Converts <paramref name="dictionary"/> to <see cref="IEnumerable"/>.
    /// </summary>
    /// <param name="dictionary"/>
    /// <exception cref="Exception"/>
    [NotNull]
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

      var result = sortedDictionary.Values;

      return result;
    }

    /// <summary>
    ///   Indicates if <paramref name="key"/> should be iterated in <see cref="DictionaryCheckSumCalculator.GetSequence"/>.
    /// </summary>
    /// <param name="key"/>
    /// <exception cref="Exception"/>
    [Pure]
    protected virtual bool IterateKey([NotNull] object key)
    {
      var result = true;

      return result;
    }

    /// <summary>
    ///   Returns the value for <paramref name="key"/> in <paramref name="dictionary"/>.
    /// </summary>
    /// <param name="dictionary"/>
    /// <param name="key"/>
    /// <exception cref="Exception"/>
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

    /// <summary>
    ///   Returns a simplified string representation of <paramref name="key"/> and <paramref name="value"/> for check sum calculation.
    /// </summary>
    /// <param name="key"/>
    /// <param name="value"/>
    /// <exception cref="Exception"/>
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

  /// <inheritdoc cref="ICheckSumCalculatorEx{T}"/>
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class DictionaryCheckSumCalculatorEx : DictionaryCheckSumCalculator,
                                                 ICheckSumCalculatorEx<IDictionary>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="DictionaryCheckSumCalculatorEx"/> class.
    /// </summary>
    /// <param name="sequenceCheckSumCalculator"/>
    /// <exception cref="ArgumentNullException"><paramref name="sequenceCheckSumCalculator"/> is <see langword="null"/>.</exception>
    public DictionaryCheckSumCalculatorEx([NotNull] ICheckSumCalculator<IEnumerable> sequenceCheckSumCalculator)
      : base(sequenceCheckSumCalculator) { }

    /// <summary>
    ///   Gets or sets the key for check sum storage.
    /// </summary>
    /// <seealso cref="DictionaryCheckSumCalculatorEx.IterateKey"/>
    [CanBeNull]
    public virtual object CheckSumKey { get; set; }

    /// <inheritdoc/>
    protected override bool IterateKey(object key)
    {
      bool result;
      if (key == this.CheckSumKey)
      {
        result = false;
      }
      else
      {
        result = base.IterateKey(key);
      }

      return result;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

      var result = this.CalculateCheckSum(input);

      this.SetValue(input,
                    key,
                    result);

      return result;
    }

    /// <summary>
    ///   Sets <paramref name="value"/> for <paramref name="key"/> in <paramref name="input"/>.
    /// </summary>
    /// <param name="input"/>
    /// <param name="key"/>
    /// <param name="value"/>
    /// <exception cref="Exception"/>
    protected virtual void SetValue([NotNull] IDictionary input,
                                    [NotNull] object key,
                                    [CanBeNull] object value)
    {
      input[key] = value;
    }

    /// <inheritdoc/>
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
