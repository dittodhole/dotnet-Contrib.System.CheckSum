/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::System.Collections.Generic;
  using global::System.Reflection;
  using global::JetBrains.Annotations;

#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class ObjectCheckSumCalculator : ICheckSumCalculator<object>
  {
    /// <exception cref="ArgumentNullException"><paramref name="sequenceCheckSumCalculator"/> is <see langword="null"/></exception>
    public ObjectCheckSumCalculator([NotNull] ICheckSumCalculator<IEnumerable> sequenceCheckSumCalculator)
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
    public virtual string CalculateCheckSum(object input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var sequence = this.GetSequence(input);
      var result = this._sequenceCheckSumCalculator.CalculateCheckSum(sequence);

      return result;
    }

    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
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

      return sortedDictionary.Values;
    }

    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    protected virtual PropertyInfo[] GetPropertyInfos([NotNull] object obj)
    {
      var type = obj.GetType();
      var result = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

      return result;
    }

    /// <exception cref="Exception"/>
    [Pure]
    protected virtual bool IteratePropertyInfo([NotNull] PropertyInfo propertyInfo)
    {
      var result = true;

      return result;
    }

    /// <exception cref="Exception"/>
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

    /// <exception cref="Exception"/>
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

#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class ObjectCheckSumCalculatorEx : ObjectCheckSumCalculator,
                                             ICheckSumCalculatorEx<object>
  {
    /// <inheritdoc/>
    public ObjectCheckSumCalculatorEx([NotNull] ICheckSumCalculator<IEnumerable> sequenceCheckSumCalculator)
      : base(sequenceCheckSumCalculator) { }

    [CanBeNull]
    public virtual string CheckSumPropertyName { get; set; }

    /// <inheritdoc/>
    protected override bool IteratePropertyInfo(PropertyInfo propertyInfo)
    {
      if (string.Equals(propertyInfo.Name,
                        this.CheckSumPropertyName,
                        StringComparison.Ordinal))
      {
        return false;
      }

      return base.IteratePropertyInfo(propertyInfo);
    }

    /// <inheritdoc/>
    public virtual string GetStoredCheckSum(object input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var propertyName = this.CheckSumPropertyName;
      var propertyInfo = this.GetPropertyInfo(input,
                                              propertyName);
      if (propertyInfo == null)
      {
        throw new Exception(string.Format("Could not find property {0}",
                                          propertyName));
      }

      var checkSum = this.GetValue(input,
                                   propertyInfo);

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

    /// <exception cref="Exception"/>
    [Pure]
    [CanBeNull]
    protected virtual PropertyInfo GetPropertyInfo([NotNull] object obj,
                                                   [CanBeNull] string propertyName)
    {
      if (propertyName == null)
      {
        return null;
      }

      var type = obj.GetType();
      var result = type.GetProperty(propertyName);

      return result;
    }

    /// <inheritdoc/>
    public virtual string CalculateAndStoreCheckSum(object input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var result = this.CalculateCheckSum(input);

      var propertyName = this.CheckSumPropertyName;
      var propertyInfo = this.GetPropertyInfo(input,
                                              propertyName);
      if (propertyInfo == null)
      {
        throw new Exception(string.Format("Could not find property {0}",
                                          propertyName));
      }

      this.SetValue(input,
                    propertyInfo,
                    result);

      return result;
    }

    /// <exception cref="Exception"/>
    protected virtual void SetValue([NotNull] object obj,
                                    [NotNull] PropertyInfo propertyInfo,
                                    [CanBeNull] object value)
    {
      try
      {
        propertyInfo.SetValue(obj,
                              value,
                              null);
      }
      catch (Exception exception)
      {
        throw new Exception(string.Format("Could not set value on property {0}",
                                          propertyInfo.Name),
                            exception);
      }
    }

    /// <inheritdoc/>
    public virtual bool IsStoredCheckSumValid(object input)
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
