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
            //if (PathRegexValidation(path)) return true;
            if (foo(path, true, true, true)) return true;
            throw new ArgumentException("Invalid path! Invalid format", path);
        }

        private static bool foo(string path, bool RequireDirectory, bool IncludeFileName, bool RequireFileName = false)
        {
            string root = null;
            string directory = null;
            string filename = null;
            try
            {
                // throw ArgumentException - The path parameter contains invalid characters, is empty, or contains only white spaces.
                root = Path.GetPathRoot(path);

                // throw ArgumentException - path contains one or more of the invalid characters defined in GetInvalidPathChars.
                // -or- String.Empty was passed to path.
                directory = Path.GetDirectoryName(path);

                // path contains one or more of the invalid characters defined in GetInvalidPathChars
                if (IncludeFileName) { filename = Path.GetFileName(path); }
            }
            catch (ArgumentException)
            {
                return false;
            }

            // null if path is null, or an empty string if path does not contain root directory information
            if (String.IsNullOrEmpty(root)) { return false; }

            // null if path denotes a root directory or is null. Returns String.Empty if path does not contain directory information
            if (String.IsNullOrEmpty(directory)) { return false; }

            if (RequireFileName)
            {
                // if the last character of path is a directory or volume separator character, this method returns String.Empty
                if (String.IsNullOrEmpty(filename)) { return false; }

                // check for illegal chars in filename
                if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) { return false; }
            }
            return true;
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
