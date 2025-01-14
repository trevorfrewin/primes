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
            new() { StartNumber = 10000, RangeLength = 10000 },
            new() { StartNumber = 100000, RangeLength = 10000 },
            new() { StartNumber = 110000, RangeLength = 10000 },
            new() { StartNumber = 111000, RangeLength = 10000 },
            new() { StartNumber = 1000000, RangeLength = 10000 },
            new() { StartNumber = 1100000, RangeLength = 10000 },
            new() { StartNumber = 1110000, RangeLength = 10000 },
            new() { StartNumber = 10000000, RangeLength = 10000 },
            new() { StartNumber = 11000000, RangeLength = 10000 },
            new() { StartNumber = 11100000, RangeLength = 10000 },
            new() { StartNumber = 100000000, RangeLength = 10000 },
            new() { StartNumber = 110000000, RangeLength = 10000 },
            new() { StartNumber = 111000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000, RangeLength = 10000 },
            new() { StartNumber = 1100000000, RangeLength = 10000 },
            new() { StartNumber = 1110000000, RangeLength = 10000 },
            new() { StartNumber = 10000000000, RangeLength = 10000 },
            new() { StartNumber = 11000000000, RangeLength = 10000 },
            new() { StartNumber = 11100000000, RangeLength = 10000 },
            new() { StartNumber = 100000000000, RangeLength = 10000 },
            new() { StartNumber = 110000000000, RangeLength = 10000 },
            new() { StartNumber = 111000000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000000, RangeLength = 10000 },
            new() { StartNumber = 1100000000000, RangeLength = 10000 },
            new() { StartNumber = 1110000000000, RangeLength = 10000 },
            new() { StartNumber = 10000000000000, RangeLength = 10000 },
            new() { StartNumber = 11000000000000, RangeLength = 10000 },
            new() { StartNumber = 11100000000000, RangeLength = 10000 },
            new() { StartNumber = 100000000000000, RangeLength = 10000 },
            new() { StartNumber = 110000000000000, RangeLength = 10000 },
            new() { StartNumber = 111000000000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1100000000000000, RangeLength = 10000 },
            new() { StartNumber = 1110000000000000, RangeLength = 10000 },
            new() { StartNumber = 10000000000000000, RangeLength = 10000 },
            new() { StartNumber = 11000000000000000, RangeLength = 10000 },
            new() { StartNumber = 11100000000000000, RangeLength = 10000 },
/*            new() { StartNumber = 100000000000000000, RangeLength = 10000 }, // at this range and beyond the individual thread for the Primes takes several minutes.
            new() { StartNumber = 110000000000000000, RangeLength = 10000 },
            new() { StartNumber = 111000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1000000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1100000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1110000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1200000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1300000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1400000000000000000, RangeLength = 10000 },
            new() { StartNumber = 1500000000000000000, RangeLength = 10000 },
            new() { StartNumber = 2000000000000000000, RangeLength = 10000 },
            new() { StartNumber = 3000000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4000000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4100000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4200000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4300000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4400000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4500000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4600000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4605000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4608000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4609500000000000000, RangeLength = 10000 },
            new() { StartNumber = 4609750000000000000, RangeLength = 10000 },
            new() { StartNumber = 4610000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4610500000000000000, RangeLength = 10000 },
            new() { StartNumber = 4611000000000000000, RangeLength = 10000 },
            new() { StartNumber = 4611500000000000000, RangeLength = 10000 } */
//            new() { StartNumber = 4612000000000000000, RangeLength = 10000 }, This range and beyond fails with divide by 0 error.
//            new() { StartNumber = 4615000000000000000, RangeLength = 10000 },
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