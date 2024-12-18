using System.Diagnostics;
using Primes.Play.Generate;

Console.WriteLine("Generation begins!");
ulong startNumber = 0;
ulong rangeLength = 10000;

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
 
 
 9007199254748399
calculates this ^^^^ (greater/higher) Prime cleanly (and beyond!)



*/


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

