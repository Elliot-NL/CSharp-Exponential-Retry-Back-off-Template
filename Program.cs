using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing_Files___Exponential_Retry
{
    class Program
    {

        public static class Retry
        {
            public static void Do(
                Action action,
                TimeSpan retryInterval,
                int maxAttemptCount = 3)
            {
                Do<object>(() =>
                {
                    action();
                    return null;
                }, retryInterval, maxAttemptCount);
            }

            public static T Do<T>(
                Func<T> action,
                TimeSpan retryInterval,
                int maxAttemptCount = 3)
            {
                var exceptions = new List<Exception>();

                for (int attempted = 0; attempted < maxAttemptCount; attempted++)
                {
                    try
                    {
                        if(attempted > 0)
                        {
                            System.Threading.Thread.Sleep(retryInterval);
                        }
                        return action();
                    }
                    catch(Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
                throw new AggregateException(exceptions);
            }
        }
        
        // Calculates Fibonacci:
        public static int Fib(int fibnum)
        {
            int a = 0;
            int b = 1;

            for(int i = 0; i < fibnum; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return a;

        }

        // Runs the Fibonacci Sequence:
        // Finds the number in the fibonacci sequence when added:
        private static void RunFib()
        {
            for (int i = 0; i < 15; i++)
            {
                System.Threading.Thread.Sleep(100);
                Console.WriteLine(Fib(i));
            }
        }

        static void Main(string[] args)
        {
            /// exponential retry/backoff implementation using Fibonacci sequence
            /// Runs fibonacci sequence and displays results
            /// If it succeeds it will continue calculating.
            /// If it fails it will begin backing off. 

            // Performs the Retry Logic:
            Retry.Do(() => RunFib(), TimeSpan.FromSeconds(1));

            // Alternative:
            Retry.Do(RunFib, TimeSpan.FromSeconds(1));

            Console.ReadLine();
        }

    }
}
