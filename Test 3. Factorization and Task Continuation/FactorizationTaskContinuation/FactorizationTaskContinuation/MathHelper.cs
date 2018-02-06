using System;
using System.Collections.Generic;

namespace FactorizationTaskContinuation
{
    public class MathHelper : IMathHelper
    {
        /// <summary>
        ///     Finds the factors.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public List<long> FindFactors(long num)
        {
            var result = new List<long>();

            try
            {
                // Base getting
                // Get all multipliers equal 2.
                while (num % 2 == 0)
                {
                    result.Add(2);
                    num /= 2;
                }

                // Extended getting
                // Take all other multipliers.
                long factor = 3;
                while (factor * factor <= num)
                    if (num % factor == 0)
                    {
                        // factor found
                        result.Add(factor);
                        num /= factor;
                    }
                    else
                    {
                        // multiplier incrementation to find new one.
                        factor += 2;
                    }

                // If num is not 1, then whatever is left is multiplier.
                if (num > 1) result.Add(num);

                return result;
            }
            catch
            {
                throw new Exception("Calculation exception");
            }
        }
    }
}