namespace Primes.Play.Generate;

public class Processor
{
    private static bool IsPrime(ulong number)
    {
        if (ulong.IsEvenInteger(number)) return false;
        if (number == 1) return false;
        if (number == 2) return true;

        var limit = Math.Ceiling(Math.Sqrt(number)); //hoisting the loop limit

        for (int i = 2; i <= limit; ++i)
        {
            if (number % (ulong)i == 0) return false;
        }

        return true;
    }

    public static IEnumerable<ulong> Process(ulong baseNumber, ulong rangeLength)
    {
        ulong currentNumber = baseNumber;
        List<ulong> primes = [];

        while (currentNumber < baseNumber + rangeLength)
        {
            if (Processor.IsPrime(currentNumber))
            {
                Console.WriteLine(currentNumber);
                primes.Add(currentNumber);
            }

            currentNumber++;
        }

        return primes;
    }
}