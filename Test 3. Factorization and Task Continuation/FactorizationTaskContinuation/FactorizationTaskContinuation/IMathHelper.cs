using System.Collections.Generic;

namespace FactorizationTaskContinuation
{
    public interface IMathHelper
    {
        /// <summary>
        ///     Finds the factors.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        List<long> FindFactors(long num);
    }
}