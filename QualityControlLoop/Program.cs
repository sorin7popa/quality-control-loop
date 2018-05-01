using System.Collections.Generic;

namespace QualityControlLoop
{
    internal class Program
    {
        private const string InputFileName = "../../io/MeasuredInput.csv";
        private const string OutputFileName = "../../io/Output.csv";
        private const int DataWindowSize = 50;
        private const double Xi = 8.91;
        private const double Xs = 8.94;

        private static readonly Repository Repository;
        private static readonly DataWindow DataWindow;
        private static readonly ParametersCalculator ParametersCalculator;
        private static readonly Optimizer Optimizer;
        private static readonly Stabilizer Stabilizer;
        private static readonly Calibrator Calibrator;

        static Program()
        {
            Repository = new Repository(InputFileName, OutputFileName);
            DataWindow = new DataWindow(DataWindowSize, Xi, Xs);
            ParametersCalculator = new ParametersCalculator();
            Optimizer = new Optimizer(Xi, Xs);
            Stabilizer = new Stabilizer();
            Calibrator = new Calibrator(Xi, Xs);
        }

        private static void Main(string[] args)
        {
            while (Repository.DataAvailable())
            {
                var measuredInput = Repository.ReadNext();
                DataWindow.Enqueue(measuredInput);

                var parameters = ParametersCalculator.GetParameters(DataWindow.Data);
                var nonCompliancePercentage = Optimizer.NonCompliancePercentage(DataWindow.Data);

                var systemErrors = Stabilizer.SystemErrors(parameters.Hi2);
                var calibration = systemErrors ? default(double) : Calibrator.GetCalibration(parameters.Mean);
                
                var output = new QualityControlLoopOutput
                {
                    CurrentMeasureadInput = measuredInput,
                    DataWindow = string.Join(",", DataWindow.Data),
                    MinInput = parameters.MinInput,
                    MaxInput = parameters.MaxInput,
                    MeanValue = parameters.Mean,
                    Dispersion = parameters.Dispersion,
                    IntervalCount = parameters.IntervalCount,
                    Intervals = parameters.Intervals,
                    Hi2 = parameters.Hi2,
                    NonCompliancePercentage = nonCompliancePercentage,
                    CalibrationAction = calibration,
                    SystemErrorsExist = systemErrors
                };

                Repository.Write(output);
            }
            

            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }

    internal class QualityControlLoopOutput
    {
        public double CurrentMeasureadInput { get; set; }
        public string DataWindow { get; set; }
        public double MinInput { get; set; }
        public double MaxInput { get; set; }
        public double MeanValue { get; set; }
        public double Dispersion { get; set; }
        public int IntervalCount { get; set; }
        public IEnumerable<SubInterval> Intervals { get; set; }
        public double Hi2 { get; set; }
        public object NonCompliancePercentage { get; set; }
        public double CalibrationAction { get; set; }
        public bool SystemErrorsExist { get; set; }
    }
}
