/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::JetBrains.Annotations;

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
    /// <param name="checkSumCalculator"/>
    /// <exception cref="ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/>.</exception>
    public DictionaryCheckSumCalculatorEx([NotNull] ICheckSumCalculator<IEnumerable> checkSumCalculator)
      : base(checkSumCalculator) { }

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
