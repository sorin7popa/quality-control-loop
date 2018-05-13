using System.Collections.Generic;

namespace QualityControlLoop.ParameterCalculation
{
    internal class Parameters
    {
        public double MinInput { get; set; }
        public double MaxInput { get; set; }
        public double Mean { get; set; }
        public double Dispersion { get; set; }
        public int IntervalCount { get; set; }
        public IEnumerable<SubInterval> Intervals { get; set; }
        public double Hi2 { get; set; }
    }
}