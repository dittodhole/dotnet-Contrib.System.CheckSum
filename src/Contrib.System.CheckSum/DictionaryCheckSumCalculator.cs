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
  [PublicAPI]
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class DictionaryCheckSumCalculator : ICheckSumCalculator<IDictionary>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.DictionaryCheckSumCalculator"/> class.
    /// </summary>
    public DictionaryCheckSumCalculator()
    {
      this._checkSumCalculator = new SequenceCheckSumCalculator();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.DictionaryCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="checkSumCalculator"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/>.</exception>
    public DictionaryCheckSumCalculator([NotNull] ICheckSumCalculator<IEnumerable> checkSumCalculator)
    {
      if (checkSumCalculator == null)
      {
        throw new ArgumentNullException("checkSumCalculator");
      }
      this._checkSumCalculator = checkSumCalculator;
    }

    [NotNull]
    private readonly ICheckSumCalculator<IEnumerable> _checkSumCalculator;

    /// <inheritdoc/>
    public virtual string CalculateCheckSum(IDictionary input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var sequence = this.GetSequence(input);
      var result = this._checkSumCalculator.CalculateCheckSum(sequence);

      return result;
    }

    /// <summary>
    ///   Converts <paramref name="dictionary"/> to <see cref="T:System.Collections.IEnumerable"/>.
    /// </summary>
    /// <param name="dictionary"/>
    /// <exception cref="T:System.Exception"/>
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
    ///   Indicates if <paramref name="key"/> should be iterated in <see cref="T:Contrib.System.CheckSum.DictionaryCheckSumCalculator.GetSequence"/>.
    /// </summary>
    /// <param name="key"/>
    /// <exception cref="T:System.Exception"/>
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
    /// <exception cref="T:System.Exception"/>
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
    /// <exception cref="T:System.Exception"/>
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
}
