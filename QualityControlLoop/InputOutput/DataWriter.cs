using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using QualityControlLoop.ParameterCalculation;

namespace QualityControlLoop.InputOutput
{
    internal class DataWriter
    {
        private readonly string _parametersFileName;
        private readonly string _dataWindowFileSuffix;
        private readonly string _dataIntervalsFileSuffix;
        private readonly string _fileExtension;

        private static int _writingIterationCount;

        public DataWriter(string folderPrefix, string parametersFileName, string dataWindowFileSuffix,
            string dataIntervalsFileSuffix, string fileExtension)
        {
            var folderName = $"{folderPrefix}_{DateTime.Now.Ticks}";
            _parametersFileName = Path.Combine(folderName, parametersFileName);
            _dataWindowFileSuffix = dataWindowFileSuffix;
            _dataIntervalsFileSuffix = dataIntervalsFileSuffix;
            _fileExtension = fileExtension;

            Directory.CreateDirectory(folderName);
            WriteOutputFileHeader();
        }

        private void WriteOutputFileHeader()
        {
            var headerLine = string.Join(",", new List<string>
            {
                "Index",
                nameof(QualityControlLoopOutput.CurrentMeasureadInput),
                nameof(QualityControlLoopOutput.MinInput),
                nameof(QualityControlLoopOutput.MaxInput),
                nameof(QualityControlLoopOutput.MeanValue),
                nameof(QualityControlLoopOutput.Dispersion),
                nameof(QualityControlLoopOutput.IntervalCount),
                nameof(QualityControlLoopOutput.Hi2),
                nameof(QualityControlLoopOutput.ReferenceHi2),
                nameof(QualityControlLoopOutput.NonCompliancePercentage),
                nameof(QualityControlLoopOutput.CalibrationAction),
                nameof(QualityControlLoopOutput.SystemErrorsExist),
            });

            File.AppendAllLines($"{_parametersFileName}.{_fileExtension}", new List<string> {headerLine});
        }

        public void Write(QualityControlLoopOutput output)
        {
            _writingIterationCount++;
            Console.WriteLine($"Writing iteration {_writingIterationCount}");
            WriteDataWindow(output.DataWindow);
            WriteIntervals(output.Intervals);

            var dataLine = string.Join(",", new List<string>
            {
                _writingIterationCount.ToString(CultureInfo.InvariantCulture),
                output.CurrentMeasureadInput.ToString(CultureInfo.InvariantCulture),
                output.MinInput.ToString(CultureInfo.InvariantCulture),
                output.MaxInput.ToString(CultureInfo.InvariantCulture),
                output.MeanValue.ToString(CultureInfo.InvariantCulture),
                output.Dispersion.ToString(CultureInfo.InvariantCulture),
                output.IntervalCount.ToString(CultureInfo.InvariantCulture),
                output.Hi2.ToString(CultureInfo.InvariantCulture),
                output.ReferenceHi2.ToString(CultureInfo.InvariantCulture),
                output.NonCompliancePercentage.ToString(CultureInfo.InvariantCulture),
                output.CalibrationAction.ToString(CultureInfo.InvariantCulture),
                output.SystemErrorsExist.ToString(CultureInfo.InvariantCulture),
            });

            File.AppendAllLines($"{_parametersFileName}.{_fileExtension}", new List<string> { dataLine });
        }

        private void WriteDataWindow(IEnumerable<double> outputDataWindow)
        {
            var dataOutputFileName = $"{_parametersFileName}_{_writingIterationCount}_{_dataWindowFileSuffix}.{_fileExtension}";
            File.WriteAllLines(dataOutputFileName, outputDataWindow.Select(d => d.ToString(CultureInfo.InvariantCulture)));
        }

        private void WriteIntervals(IEnumerable<SubInterval> outputIntervals)
        {
            var dataOutputFileName = $"{_parametersFileName}_{_writingIterationCount}_{_dataIntervalsFileSuffix}.{_fileExtension}";

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