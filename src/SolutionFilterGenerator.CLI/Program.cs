using CommandLine;

namespace SolutionFilterGenerator.CLI;

internal static class Program
{
    /// <summary>
    /// Options for the "recurse" command.
    /// </summary>
    [Verb("recurse", HelpText = "Recursively creates .slnf files for the given .sln.")]
    private class RecurseOptions
    {
        [Option('s', "solutionfile", Required = true, HelpText = "The path to the target .sln file.")]
        public string SolutionFile { get; set; } = null!;

        [Option('f', "filtersuffix", Required = true, HelpText = "An optional suffix to add to generated .slnf file names.")]
        public string? FilterSuffix { get; set; }
    }

    /// <summary>
    /// Entry point.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static async Task Main(string[] args) =>
        await Parser.Default
                    .ParseArguments<RecurseOptions>(args)
                    .MapResult(RecurseAsync,
                               _ => Task.FromResult(1));

    /// <summary>
    /// Recursively creates .slnf files for the given .sln.
    /// </summary>
    /// <param name="recurseOptions"></param>
    /// <returns></returns>
    private static async Task<int> RecurseAsync(RecurseOptions recurseOptions)
    {
        await Recurse.RecurseAsync(recurseOptions.SolutionFile, recurseOptions.FilterSuffix);
        return 1;
    }
}
