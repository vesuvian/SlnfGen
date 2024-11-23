using SolutionFilterGenerator.Utils;

namespace SolutionFilterGenerator;

public class Recurse
{
    /// <summary>
    /// Walks the solution directory, creating a .slnf in each directory that
    /// contains projects referenced by the solution.
    /// </summary>
    /// <param name="slnFilePath">The path to the .sln file.</param>
    /// <param name="suffix">Optional suffix to append to the generated .slnf file names.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task RecurseAsync(string slnFilePath, string? suffix = null, CancellationToken cancellationToken = default)
    {
        var slnDirectoryPath = Path.GetDirectoryName(slnFilePath)!;
        var projectFilePaths = SolutionUtils.GetProjectFilePaths(slnFilePath);
        var graph = BuildProjectFileGraph(projectFilePaths);

        foreach (var kvp in graph)
        {
            // Skip directories with only 1 project
            if (kvp.Value.Count <= 1)
                continue;

            // Skip directories outside the solution directory
            if (kvp.Key.StartsWith(".."))
                continue;

            // Build the slnf
            var directoryAbsolutePath = Path.Combine(slnDirectoryPath, kvp.Key);
            var directoryName = Path.GetFileName(directoryAbsolutePath);
            var slnfFilePath = Path.Combine(directoryAbsolutePath, $"{directoryName}{suffix}.slnf");
            var slnf = SolutionFilter.FromAbsolutePaths(slnFilePath, slnfFilePath, kvp.Value);

            // Write the file
            await File.WriteAllTextAsync(slnfFilePath, slnf.ToString(), cancellationToken);
        }
    }

    /// <summary>
    /// Builds a map of relative directory paths to relative project file paths.
    /// </summary>
    /// <param name="filePaths"></param>
    /// <returns></returns>
    private static Dictionary<string, HashSet<string>> BuildProjectFileGraph(IEnumerable<string> filePaths)
    {
        var output = new Dictionary<string, HashSet<string>>();

        foreach (var filePath in filePaths)
        {
            var parts = filePath.Split('\\', StringSplitOptions.RemoveEmptyEntries);

            for (var i = 1; i < parts.Length; i++)
            {
                var span = string.Join('\\', parts.Take(i));
                output.TryAdd(span, []);
                output[span].Add(filePath);
            }
        }

        return output;
    }
}
