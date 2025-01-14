using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Primes.Play.Generate;

/*
 If the Prime Processors function loooks like this:
     private static bool IsPrime(ulong number)
    {
        if (double.IsEvenInteger(number)) return false;
        if (number == 1) return false;
        if (number == 2) return true;

        var limit = Math.Ceiling(Math.Sqrt(number)); //hoisting the loop limit

        for (int i = 2; i <= limit; ++i)
        {
            if (number % (ulong)i == 0) return false;
        }

        return true;
    }
 On my kit as at today the maximum yielded prime number (when at or beyond 8 threads for the available processors) is: 
 9007199254740881

 If I make a slight improvement to the efficient of the code - I get bigger prime numbers!
 Specifically:
    if (double.IsEvenInteger(number)) return false; ------->        if (ulong.IsEvenInteger(number)) return false;
 After this change the maximum prime number yielded is:
    (approximately!) 4612000000000000000
 which is a useful illustration of the impact of casting (and/or using the incorrect scalar utility function)

*/

if (args.Length > 0)
{
    if (string.Equals(args[0], "Pattern"))
    {
        Console.WriteLine("Generation begins - generating against standard pattern!");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        new Engine().Run();
        stopWatch.Stop();

        Console.WriteLine("Generation ends after {0}h{1:00}m{2:00}s seconds!", stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
    }

    if (string.Equals(args[0], "Summarise"))
    {
        Console.WriteLine("Summarising results!");

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        DirectoryInfo dir = new(".");
        var allResultsFiles = dir.GetFiles("*.json");
        List<PrimesSummary> results = [];

        foreach (var fileInfo in allResultsFiles)
        {
            dynamic formattedContent = await JsonFileReader.ReadAsync<dynamic>(fileInfo.FullName);

            try
            {
                results.Add(new PrimesSummary(
                    ((JsonElement)formattedContent).GetProperty("startNumber").GetUInt64(),
                    ((JsonElement)formattedContent).GetProperty("rangeLength").GetUInt64(),
                    ((JsonElement)formattedContent).GetProperty("count").GetUInt64()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unsupported JSON file found (ignored): {0}", fileInfo.FullName);
                Console.WriteLine(" Ex: {0}-{1}", ex.GetType().Name, ex.Message);
            }
        }
        stopWatch.Stop();

        var resultsCSVString = new StringBuilder();
        resultsCSVString.AppendLine("StartNumber\tCount\tRangeLenth");

        foreach(var result in results)
        {
            resultsCSVString.AppendLine(string.Format("{0}\t{1}\t{2}", result.StartNumber, result.Count, result.RangeLength));
        }

        File.WriteAllText("./summary.csv", resultsCSVString.ToString());

        Console.WriteLine("Summarising ends after {0}h{1:00}m{2:00}s seconds!", stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
    }
}
else
{
    Console.WriteLine("Generation begins - continuing file based series!");
    ulong startNumber = 0;
    ulong rangeLength = 10000;

    // Find the biggest number for which we have generated
    DirectoryInfo dir = new(".");
    FileInfo topMostFile = dir.GetFiles("*.json").OrderByDescending(p => p.CreationTime).First();

    if (topMostFile != null)
    {
        string? truncatedName = topMostFile.Name.Replace(".json", string.Empty);

        if (ulong.TryParse(truncatedName, out startNumber))
        {
            startNumber += rangeLength;
        }
    }

    var stopWatch = new Stopwatch();
    stopWatch.Start();
    new Engine().Run(startNumber, rangeLength, 3);
    stopWatch.Stop();

    Console.WriteLine("Generation ends after {0}h{1:00}m{2:00}s seconds!", stopWatch.Elapsed.Hours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
}

internal class PrimesSummary
{
    internal PrimesSummary(ulong startNumber, ulong rangeLength, ulong count)
    {
        StartNumber = startNumber;
        RangeLength = rangeLength;
        Count = count;
    }

    internal readonly ulong StartNumber;

    internal readonly ulong RangeLength;

    internal readonly ulong Count;
}

internal static class JsonFileReader
{
    public static async Task<T> ReadAsync<T>(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
#pragma warning disable CS8603 // Possible null reference return.
        return await JsonSerializer.DeserializeAsync<T>(stream);
#pragma warning restore CS8603 // Possible null reference return.
    }
}