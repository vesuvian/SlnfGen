using Microsoft.Build.Construction;

namespace SolutionFilterGenerator.Utils;

public static class SolutionUtils
{
    /// <summary>
    /// Gets the projects referenced by the given solution file.
    /// </summary>
    /// <param name="slnFilePath"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetProjectFilePaths(string slnFilePath) =>
        SolutionFile.Parse(slnFilePath)
                    .ProjectsInOrder
                    .Where(p => p.ProjectType != SolutionProjectType.SolutionFolder)
                    .Select(p => p.RelativePath)
                    .Distinct()
                    .Order();
}
