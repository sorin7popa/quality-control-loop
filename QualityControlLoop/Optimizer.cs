using System.Collections.Generic;
using System.Linq;

namespace QualityControlLoop
{
    internal class Optimizer
    {
        private readonly double _xi;
        private readonly double _xs;

        public Optimizer(double xi, double xs)
        {
            _xi = xi;
            _xs = xs;
        }

        public double NonCompliancePercentage(IReadOnlyCollection<double> measuredData)
        {
            var count = measuredData.Count(d => d < _xi || d > _xs);
            var percentage = (double)count / measuredData.Count;

            return percentage;
        }
    }
}