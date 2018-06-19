/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::JetBrains.Annotations;

  /// <typeparam name="T"/>
  [PublicAPI]
#if CONTRIB_SYSTEM_CHECKSUM
  public
#else
  internal
#endif
  partial interface ICheckSumCalculatorEx<T>
  {
    /// <summary>
    ///   Returns the stored check sum of <paramref name="input"/>.
    /// </summary>
    /// <param name="input"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="input"/> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [CanBeNull]
    string GetStoredCheckSum([NotNull] T input);

    /// <summary>
    ///   Calculates the check sum of <paramref name="input"/> and stores it.
    /// </summary>
    /// <param name="input"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="input"/> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    string CalculateAndStoreCheckSum([NotNull] T input);

    /// <summary>
    ///   Validates the stored check sum of <paramref name="input"/>.
    /// </summary>
    /// <param name="input"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="input"/> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    bool IsStoredCheckSumValid([NotNull] T input);
  }
}
