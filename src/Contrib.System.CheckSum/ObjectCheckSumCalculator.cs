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

  public partial class ObjectCheckSumCalculator : ICheckSumCalculator<object>
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

    /// <exception cref="InvalidOperationException" />
    /// <inheritdoc />
    public virtual string CalculateCheckSum(object input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var sequence = this.ObjectIterator(input);
      var checkSum = this._sequenceCheckSumCalculator.CalculateCheckSum(sequence);

      return checkSum;
    }

    /// <exception cref="InvalidOperationException" />
    /// <exception cref="Exception" />
    [NotNull]
    [ItemNotNull]
    private IEnumerable ObjectIterator([NotNull] object obj)
    {
      var sortedDictionary = new SortedDictionary<string, object>();

      var type = obj.GetType();
      var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

      foreach (var propertyInfo in propertyInfos)
      {
        if (!this.IteratePropertyInfo(propertyInfo))
        {
          continue;
        }

        object value;
        try
        {
          value = propertyInfo.GetValue(obj,
                                        null);
        }
        catch (Exception exception)
        {
          throw new InvalidOperationException(string.Format("Could not get value from property {0}",
                                                            propertyInfo.Name),
                                              exception);
        }

        var part = propertyInfo.Name + "=" + value;

        sortedDictionary.Add(propertyInfo.Name,
                             part);
      }

      return sortedDictionary.Values;
    }

    [Pure]
    public virtual bool IteratePropertyInfo([NotNull] PropertyInfo propertyInfo)
    {
      return true;
    }
  }

  public partial class ObjectCheckSumCalculatorEx : ObjectCheckSumCalculator,
                                                    ICheckSumCalculatorEx<object>
  {
    /// <inheritdoc />
    public ObjectCheckSumCalculatorEx([NotNull] ICheckSumCalculator<IEnumerable> sequenceCheckSumCalculator)
      : base(sequenceCheckSumCalculator)
    {
      this.CheckSumPropertyName = "CheckSum";
    }

    [CanBeNull]
    public string CheckSumPropertyName { get; set; }

    /// <inheritdoc />
    public override bool IteratePropertyInfo(PropertyInfo propertyInfo)
    {
      if (string.Equals(propertyInfo.Name,
                        this.CheckSumPropertyName,
                        StringComparison.Ordinal))
      {
        return false;
      }

      return base.IteratePropertyInfo(propertyInfo);
    }

    /// <exception cref="InvalidOperationException" />
    /// <inheritdoc />
    public virtual string GetStoredCheckSum(object input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var type = input.GetType();
      var propertyInfo = type.GetProperty(this.CheckSumPropertyName);
      if (propertyInfo == null)
      {
        throw new InvalidOperationException(string.Format("Could not find property {0} on {1}",
                                                          this.CheckSumPropertyName,
                                                          type));
      }

      object checkSum;
      try
      {
        checkSum = propertyInfo.GetValue(input,
                                         null);
      }
      catch (Exception exception)
      {
        throw new InvalidOperationException(string.Format("Could not get value from property {0}",
                                                          propertyInfo.Name),
                                            exception);
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

    /// <exception cref="InvalidOperationException" />
    /// <inheritdoc />
    public virtual string CalculateAndStoreCheckSum(object input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      var checkSum = this.CalculateCheckSum(input);

      var type = input.GetType();
      var propertyInfo = type.GetProperty(this.CheckSumPropertyName);
      if (propertyInfo == null)
      {
        throw new InvalidOperationException(string.Format("Could not find property {0} on {1}",
                                                          this.CheckSumPropertyName,
                                                          type));
      }

      try
      {
        propertyInfo.SetValue(input,
                              checkSum,
                              null);
      }
      catch (Exception exception)
      {
        throw new InvalidOperationException(string.Format("Could not set value on property {0}",
                                                          propertyInfo.Name),
                                            exception);
      }

      return checkSum;
    }

    /// <exception cref="InvalidOperationException" />
    /// <inheritdoc />
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
