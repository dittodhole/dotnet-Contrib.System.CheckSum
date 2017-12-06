/** @pp
 * rootnamespace: System.Contrib
 */
namespace System.Contrib.CheckSum
{
  using System;
  using JetBrains.Annotations;

  public partial interface ICheckSumCalculator<T>
  {
    /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" /></exception>
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    string CalculateCheckSum([NotNull] T input);
  }

  public partial interface ICheckSumCalculatorEx<T>
  {
    /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" /></exception>
    /// <exception cref="Exception" />
    [Pure]
    [CanBeNull]
    string GetStoredCheckSum([NotNull] T input);

    /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" /></exception>
    /// <exception cref="Exception" />
    [NotNull]
    string CalculateAndStoreCheckSum([NotNull] T input);

    /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" /></exception>
    /// <exception cref="Exception" />
    [Pure]
    bool IsStoredCheckSumValid([NotNull] T input);
  }
}
