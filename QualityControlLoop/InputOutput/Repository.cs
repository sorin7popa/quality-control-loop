using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using QualityControlLoop.ParameterCalculation;

namespace QualityControlLoop.InputOutput
{
    internal class Repository
    {
        private static int _writingIterationCount;
        private readonly StreamReader _inputFile;
        private readonly string _outputFileNameWithoutExtension;
        private readonly string _outputFileNameExtension;
        private readonly string _outputFileName;

        public Repository(string inputFileName, string outputFileName)
        {
            _inputFile = new StreamReader(inputFileName);

            _outputFileNameWithoutExtension = $"{outputFileName.Substring(0, outputFileName.Length - 4)}_{DateTime.Now.Ticks}";
            _outputFileNameExtension = outputFileName.Substring(outputFileName.Length - 3, 3);
            _outputFileName = $"{_outputFileNameWithoutExtension}.{_outputFileNameExtension}";
            PrintFileHeader();
        }

        public bool DataAvailable()
        {
            return _inputFile.Peek() > 0;
        }

        public double ReadNext()
        {
            string line;
            double measuredInput = 0;

            if ((line = _inputFile.ReadLine()) != null)
            {
                measuredInput = double.Parse(line);
            }

            return measuredInput;
        }

        private void PrintFileHeader()
        {
            var headerLine = string.Join(",", new List<string>
            {
                nameof(QualityControlLoopOutput.CurrentMeasureadInput),
                nameof(QualityControlLoopOutput.MinInput),
                nameof(QualityControlLoopOutput.MaxInput),
                nameof(QualityControlLoopOutput.MeanValue),
                nameof(QualityControlLoopOutput.Dispersion),
                nameof(QualityControlLoopOutput.IntervalCount),
                nameof(QualityControlLoopOutput.NonCompliancePercentage),
                nameof(QualityControlLoopOutput.CalibrationAction),
                nameof(QualityControlLoopOutput.SystemErrorsExist),
            });

            File.AppendAllLines(_outputFileName, new List<string> {headerLine});
        }

        public void Write(QualityControlLoopOutput output)
        {
            _writingIterationCount++;
            Console.WriteLine($"Writing iteration {_writingIterationCount}");
            WriteDataWindow(output.DataWindow);
            WriteIntervals(output.Intervals);

            var dataLine = string.Join(",", new List<string>
            {
                output.CurrentMeasureadInput.ToString(CultureInfo.InvariantCulture),
                output.MinInput.ToString(CultureInfo.InvariantCulture),
                output.MaxInput.ToString(CultureInfo.InvariantCulture),
                output.MeanValue.ToString(CultureInfo.InvariantCulture),
                output.Dispersion.ToString(CultureInfo.InvariantCulture),
                output.IntervalCount.ToString(CultureInfo.InvariantCulture),
                output.NonCompliancePercentage.ToString(CultureInfo.InvariantCulture),
                output.CalibrationAction.ToString(CultureInfo.InvariantCulture),
                output.SystemErrorsExist.ToString(CultureInfo.InvariantCulture),
            });

            File.AppendAllLines(_outputFileName, new List<string> { dataLine });
        }

        private void WriteDataWindow(IReadOnlyCollection<double> outputDataWindow)
        {
            var dataOutputFileName = $"{_outputFileNameWithoutExtension}_{_writingIterationCount}_data.{_outputFileNameExtension}";
            File.WriteAllLines(dataOutputFileName, outputDataWindow.Select(d => d.ToString(CultureInfo.InvariantCulture)));
        }
        
        private void WriteIntervals(IEnumerable<SubInterval> outputIntervals)
        {
            var dataOutputFileName = $"{_outputFileNameWithoutExtension}_{_writingIterationCount}_intervals.{_outputFileNameExtension}";

            var headerLine = string.Join(",", new List<string>
            {
                nameof(SubInterval.Index),
                nameof(SubInterval.LeftBound),
                nameof(SubInterval.RightBound),
                nameof(SubInterval.InputsCount),
                nameof(SubInterval.InputsFrequency),
                nameof(SubInterval.GaussExpectedCount),
                nameof(SubInterval.GaussExpectedFrequency),
            });

            var dataLines = outputIntervals.Select(interval => string.Join(",", new List<string>
            {
                interval.Index.ToString(CultureInfo.InvariantCulture),
                interval.LeftBound.ToString(CultureInfo.InvariantCulture),
                interval.RightBound.ToString(CultureInfo.InvariantCulture),
                interval.InputsCount.ToString(CultureInfo.InvariantCulture),
                interval.InputsFrequency.ToString(CultureInfo.InvariantCulture),
                interval.GaussExpectedCount.ToString(CultureInfo.InvariantCulture),
                interval.GaussExpectedFrequency.ToString(CultureInfo.InvariantCulture),
            })).ToList();

            dataLines.Insert(0, headerLine);

            File.WriteAllLines(dataOutputFileName, dataLines);
        }
    }
}