namespace QualityControlLoop.Stabilizing
{
    internal class Stabilizer
    {
        private readonly StandardHi2Values _standardHi2Values;
        private readonly int _numberOfParameters;

        public Stabilizer(StandardHi2Values standardHi2Values, int numberOfParameters)
        {
            _standardHi2Values = standardHi2Values;
            _numberOfParameters = numberOfParameters;
        }

        public bool SystemErrors(double hi2, int intervalCount, out double referenceHi2)
        {
            var v = intervalCount - _numberOfParameters + 1;
            referenceHi2 = _standardHi2Values.GetValue(v);
            return hi2 > referenceHi2;
        }
    }
}