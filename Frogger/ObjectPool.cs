using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frogger
{
    class ObjectPool<T> where T : new()
    {
        private readonly ConcurrentBag<T> items = new ConcurrentBag<T>();
        private int counter = 0;
        private int max = 5;

        public void Release(T item)
        {
            if(counter < max)
            {
                items.Add(item);
                counter++;
            }
        }

        public T Get()
        {
            T item;
            if(items.TryTake(out item))
            {
                counter--;
                return item;
            }

            else
            {
                T obj = new T();
                items.Add(obj);
                counter++;
                return obj;
            }
        }
    }
}
