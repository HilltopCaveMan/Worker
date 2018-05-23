using System.Collections.Generic;

namespace Monopy.PreceRateWage.Common
{
    public delegate bool CompareDelegate<T>(T x, T y);

    public class CompareExtend<T> : IEqualityComparer<T>
    {
        private CompareDelegate<T> _compare;

        public CompareExtend(CompareDelegate<T> d)
        {
            _compare = d;
        }

        public bool Equals(T x, T y)
        {
            if (_compare != null)
            {
                return _compare(x, y);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}