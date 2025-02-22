using NLoggify.Utils;
using System.Text.RegularExpressions;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Static class that provides method to validate configurations parameters
    /// </summary>
    internal static class ConfigValidation
    {
        /// <summary>
        /// Validates whether the provided path is a valid filepath (it could be either null or inexistent)
        /// </summary>
        /// <param name="path">The path to validate</param>
        /// <returns>True if the format is valid, otherwise false.</returns>
        /// <exception cref="IOException">This given path is invalid</exception>
        public static bool ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            // Validate the path following the best logic
            if (GenericUtils.ValidatePath(path, true, true)) return true;
            throw new ArgumentException("Invalid path! Invalid format", path);
        }

        /// <summary>
        /// Validates whether the provided timestamp format is a valid DateTime format string.
        /// </summary>
        /// <param name="format">The format string to validate.</param>
        /// <exception cref="ArgumentException">This given format is invalid.</exception>
        /// <returns>True if the format is valid, otherwise false.</returns>
        public static bool ValidateTimestampFormat(string format)
        {
            // Empty format, skip
            if (string.IsNullOrEmpty(format)) return false;

            // Try parsing a sample DateTime with the provided format to validate it
            if (!DateTime.TryParseExact(DateTime.Now.ToString(format), format, null, System.Globalization.DateTimeStyles.None, out _))
            {
                // Invalid format, throw an exception
                throw new ArgumentException("Invalid timestamp format. Please provide a valid DateTime format string.");
            }

            // Valid format
            return true;
        }
    }
}
