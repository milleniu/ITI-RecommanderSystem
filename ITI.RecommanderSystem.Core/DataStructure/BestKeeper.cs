using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ITI.RecommanderSystem.Core.DataStructure
{
    public class BestKeeper<T> : IReadOnlyCollection<T>
    {
        private readonly T[] _items;

        private class ComparerAdapter : IComparer<T>
        {
            private readonly Func<T, T, int> _comparator;

            public ComparerAdapter(Func<T, T, int> comparator) => _comparator = comparator;

            public int Compare(T x, T y) => _comparator(x, y);
        }

        public BestKeeper(int capacity, Func<T, T, int> comparator)
            : this(capacity, new ComparerAdapter(comparator))
        {
        }

        public BestKeeper(int capacity, IComparer<T> comparer = null)
        {
            if (capacity <= 0) throw new ArgumentException("The max count must be greater than 0.", nameof(capacity));
            Comparer = comparer ?? Comparer<T>.Default;
            _items = new T[capacity];
        }

        public bool Add(T candidate, Action<T> collector = null)
        {
            if (IsFull)
            {
                if (Comparer.Compare(candidate, _items[0]) < 0) return false;
                AddFromTop(candidate, collector);
                return true;
            }

            AddFromBottom(candidate);
            return true;
        }

        public IComparer<T> Comparer { get; }

        public int Capacity => _items.Length;

        public int Count { get; private set; }

        public IEnumerator<T> GetEnumerator() => _items.Take(Count).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private bool IsFull => Count == _items.Length;

        private void AddFromBottom(T item)
        {
            _items[Count] = item;
            var idx = Count;

            while (idx > 0)
            {
                var fatherIdx = (idx - 1) / 2;
                if (Comparer.Compare(item, _items[fatherIdx]) > 0) break;
                Swap(idx, fatherIdx);
                idx = fatherIdx;
            }
            Count++;
        }

        private void AddFromTop(T candidate, Action<T> collector)
        {
            var idx = 0;
            var removedItem = _items[0];
            _items[0] = candidate;

            while (true)
            {
                var leftIdx = idx * 2 + 1;
                var rightIdx = idx * 2 + 2;

                int smallestIdx;
                if (leftIdx < Count && Comparer.Compare(_items[leftIdx], candidate) < 0) smallestIdx = leftIdx;
                else smallestIdx = idx;
                if (rightIdx < Count && Comparer.Compare(_items[rightIdx], _items[smallestIdx]) < 0) smallestIdx = rightIdx;

                if (smallestIdx == idx)
                {
                    collector?.Invoke(removedItem);
                    return;
                }

                Swap(smallestIdx, idx);
                idx = smallestIdx;
            }
        }

        private void Swap(int idx1, int idx2)
        {
            var item = _items[idx1];
            _items[idx1] = _items[idx2];
            _items[idx2] = item;
        }
    }
}
