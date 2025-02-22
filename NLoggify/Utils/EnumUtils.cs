namespace NLoggify.Utils
{
    /// <summary>
    /// Utility class that provides utility methods for enums.
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// Get the values of the given enum
        /// </summary>
        /// <typeparam name="T">Type of the enum to cast</typeparam>
        /// <returns>The values of the enum as a <see cref="IEnumerable{T}"/></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
