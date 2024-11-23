namespace SolutionFilterGenerator.Utils
{
    public static class PathUtils
    {
        /// <summary>
        /// Gets the relative path from the base path to the full path.
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string basePath, string fullPath)
        {
            basePath = Path.EndsInDirectorySeparator(basePath) ? basePath : basePath + Path.DirectorySeparatorChar;
            var baseUri = new Uri(basePath);
            var fullUri = new Uri(fullPath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
