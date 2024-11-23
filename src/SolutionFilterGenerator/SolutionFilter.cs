using System.Text.Json;
using SolutionFilterGenerator.Utils;

namespace SolutionFilterGenerator
{
    public class SolutionFilter
    {
        public SolutionFilterSolution Solution { get; set; } = new();

        /// <summary>
        /// Creates a solution filter from the given absolute paths.
        /// </summary>
        /// <param name="slnFilePath"></param>
        /// <param name="slnfFilePath"></param>
        /// <param name="projectFilePaths"></param>
        /// <returns></returns>
        public static SolutionFilter FromAbsolutePaths(string slnFilePath, string slnfFilePath,
                                                       IEnumerable<string> projectFilePaths)
        {
            var slnfDirectory = Path.GetDirectoryName(slnfFilePath)!;

            return new SolutionFilter
            {
                Solution =
                {
                    Path = PathUtils.GetRelativePath(slnfDirectory, slnFilePath),
                    Projects = projectFilePaths.Distinct().Order().ToList()
                }
            };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Serialize(this, options);
        }
    }

    public class SolutionFilterSolution
    {
        /// <summary>
        /// The relative path from the .slnf to the .sln
        /// </summary>
        public string Path { get; set; } = null!;

        /// <summary>
        /// The relative paths from the .sln to the individual projects.
        /// </summary>
        public List<string> Projects { get; set; } = new();
    }
}
