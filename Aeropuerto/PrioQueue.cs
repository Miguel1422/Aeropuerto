using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aeropuerto
{
    class PrioQueue<T> : IEnumerable<T> where T : IComparable<T>
    {
        private T[] data;
        private int tam = 0;
        public PrioQueue(int maxTam)
        {
            data = new T[maxTam];
        }

        public void Add(T item)
        {
            data[tam] = item;
            tam++;
            Sort();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < tam; i++)
            {
                yield return data[i];
            }
        }

        public int Count
        {
            get { return tam; }
        }

        public void Sort()
        {
            Array.Sort(data, 0, tam);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
