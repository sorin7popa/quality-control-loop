using QualityControlLoop.Stabilizing;

namespace QualityControlLoop
{
    internal class Program
    {
        private const string InputFileName = "MeasuredInput.csv";
        private const string OutputFileName = "Output.csv";
        private const int DataWindowSize = 50;
        private const double Xi = 8.91;
        private const double Xs = 8.94;

        private const StabilizerPrecision Precision = StabilizerPrecision._950;
        private const int NumberOfParameters = 2; //mean & dispersion

        private static readonly Repository Repository;
        private static readonly DataWindow DataWindow;
        private static readonly ParametersCalculator ParametersCalculator;
        private static readonly Optimizer Optimizer;
        private static readonly Stabilizer Stabilizer;
        private static readonly Calibrator Calibrator;

        static Program()
        {
            Repository = new Repository(InputFileName, OutputFileName);
            DataWindow = new DataWindow(DataWindowSize);
            ParametersCalculator = new ParametersCalculator();
            Optimizer = new Optimizer(Xi, Xs);
            var standardHi2Values = new StandardHi2Values(Precision);
            Stabilizer = new Stabilizer(standardHi2Values, NumberOfParameters);
            Calibrator = new Calibrator(Xi, Xs);
        }

        private static void Main(string[] args)
        {
            while (Repository.DataAvailable())
            {
                var measuredInput = Repository.ReadNext();
                DataWindow.Enqueue(measuredInput);

                if (!DataWindow.Full())
                {
                    continue;
                }

                var parameters = ParametersCalculator.GetParameters(DataWindow.Data);
                var nonCompliancePercentage = Optimizer.NonCompliancePercentage(DataWindow.Data);

                var systemErrors = Stabilizer.SystemErrors(parameters.Hi2, parameters.IntervalCount);
                var calibration = systemErrors ? default(double) : Calibrator.GetCalibration(parameters.Mean);

                var output = new QualityControlLoopOutput(measuredInput, DataWindow.Data, parameters,
                    nonCompliancePercentage, calibration, systemErrors);
                Repository.Write(output);
            }
        }
    }
}
