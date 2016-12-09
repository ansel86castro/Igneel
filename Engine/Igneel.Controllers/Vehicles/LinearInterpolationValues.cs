using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NxF32 = System.Single;

namespace Igneel.Controllers
{
    [Serializable]
    public class LinearInterpolationValues:IEnumerable<KeyValuePair<float,float>>, IEnumerable<float>
    {
        SortedList<NxF32, NxF32> _map = new SortedList<NxF32, NxF32>();
        float _min, _max;

        public void Clear()
        {
            _map.Clear();
        }

        public int Count { get { return _map.Count; } }     

        private int GetLowerBoundIndex(float number)
        {
            for (int i = 0; i < _map.Count; i++)
            {
                if (_map.Keys[i] >= number)
                    return i;
            }
            return -1;
        }

        public void Insert(float number, float value)
        {
            if (_map.Count == 0)
                _min = _max = number;
            else
            {
                _min = Math.Min(_min, number);
                _max = Math.Max(_max, number);
            }
            _map[number] = value;
        }
        
        public bool IsValid(float number) 
        {
            return number >= _min && number <= _max;
        }

        public float GetValueAt(int index)
        {
            return _map.Values[index];
        }

        public float GetValue(float number)
        {
            if (number < _min) return _map[_min];
            if (number >= _max) return _map[_max];

            int lowerBound = GetLowerBoundIndex(number);

            float x0 = _map.Keys[lowerBound];
            float x1 = _map.Keys[lowerBound++];
            float y0 = _map[x0];
            float y1 = _map[x1];

            float lerp = (number - x0) / (x1 - x0);
            return y0 * (1 - lerp) + y1 * lerp; 
        }

        public IEnumerator<NxF32> GetEnumerator()
        {
            return _map.Values.GetEnumerator();
        }

        IEnumerator<KeyValuePair<NxF32, NxF32>> IEnumerable<KeyValuePair<NxF32, NxF32>>.GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _map.Values.GetEnumerator();
        }       
    }
}
