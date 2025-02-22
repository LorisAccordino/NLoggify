using System.IO;
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

            // Validate the path through a regex
            if (PathRegexValidation(path)) return true;
            throw new IOException($"Inexistent path! Cannot access to the given path: {path}");
        }

        /// <summary>
        /// Validate a filepath through a regex
        /// </summary>
        /// <param name="path">The path to validate</param>
        /// <returns>True if the format is valid, otherwise false.</returns>
        private static bool PathRegexValidation(string path)
        {
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (!driveCheck.IsMatch(path.Substring(0, 3))) return false;
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
                return false;

            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(path));
            if (!dir.Exists)
                dir.Create();
            return true;
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
