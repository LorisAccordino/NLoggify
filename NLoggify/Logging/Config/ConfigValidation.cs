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
        /// <param name="filePath">The path to validate</param>
        /// <returns></returns>
        /// <exception cref="IOException">This given path is invalid</exception>
        public static bool ValidatePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            if (Uri.IsWellFormedUriString(filePath, UriKind.RelativeOrAbsolute)) return true;
            //if (Path.Exists(filePath)) return true;
            throw new IOException($"Inexistent path! Cannot access to the given path: {filePath}");
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
