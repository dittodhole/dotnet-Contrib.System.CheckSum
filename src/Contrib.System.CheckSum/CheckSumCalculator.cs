/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::JetBrains.Annotations;

#if CONTRIB_SYSTEM_CHECKSUM
  public
 #else
  internal
#endif
  partial interface ICheckSumCalculator<T>
  {
    /// <exception cref="ArgumentNullException"><paramref name="input"/> is <see langword="null" /></exception>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    string CalculateCheckSum([NotNull] T input);
  }

#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial interface ICheckSumCalculatorEx<T>
  {
    /// <exception cref="ArgumentNullException"><paramref name="input"/> is <see langword="null" /></exception>
    /// <exception cref="Exception"/>
    [Pure]
    [CanBeNull]
    string GetStoredCheckSum([NotNull] T input);

    /// <exception cref="ArgumentNullException"><paramref name="input"/> is <see langword="null" /></exception>
    /// <exception cref="Exception"/>
    [NotNull]
    string CalculateAndStoreCheckSum([NotNull] T input);

    /// <exception cref="ArgumentNullException"><paramref name="input"/> is <see langword="null" /></exception>
    /// <exception cref="Exception"/>
    [Pure]
    bool IsStoredCheckSumValid([NotNull] T input);
  }
}
