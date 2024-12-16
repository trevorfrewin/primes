namespace Primes.Play.Generate;

public class Processor
{
    private static bool IsPrime(double number)
    {
        if (double.IsEvenInteger(number)) return false;
        if (number == 1) return false;
        if (number == 2) return true;

        var limit = Math.Ceiling(Math.Sqrt(number)); //hoisting the loop limit

        for (int i = 2; i <= limit; ++i)  
        if (number % i == 0)  
            return false;
        return true;
    }

    public static IEnumerable<double> Process (double baseNumber, int rangeLength)
    {
        double currentNumber = baseNumber;
        List<double> primes = [];

        while(currentNumber < baseNumber + rangeLength)
        {
            if (Processor.IsPrime(currentNumber))
            {
                primes.Add(currentNumber);
            }

            currentNumber++;
        }

        return primes;
    }
}