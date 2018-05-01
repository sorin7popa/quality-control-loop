using System.Collections.Generic;
using System.Linq;

namespace QualityControlLoop
{
    internal class DataWindow
    {
        private readonly List<double> _data;

        public IReadOnlyCollection<double> Data => _data.ToList();

        public DataWindow(int dataWindowSize, double xi, double xs)
        {
            _data = Enumerable.Repeat((xi + xs) / 2, dataWindowSize).ToList();
        }

        public void Enqueue(double value)
        {
            _data.RemoveAt(0);
            _data.Add(value);
        }
    }
}