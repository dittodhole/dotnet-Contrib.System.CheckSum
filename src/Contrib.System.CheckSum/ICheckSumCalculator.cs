/** @pp
 * rootnamespace: Contrib.System.CheckSum
 */
namespace Contrib.System.CheckSum
{
  using global::System;
  using global::JetBrains.Annotations;

  /// <summary>
  ///
  /// </summary>
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
    /// <exception cref="ArgumentNullException"><paramref name="input"/> is <see langword="null" />.</exception>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    string CalculateCheckSum([NotNull] T input);
  }
}
