﻿using System.Collections.Generic;

namespace QualityControlLoop
{
    internal class QualityControlLoopOutput
    {
        public QualityControlLoopOutput(double currentMeasureadInput, IReadOnlyCollection<double> dataWindow, Parameters parameters, double nonCompliancePercentage, double calibrationAction, bool systemErrorsExist)
        {
            CurrentMeasureadInput = currentMeasureadInput;
            DataWindow = string.Join(",", dataWindow);
            MinInput = parameters.MinInput;
            MaxInput = parameters.MaxInput;
            MeanValue = parameters.Mean;
            Dispersion = parameters.Dispersion;
            IntervalCount = parameters.IntervalCount;
            Intervals = parameters.Intervals;
            Hi2 = parameters.Hi2;
            NonCompliancePercentage = nonCompliancePercentage;
            CalibrationAction = calibrationAction;
            SystemErrorsExist = systemErrorsExist;
        }

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