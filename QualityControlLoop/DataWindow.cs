using System.Collections.Generic;
using System.Linq;

namespace QualityControlLoop
{
    internal class DataWindow
    {
        private readonly int _dataWindowSize;
        private readonly List<double> _data;

        public IReadOnlyCollection<double> Data => _data.ToList();

        public DataWindow(int dataWindowSize)
        {
            _dataWindowSize = dataWindowSize;
            _data = new List<double>(dataWindowSize);
        }

        public void Enqueue(double value)
        {
            if (Full())
            {
                _data.RemoveAt(0);
            }
            _data.Add(value);
        }

        public bool Full()
        {
            return _data.Count == _dataWindowSize;
        }
    }
}