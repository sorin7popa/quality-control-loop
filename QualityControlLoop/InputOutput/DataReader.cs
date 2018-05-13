using System.IO;

namespace QualityControlLoop.InputOutput
{
    internal class DataReader
    {
        private readonly StreamReader _inputFile;

        public DataReader(string inputFileName)
        {
            _inputFile = new StreamReader(inputFileName);
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
    }
}