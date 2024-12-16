// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Primes.Play.Generate;

Console.WriteLine("Generation begins!");
Double startNumber = 0;
var rangeLength = 10000;

// Find the biggest number for which we have generated
DirectoryInfo dir = new(".");
FileInfo topMostFile = dir.GetFiles("*.json").OrderByDescending(p => p.CreationTime).First();

if (topMostFile != null)
{
    string? truncatedName = topMostFile.Name.Replace(".json", string.Empty);

    if (Double.TryParse(truncatedName, out startNumber))
    {
        startNumber += rangeLength;
    }
}

// Generate Primes for sections of 100k, for 800 more sections
var maxNumber = startNumber + (800 * rangeLength);

while (startNumber < maxNumber)
{
    var processed = Processor.Process(startNumber, rangeLength);
    var wrappedProcessorOutput = new { startNumber, rangeLength, count = processed.Count(), processed };
    var json = JsonSerializer.Serialize(wrappedProcessorOutput);
    var outputfilename = string.Format(@"{0}.json", startNumber);
    File.WriteAllText(outputfilename, json);
    startNumber += rangeLength;
}

Console.WriteLine("Generation ends!");

