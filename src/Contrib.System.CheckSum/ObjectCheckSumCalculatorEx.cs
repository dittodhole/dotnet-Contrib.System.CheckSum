/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::System.Collections;
  using global::System.Reflection;
  using global::JetBrains.Annotations;

  /// <inheritdoc cref="ICheckSumCalculatorEx{T}"/>
  [PublicAPI]
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial class ObjectCheckSumCalculatorEx : ObjectCheckSumCalculator,
                                             ICheckSumCalculatorEx<object>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="ObjectCheckSumCalculatorEx"/> class.
    /// </summary>
    public ObjectCheckSumCalculatorEx()
      : base() { }

    /// <summary>
    ///   Initializes a new instance of the <see cref="ObjectCheckSumCalculatorEx"/> class.
    /// </summary>
    /// <param name="checkSumCalculator"/>
    /// <exception cref="ArgumentNullException"><paramref name="checkSumCalculator"/> is <see langword="null"/>.</exception>
    public ObjectCheckSumCalculatorEx([NotNull] ICheckSumCalculator<IEnumerable> checkSumCalculator)
      : base(checkSumCalculator) { }

    /// <summary>
    ///   Gets or sets the property for check sum storage.
    /// </summary>
    /// <seealso cref="ObjectCheckSumCalculatorEx.IteratePropertyInfo"/>
    [CanBeNull]
    public virtual string CheckSumPropertyName { get; set; }

    /// <inheritdoc/>
    protected override bool IteratePropertyInfo(PropertyInfo propertyInfo)
    {
      bool result;
      if (string.Equals(propertyInfo.Name,
                        this.CheckSumPropertyName,
                        StringComparison.Ordinal))
      {
        result = false;
      }
      else
      {
        result = base.IteratePropertyInfo(propertyInfo);
      }

      return result;
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

    /// <summary>
    ///   Returns the property for <paramref name="propertyName"/> of <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj"/>
    /// <param name="propertyName"/>
    /// <exception cref="Exception"/>
    [Pure]
    [CanBeNull]
    protected virtual PropertyInfo GetPropertyInfo([NotNull] object obj,
                                                   [CanBeNull] string propertyName)
    {
      PropertyInfo result = null;

      var propertyInfos = this.GetPropertyInfos(obj);
      foreach (var propertyInfo in propertyInfos)
      {
        if (string.Equals(propertyInfo.Name,
                          propertyName,
                          StringComparison.Ordinal))
        {
          result = propertyInfo;
          break;
        }
      }

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

    /// <summary>
    ///   Sets <paramref name="value"/> for <paramref name="propertyInfo"/> in <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj"/>
    /// <param name="propertyInfo"/>
    /// <param name="value"/>
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
