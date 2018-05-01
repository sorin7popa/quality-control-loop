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
            _outputFileName = outputFileName;
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
            if (!File.Exists(_outputFileName))
            {
                File.Create(_outputFileName);
            }

            File.AppendAllText(_outputFileName, output.CalibrationAction.ToString(CultureInfo.InvariantCulture));
        }
    }
}