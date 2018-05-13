namespace QualityControlLoop.ParameterCalculation
{
    internal class SubInterval
    {
        public int Index { get; set; }
        public double LeftBound { get; set; }
        public double RightBound { get; set; }
        public int InputsCount { get; set; }
        public double InputsFrequency { get; set; }
        public double GaussExpectedCount { get; set; }
        public double GaussExpectedFrequency { get; set; }
    }
}