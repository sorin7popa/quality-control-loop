using QualityControlLoop.InputOutput;
using QualityControlLoop.ParameterCalculation;
using QualityControlLoop.Stabilizing;

namespace QualityControlLoop
{
    internal class Program
    {
        private const string InputFileName = "MeasuredInput.csv";
        private const string OutputFolderPrefix = "Output";
        private const string ParametersOutputFileName = "Parameters";
        private const string DataWindowFileSuffix = "Data";
        private const string DataIntervalsFileSuffix = "Intervals";
        private const string OutputFileExtension = "csv";
        private const int DataWindowSize = 50;
        private const double Xi = 8.91;
        private const double Xs = 8.94;

        private const StabilizerPrecision Precision = StabilizerPrecision._950;
        private const int NumberOfParameters = 2; //mean & dispersion

        private static readonly DataReader DataReader;
        private static readonly DataWriter DataWriter;
        private static readonly DataWindow DataWindow;
        private static readonly ParametersCalculator ParametersCalculator;
        private static readonly Optimizer Optimizer;
        private static readonly Stabilizer Stabilizer;
        private static readonly Calibrator Calibrator;

        static Program()
        {
            DataReader = new DataReader(InputFileName);
            DataWriter = new DataWriter(OutputFolderPrefix, ParametersOutputFileName, DataWindowFileSuffix,
                DataIntervalsFileSuffix, OutputFileExtension);
            DataWindow = new DataWindow(DataWindowSize);
            ParametersCalculator = new ParametersCalculator();
            Optimizer = new Optimizer(Xi, Xs);
            var standardHi2Values = new StandardHi2Values(Precision);
            Stabilizer = new Stabilizer(standardHi2Values, NumberOfParameters);
            Calibrator = new Calibrator(Xi, Xs);
        }

        private static void Main(string[] args)
        {
            while (DataReader.DataAvailable())
            {
                var measuredInput = DataReader.ReadNext();
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
                DataWriter.Write(output);
            }
        }
    }
}
