namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;

    public class KeyHeap<T> where T : IComparable<T>
    {
        private List<T> elements;
        private int size = 0;
        private int cap = 0;

        private List<int> idxHeap;
        private List<int> invIdxHeap;

        // True if first T comes before second T.
        private Func<T, T, bool> firstBeforeSecond;

        public KeyHeap(HeapSorting order, List<T> keys, int capacity)
        {
            cap = capacity;
            elements = keys;

            idxHeap = new List<int>(cap + 1);
            invIdxHeap = new List<int>(cap + 1);

            for (int i = 0; i < cap + 1; i++)
            {
                idxHeap.Add(0);
                invIdxHeap.Add(0);
            }

            switch (order)
            {
                case HeapSorting.Min:
                    firstBeforeSecond = (ele1, ele2) => ele1.CompareTo(ele2) < 0;
                    break;
                case HeapSorting.Max:
                    firstBeforeSecond = (ele1, ele2) => ele1.CompareTo(ele2) > 0;
                    break;
            }
        }

        public int Size
        {
            get { return size; }
        }

        public int Capacity
        {
            get { return cap; }
        }

        public bool IsFull
        {
            get { return size == cap; }
        }

        public bool IsEmpty
        {
            get { return size == 0; }
        }

        public void Insert(int index)
        {
            if (IsFull)
                throw new InvalidOperationException();

            size++;
            idxHeap[size] = index;
            invIdxHeap[index] = size;
            percolateUp(size);
        }

        public int Remove()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            swap(1, size);
            size--;
            percolateDown(1);

            return idxHeap[size+1];
        }

        public int Top()
        {
            if (elements.Count < 2)
                throw new InvalidOperationException();

            return idxHeap[1];
        }

        public void Reorder(int index)
        {
            percolateUp(invIdxHeap[index]);
        }

        public void Clear()
        {
            size = 0;
            cap = 0;
        }

        private void percolateUp(int idx)
        {
            while (idx > 1 && firstBeforeSecond(elements[idxHeap[idx]], elements[idxHeap[idx/2]]))
            {
                swap(idx, idx / 2);
                idx /= 2;
            }
        }

        private void percolateDown(int idx)
        {
            while (2 * idx <= size)
            {
                int child = 2 * idx;

                bool parentOf2 = child < size;
                bool rightChildNext = parentOf2
                    && firstBeforeSecond(elements[idxHeap[child+1]], elements[idxHeap[child]]);

                if (rightChildNext)
                    child++;

                if (firstBeforeSecond(elements[idxHeap[child]], elements[idxHeap[idx]]))
                {
                    swap(child, idx);
                    idx = child;
                }
                else
                    break;
            }
        }

        private void swap(int idx1, int idx2)
        {
            int temp = idxHeap[idx2];
            idxHeap[idx2] = idxHeap[idx1];
            idxHeap[idx1] = temp;

            invIdxHeap[idxHeap[idx1]] = idx1;
            invIdxHeap[idxHeap[idx2]] = idx2;
        }
    }
}
