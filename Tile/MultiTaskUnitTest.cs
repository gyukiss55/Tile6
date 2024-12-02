using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    internal class MultiTaskUnitTest
    {
        public void Execute () {
            TaskExecuter();
        }
        async Task TaskExecuter () { 

            // Define ranges for prime number calculation
            var ranges = new List<(int start, int end)>
            {
                (1, 1000000),
                (1000001, 2000000),
                (2000001, 3000000),
            };

            // Start async tasks for each range
            var tasks = ranges.Select(range => Task.Run(() => FindPrimesInRange(range.start, range.end))).ToList();

            // Await results
            var results = await Task.WhenAll(tasks);

            // Combine results (optional) or process each range separately
            foreach (var (range, primes) in ranges.Zip(results, (range, primes) => (range, primes)))
            {
                Console.WriteLine($"Range {range.start}-{range.end}: Found {primes.Count} primes");
            }
        }
        static List<int> FindPrimesInRange(int start, int end)
        {
            if (start < 2) start = 2;

            // Sieve of Eratosthenes algorithm
            int rangeSize = end - start + 1;
            bool[] isPrime = Enumerable.Repeat(true, rangeSize).ToArray();

            for (int i = 2; i * i <= end; i++)
            {
                int firstMultiple = Math.Max(i * i, (start + i - 1) / i * i); // Ensure we start within range
                for (int j = firstMultiple; j <= end; j += i)
                {
                    isPrime[j - start] = false;
                }
            }

            // Collect primes
            var primes = new List<int>();
            for (int i = 0; i < rangeSize; i++)
            {
                if (isPrime[i])
                    primes.Add(start + i);
            }

            return primes;
        }
    }
}
