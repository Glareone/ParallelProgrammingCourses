using System;
using System.Collections.Generic;
using System.Threading;

namespace Test_task_1
{
    public class Math
    {
        /// <summary>
        ///     The array of searching minimum value
        /// </summary>
        private double[] _arrayOfSearchingMinValue;

        /// <summary>
        ///     The thread pool
        /// </summary>
        private IList<Thread> _threadPool;

        /// <summary>
        /// The thread results
        /// </summary>
        private IList<double> _threadResults;

        /// <summary>
        /// The handle
        /// </summary>
        private static AutoResetEvent[] _autoResetEventHandles;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Math" /> class.
        /// </summary>
        /// <param name="threadPoolLength">Length of the thread pool.</param>
        /// <param name="arrayLength">Length of the array.</param>
        public Math(int threadPoolLength = 1, int arrayLength = 10000000)
        {
            InitializeThreadPool(threadPoolLength);

            InitializeWaitHandle(threadPoolLength);

            InitializeSearchingArray(arrayLength);

            MinimumValueSync = 1.0;
        }

        /// <summary>
        ///     Gets the minimum value.
        /// </summary>
        /// <value>
        ///     The minimum value.
        /// </value>
        public double MinimumValueSync { get; private set; }

        /// <summary>
        /// Gets the minimum value asynchronous.
        /// </summary>
        /// <value>
        /// The minumum value asynchronous.
        /// </value>
        public double MinimumValueAsync
        {
            get
            {
                var minimumValue = 1.0;
                foreach (var currentThreadResult in _threadResults)
                {
                    if (currentThreadResult < minimumValue)
                    {
                        minimumValue = currentThreadResult;
                    }
                }

                return minimumValue;
            }
        }

        /// <summary>
        ///     Finds the minimum value in array.
        /// </summary>
        private void FindMinimumValueInArraySync()
        {
            foreach (var value in _arrayOfSearchingMinValue)
                if (value < MinimumValueSync)
                {
                    var newMinValue = value;
                    MinimumValueSync = newMinValue;
                }
        }

        /// <summary>
        /// Finds the minimum value in array asynchronous.
        /// </summary>
        private void FindMinimumValueInArrayAsync()
        {
            var arraySegmentsCount = _threadPool.Count;

            for (var indexMultiplier = 0; indexMultiplier < _threadPool.Count; indexMultiplier++)
            {
                var segmentStartIndex = _arrayOfSearchingMinValue.Length / arraySegmentsCount * indexMultiplier;
                var segmentEndIndex = _arrayOfSearchingMinValue.Length / arraySegmentsCount * (indexMultiplier + 1);
                var threadNumber = indexMultiplier;

                int[] currentThreadCombinedParameters = {segmentStartIndex, segmentEndIndex, threadNumber};

                var currentThread = _threadPool[indexMultiplier];

                currentThread.Start(currentThreadCombinedParameters);
            }
        }

        /// <summary>
        /// Finds the minimum value in array asynchronous.
        /// </summary>
        public TimeSpan GetMinimumValueSearchTimeAsync()
        {
            var startDateTime = DateTime.Now;

            FindMinimumValueInArrayAsync();

            WaitHandle.WaitAll(_autoResetEventHandles);
            return startDateTime - DateTime.Now;
        }

        /// <summary>
        /// Gets the search time synchronize.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetMinimumValueSearchTimeSync()
        {
            var startDateTime = DateTime.Now;

            FindMinimumValueInArraySync();

            return startDateTime - DateTime.Now;
        }

        /// <summary>
        ///     Initializes the thread pool.
        /// </summary>
        /// <param name="threadPoolLength">Length of the thread pool.</param>
        private void InitializeThreadPool(int threadPoolLength)
        {
            _threadPool = new List<Thread>();
            _threadResults = new List<double>();

            for (var i = 0; i < threadPoolLength; i++)
            {
                var thread = new Thread(FindMinValueWithStartEndArrayIndexes);

                _threadPool.Add(thread);
                _threadResults.Add(1.0);
            }

        }

        /// <summary>
        /// Initializes the event handle.
        /// </summary>
        /// <param name="threadCount">The thread count.</param>
        private void InitializeWaitHandle(int threadCount)
        {
            //_autoResetEventHandles = new[] { new AutoResetEvent(false), new AutoResetEvent(false), new AutoResetEvent(false), new AutoResetEvent(false) };
            _autoResetEventHandles = new AutoResetEvent[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                _autoResetEventHandles[i] = new AutoResetEvent(false);
            }
        }

        /// <summary>
        /// Initializes the searching array.
        /// </summary>
        /// <param name="arrayLength">Length of the array.</param>
        private void InitializeSearchingArray(int arrayLength)
        {
            _arrayOfSearchingMinValue = new double[arrayLength];

            var random = new Random();

            for (var i = 0; i < arrayLength; i++)
            {
                var value = random.NextDouble();
                _arrayOfSearchingMinValue[i] = value;
            }
        }

        /// <summary>
        /// Finds the minimum value with start end array indexes.
        /// </summary>
        /// <param name="startIndexEndIndexThreadNumber">The start index end index thread number.</param>
        /// <returns></returns>
        private void FindMinValueWithStartEndArrayIndexes(object startIndexEndIndexThreadNumber)
        {
            try
            {
                var startIndex = ((int[])startIndexEndIndexThreadNumber)[0];
                var endIndex = ((int[])startIndexEndIndexThreadNumber)[1];
                var threadNumber = ((int[])startIndexEndIndexThreadNumber)[2];



                var foundedMinimumValue = 1.0;
                for (var i = startIndex; i < endIndex; i++)
                    if (_arrayOfSearchingMinValue[i] < foundedMinimumValue)
                        foundedMinimumValue = _arrayOfSearchingMinValue[i];

                _threadResults[threadNumber] = foundedMinimumValue;
                _autoResetEventHandles[threadNumber].Set();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}