namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    
    public enum HeapSorting
    {
        Min,
        Max
    }

    public class Heap<T> where T : IComparable<T>
    {
        private List<T> elements;
        private int size = 0;
        private int cap = 0;

        // True if first T comes before second T.
        private Func<T, T, bool> firstBeforeSecond;

        public Heap(HeapSorting order, int capacity)
        {
            cap = capacity;
            elements = new List<T>(cap + 1);

            for (int i = 0; i < elements.Capacity; i++)
                elements.Add(default(T));

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

        public void Insert(T t)
        {
            if (IsFull)
                throw new InvalidOperationException();

            size++;
            elements[size] = t;
            percolateUp(size);
        }

        public T Remove()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            T top = elements[1];

            elements[1] = elements[size];
            size--;
            percolateDown(1);

            return top;
        }

        public T Top()
        {
            if (elements.Count < 2)
                throw new InvalidOperationException();

            return elements[1];
        }

        public void Clear()
        {
            size = 0;
            cap = 0;
        }

        private void percolateUp(int idx)
        {
            while (idx > 1 && firstBeforeSecond(elements[idx], elements[idx/2]))
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
                    && firstBeforeSecond(elements[child + 1], elements[child]);

                if (rightChildNext)
                    child++;

                if (firstBeforeSecond(elements[child], elements[idx]))
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
            T temp = elements[idx2];
            elements[idx2] = elements[idx1];
            elements[idx1] = temp;
        }
    }
}
