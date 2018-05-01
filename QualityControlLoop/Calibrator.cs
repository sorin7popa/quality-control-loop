namespace QualityControlLoop
{
    internal class Calibrator
    {
        private readonly double _xi;
        private readonly double _xs;

        public Calibrator(double xi, double xs)
        {
            _xi = xi;
            _xs = xs;
        }

        public double GetCalibration(double mean)
        {
            var calibration = mean - (_xi + _xs) / 2;
            return calibration;
        }
    }
}