using System.Text.Json;
using System.Text.Json.Serialization;

namespace Primes.Play.Generate;

public class Engine
{
    public class RunRange
    {
        [JsonInclude]
        public ulong StartNumber;

        [JsonInclude]
        public ulong RangeLength;
    }

    public void Run(ulong startNumber, ulong rangeLength, int runCount)
    {
        // Create the ranges for each thread
        var ranges = new List<RunRange>();

        for (int runNumber = 0; runNumber < runCount; runNumber++)
        {
            ranges.Add(new RunRange { StartNumber = startNumber + ((ulong)runNumber * rangeLength), RangeLength = rangeLength });
        }

        this.RunInternal(ranges);
    }

    public void Run()
    {
        // Create the ranges for each thread
        var ranges = new List<RunRange>
        {
            new() { StartNumber = 0, RangeLength = 10000 },
            new() { StartNumber = 100000, RangeLength = 10000 },
            new() { StartNumber = 1000000, RangeLength = 10000 },
            new() { StartNumber = 10000000, RangeLength = 10000 },
            new() { StartNumber = 100000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000, RangeLength = 10000 },
            new() { StartNumber = 10000000000, RangeLength = 10000 },
            new() { StartNumber = 100000000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000000, RangeLength = 10000 },
            new() { StartNumber = 10000000000000, RangeLength = 10000 },
            new() { StartNumber = 100000000000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000000000, RangeLength = 10000 },
            new() { StartNumber = 10000000000000000, RangeLength = 10000 },
            new() { StartNumber = 100000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000000000000, RangeLength = 10000 }
        };

        this.RunInternal(ranges);
    }

    private void RunInternal(IEnumerable<RunRange> ranges)
    {
        var integerPortion = (ulong)ranges.First().StartNumber;
        var json = JsonSerializer.Serialize(ranges);
        var outputFileName = $"{integerPortion}-ranges.json";
        lock (typeof(Engine))
        {
            File.WriteAllText(outputFileName, json);
        }

        // foreach on each range created
        Parallel.ForEach(ranges, range =>
        {
            try
            {
                ProcessAndSave(range.StartNumber, range.RangeLength);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing range {range.StartNumber}: {ex.Message}");
            }
        });
    }

    private static void ProcessAndSave(ulong startNumber, ulong rangeLength)
    {
        // Perform processing
        var processed = Processor.Process(startNumber, rangeLength);

        // Wrap processed output
        var wrappedProcessorOutput = new
        {
            startNumber,
            rangeLength,
            count = processed.Count(),
            processed
        };

        var json = JsonSerializer.Serialize(wrappedProcessorOutput);
        var outputFileName = $"{startNumber}.json";
        lock (typeof(Engine))
        {
            File.WriteAllText(outputFileName, json);
        }
    }
}