namespace NLoggify.Utils
{
    /// <summary>
    /// Utility class that provides miscellanous utility methods.
    /// </summary>
    internal static class GenericUtils
    {
        /// <summary>
        /// Validate a given path with a robust series of checks
        /// </summary>
        /// <param name="path">Path to validate</param>
        /// <param name="IncludeFileName">Does the path include filename?</param>
        /// <param name="RequireFileName">Is the filename required?</param>
        /// <returns>True if the format is valid, otherwise false.</returns>
        public static bool ValidatePath(string path, bool IncludeFileName, bool RequireFileName = false)
        {
            if (string.IsNullOrEmpty(path)) return false;

            string? root = null;
            string? directory = null;
            string? filename = null;

            // Throw ArgumentException - The path parameter contains invalid characters, is empty, or contains only white spaces.
            root = Path.GetPathRoot(path);

            // Throw ArgumentException - path contains one or more of the invalid characters defined in GetInvalidPathChars.
            // -OR- String.Empty was passed to path.
            directory = Path.GetDirectoryName(path);

            // Path contains one or more of the invalid characters defined in GetInvalidPathChars
            if (IncludeFileName) { filename = Path.GetFileName(path); }

            // Null if path is null, or an empty string if path does not contain root directory information
            if (string.IsNullOrEmpty(root)) { return false; }

            // Null if path denotes a root directory or is null. Returns String.Empty if path does not contain directory information
            if (string.IsNullOrEmpty(directory)) { return false; }

            if (RequireFileName)
            {
                // If the last character of path is a directory or volume separator character, this method returns String.Empty
                if (string.IsNullOrEmpty(filename)) { return false; }

                // Check for illegal chars in filename
                if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Ensures that the given string is a valid filename by replacing invalid characters.
        /// </summary>
        /// <param name="filename">The filename to validate.</param>
        /// <returns>A valid filename or an empty string if the correction is impossible.</returns>
        //[ExcludeFromCodeCoverage] // No reason to test it
        public static string MakeValidFilename(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return string.Empty;

            char[] invalidChars = Path.GetInvalidFileNameChars();
            string validFilename = new string(filename.Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());

            return string.IsNullOrWhiteSpace(validFilename) ? string.Empty : validFilename;
        }

        /// <summary>
        /// Get the values of the given enum
        /// </summary>
        /// <typeparam name="T">Type of the enum to cast</typeparam>
        /// <returns>The values of the enum as a <see cref="IEnumerable{T}"/></returns>
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
