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
  partial interface ICheckSumCalculator<T>
  {
    /// <summary>
    ///   Calculates the check sum of <paramref name="input"/>.
    /// </summary>
    /// <param name="input"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="input"/> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    string CalculateCheckSum([NotNull] T input);
  }
}
