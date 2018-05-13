using System;
using System.Collections.Generic;
using System.Linq;

namespace QualityControlLoop.ParameterCalculation
{
    internal class ParametersCalculator
    {
        public Parameters GetParameters(IReadOnlyCollection<double> data)
        {
            var minInput = data.Min();
            var maxInput = data.Max();
            var mean = data.Average();
            var dispersion = Dispersion(data);
            var intervalCount = IntervalCount(data.Count);
            var intervals = SubIntervals(minInput, maxInput, intervalCount, data, dispersion);
            var hi2 = Hi2(intervals, data.Count);

            var parameters = new Parameters
            {
                MinInput = minInput,
                MaxInput = maxInput,
                Mean = mean,
                Dispersion = dispersion,
                IntervalCount = intervalCount,
                Intervals = intervals,
                Hi2 = hi2
            };

            return parameters;
        }

        private static double Dispersion(IReadOnlyCollection<double> data)
        {
            var average = data.Average();
            var sum = data.Select(x => Math.Pow(x - average, 2)).Sum();
            var dispersion = Math.Sqrt(sum / data.Count);

            return dispersion;
        }

        private static int IntervalCount(int dataCount)
        {
            var intervalCount = 1 + 3.322 * Math.Log10(dataCount);
            return (int)Math.Ceiling(intervalCount);
        }

        private static List<SubInterval> SubIntervals(double minInput, double maxInput, 
            int intervalCount, IReadOnlyCollection<double> data, double dispersion)
        {
            var dataIntervalSize = maxInput - minInput;
            var dataIntervalCenter = (maxInput + minInput) / 2;
            var subIntervalSize = dataIntervalSize / intervalCount;
            var subIntervals = Enumerable.Range(0, intervalCount).Select(i => new SubInterval
            {
                Index = i + 1,
                LeftBound = minInput + i * subIntervalSize,
                RightBound = minInput + (i + 1) * subIntervalSize
            }).ToList();

            subIntervals.First().LeftBound -= dataIntervalSize * 0.0001;
            subIntervals.Last().RightBound += dataIntervalSize * 0.0001;

            foreach (var subInterval in subIntervals)
            {
                subInterval.InputsCount = data.Count(x => subInterval.LeftBound <= x && x < subInterval.RightBound);
                subInterval.InputsFrequency = (double)subInterval.InputsCount / data.Count;
                subInterval.GaussExpectedFrequency = ExpectedGaussFrequency(subInterval.LeftBound, subInterval.RightBound, dataIntervalCenter, dispersion);
                subInterval.GaussExpectedCount = subInterval.GaussExpectedFrequency * data.Count;
            }

            return subIntervals;
        }

        private static double ExpectedGaussFrequency(double leftBound, double rightBound, double center, double dispersion)
        {
            var lambdaLeftBound = Math.Abs(leftBound - center) / dispersion;
            var lambdaRightBound = Math.Abs(rightBound - center) / dispersion;

            var frequencyForLeftBound = GaussExpectedFrequencies.GetFrequency(lambdaLeftBound);
            var frequencyForRightBound = GaussExpectedFrequencies.GetFrequency(lambdaRightBound);

            double expectedFrequency;
            if (leftBound < center && rightBound < center)
            {
                expectedFrequency = frequencyForLeftBound - frequencyForRightBound;
            }
            else if (leftBound < center && rightBound > center)
            {
                expectedFrequency = frequencyForLeftBound + frequencyForRightBound;
            }
            else
            {
                expectedFrequency = frequencyForRightBound - frequencyForLeftBound;
            }

            return expectedFrequency;
        }

        private static double Hi2(IEnumerable<SubInterval> intervals, int dataCount)
        {
            var hi2 = intervals.Select(i => Math.Pow(i.InputsCount - i.GaussExpectedCount, 2)).Sum() / dataCount;
            return hi2;
        }
    }
}