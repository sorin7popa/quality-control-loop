using System;
using System.Collections.Generic;

namespace QualityControlLoop
{
    internal class GaussExpectedFrequencies
    {
        private static readonly Dictionary<double, double> Frequencies =
            new Dictionary<double, double>
            {
                { 0.0, 0.0000 },
                { 0.1, 0.0398 },
                { 0.2, 0.0793 },
                { 0.3, 0.1179 },
                { 0.4, 0.1554 },
                { 0.5, 0.1915 },
                { 0.6, 0.2257 },
                { 0.7, 0.2580 },
                { 0.8, 0.2881 },
                { 0.9, 0.3159 },
                { 1.0, 0.3413 },
                { 1.1, 0.3643 },
                { 1.2, 0.3849 },
                { 1.3, 0.4032 },
                { 1.4, 0.4192 },
                { 1.5, 0.4332 },
                { 1.6, 0.4452 },
                { 1.7, 0.4554 },
                { 1.8, 0.4641 },
                { 1.9, 0.4713 },
                { 2.0, 0.4772 },
                { 2.1, 0.4821 },
                { 2.2, 0.4861 },
                { 2.3, 0.4893 },
                { 2.4, 0.4918 },
                { 2.5, 0.4938 },
                { 2.6, 0.4953 },
                { 2.7, 0.4965 },
                { 2.8, 0.4974 },
                { 2.9, 0.4981 },
                { 3.0, 0.4987 },
                { 3.1, 0.4990 },
                { 3.2, 0.4993 },
                { 3.3, 0.4995 },
                { 3.4, 0.4997 }
            };

        public static double GetFrequency(double lambda)
        {
            var roundedLambda = Math.Round(lambda * 10) / 10;
            return Frequencies[roundedLambda];
        }
    }
}