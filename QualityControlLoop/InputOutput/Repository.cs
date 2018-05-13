using System;
using System.Globalization;
using System.IO;

namespace QualityControlLoop
{
    internal class Repository
    {
        private readonly StreamReader _inputFile;
        private readonly string _outputFileName;
        public Repository(string inputFileName, string outputFileName)
        {
            _inputFile = new StreamReader(inputFileName);

            var outputFileNameWithoutExtension = outputFileName.Substring(0, outputFileName.Length - 4);
            var extension = outputFileName.Substring(outputFileName.Length - 3, 3);
            _outputFileName = $"{outputFileNameWithoutExtension}_{DateTime.Now.Ticks}.{extension}";
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

        public void Write(QualityControlLoopOutput output)
        {
            File.AppendAllText(_outputFileName, output.CalibrationAction.ToString(CultureInfo.InvariantCulture) + Environment.NewLine);
        }
    }
}