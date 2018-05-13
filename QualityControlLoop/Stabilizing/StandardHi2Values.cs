using System;
using System.Collections.Generic;

namespace QualityControlLoop.Stabilizing
{
    internal class StandardHi2Values
    {
        private readonly StabilizerPrecision _precision;

        private static readonly Dictionary<int, double> Precision950 = new Dictionary<int, double>
        {
            {1, 3841},
            {2, 5.991},
            {3, 7.815},
            {4, 9.488},
            {5, 11.07},
            {6, 12.59},
            {7, 17.07},
            {8, 15.51},
            {9, 16.92},
            {10, 18.31},
            {11, 19.62},
            {12, 21.03},
            {13, 22.36},
            {14, 23.68},
            {15, 25.00},
            {16, 26.30},
            {17, 27.59},
            {18, 28.87},
            {19, 30.14},
            {20, 31.41},
        };

        private static readonly Dictionary<StabilizerPrecision, Dictionary<int, double>> Mappings =
            new Dictionary<StabilizerPrecision, Dictionary<int, double>>
            {
                {StabilizerPrecision._950, Precision950}
            };

        public Dictionary<int, double> Hi2Values => Mappings[_precision];

        public StandardHi2Values(StabilizerPrecision precision)
        {
            if (!Mappings.ContainsKey(precision))
            {
                throw new NotImplementedException();
            }

            _precision = precision;
        }

        public double GetValue(int v)
        {
            return Hi2Values[v];
        }
    }
}