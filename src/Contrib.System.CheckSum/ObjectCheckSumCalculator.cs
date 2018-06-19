/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::System.Collections.Generic;
  using global::System.Reflection;
  using global::JetBrains.Annotations;

  /// <inheritdoc/>
  [PublicAPI]
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class ObjectCheckSumCalculator : ICheckSumCalculator<object>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.ObjectCheckSumCalculator"/> class.
    /// </summary>
    public ObjectCheckSumCalculator()
    {
      this._checkSumCalculator = new SequenceCheckSumCalculator();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.CheckSum.ObjectCheckSumCalculator"/> class.
    /// </summary>
    /// <param name="checkSumCalculator"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/>.</exception>
    public ObjectCheckSumCalculator([NotNull] ICheckSumCalculator<IEnumerable> checkSumCalculator)
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
    public virtual string CalculateCheckSum(object input)
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
    ///   Converts <paramref name="obj"/> to <see cref="T:System.Collections.IEnumerable"/>.
    /// </summary>
    /// <param name="obj"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    protected virtual IEnumerable GetSequence([NotNull] object obj)
    {
      var sortedDictionary = new SortedDictionary<string, object>(StringComparer.Ordinal);

      var propertyInfos = this.GetPropertyInfos(obj);
      foreach (var propertyInfo in propertyInfos)
      {
        if (!this.IteratePropertyInfo(propertyInfo))
        {
          continue;
        }

        var value = this.GetValue(obj,
                                  propertyInfo);
        var part = this.GetPart(propertyInfo,
                                value);

        sortedDictionary[propertyInfo.Name] = part;
      }

      var result = sortedDictionary.Values;

      return result;
    }

    /// <summary>
    ///   Returns all <see cref="T:System.Reflection.PropertyInfo"/> classes for <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    protected virtual PropertyInfo[] GetPropertyInfos([NotNull] object obj)
    {
      var type = obj.GetType();
      var result = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

      return result;
    }

    /// <summary>
    ///   Indicates if <paramref name="propertyInfo"/> should be iterated in <see cref="M:Contrib.System.CheckSum.ObjectCheckSumCalculator.GetSequence(System.Object)"/>.
    /// </summary>
    /// <param name="propertyInfo"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    protected virtual bool IteratePropertyInfo([NotNull] PropertyInfo propertyInfo)
    {
      var result = true;

      return result;
    }

    /// <summary>
    ///   Returns the value for <paramref name="propertyInfo"/> in <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj"/>
    /// <param name="propertyInfo"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [CanBeNull]
    protected virtual object GetValue([NotNull] object obj,
                                      [NotNull] PropertyInfo propertyInfo)
    {
      object result;
      try
      {
        result = propertyInfo.GetValue(obj,
                                       null);
      }
      catch (Exception exception)
      {
        throw new Exception(string.Format("Could not get value from property {0}",
                                          propertyInfo.Name),
                            exception);
      }

      return result;
    }

    /// <summary>
    ///   Returns a simplified string representation of <paramref name="propertyInfo"/> and <paramref name="value"/> for check sum calculation.
    /// </summary>
    /// <param name="propertyInfo"/>
    /// <param name="value"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    protected virtual string GetPart([NotNull] PropertyInfo propertyInfo,
                                     [CanBeNull] object value)
    {
      var result = string.Concat(propertyInfo.Name,
                                 "=",
                                 value);

      return result;
    }
  }
}
