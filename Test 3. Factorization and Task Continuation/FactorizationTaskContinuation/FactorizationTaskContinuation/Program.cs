using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FactorizationTaskContinuation
{
    internal class Program
    {
        /// <summary>
        /// Gets or sets the math helper.
        /// </summary>
        /// <value>
        /// The math helper.
        /// </value>
        private static IMathHelper MathHelper { get; set; }

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="Exception">input value cannot be parsed</exception>
        private static void Main(string[] args)
        {
            Initialize();

            Console.WriteLine("Enter your number for factorization");
            var inputString = Console.ReadLine();
            if (inputString != null)
            {
                if (long.TryParse(inputString, out var value))
                {
                    var listOfFactorizationMultipliers = Factorization(value);
                    var listOfFactorizationMultipliersTask = FactorizationSingleTask(value);
                    var listOfFactorizationMultipliersFakeAsync = FactorizationFakeAsync(value).Result;
                    var listOfFactorizationMultipliersAsync = FactorizationAsync(value).Result;

                    PrintResult(listOfFactorizationMultipliers, "Base Factorization Calculation");
                    PrintResult(listOfFactorizationMultipliersTask, "Single Task Factorization Calculation");
                    PrintResult(listOfFactorizationMultipliersFakeAsync, "Fake Async Task Factorization Calculation");
                    PrintResult(listOfFactorizationMultipliersAsync, "Fake Async Task Factorization Calculation");

                    Console.ReadKey();
                }
                else
                {
                    throw new Exception("input value cannot be parsed");
                }
            }
            else
            {
                Console.WriteLine("input string is null. Factorization calculation stopped...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private static void Initialize()
        {
           MathHelper = new MathHelper();
        }

        /// <summary>
        /// Prints the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listOfValues">The list of values.</param>
        /// <param name="helpInformation">The help information.</param>
        private static void PrintResult<T>(IEnumerable<T> listOfValues, string helpInformation = null)
        {
            Console.WriteLine(helpInformation == null ? "Result is:" : $@"{helpInformation}, Result is:");
            Console.WriteLine(string.Join(" * ", listOfValues));
        }

        /// <summary>
        /// Factorizations the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static List<long> Factorization(long value)
        {
            return MathHelper.FindFactors(value);
        }

        /// <summary>
        /// Factorizations the task.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static List<long> FactorizationSingleTask(long value)
        {
            var task = Task.Factory.StartNew(() => MathHelper.FindFactors(value));
            return task.Result;
        }

        /// <summary>
        /// Factorizations the fake asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static Task<List<long>> FactorizationFakeAsync(long value)
        {
            var tcs = new TaskCompletionSource<List<long>>();
            Task.Run(() =>
            {
                var result = MathHelper.FindFactors(value);
                tcs.SetResult(result);
            });

            return tcs.Task;
        }

        /// <summary>
        /// Factorizations the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static Task<List<long>> FactorizationAsync(long value)
        {
            var tcs = new TaskCompletionSource<List<long>>();
            Task.Run(() =>
            {
                List<long> result;
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    result = MathHelper.FindFactors(value);
                    tcs.SetResult(result);
                }).Start();
            });

            return tcs.Task;
        }

    }
}