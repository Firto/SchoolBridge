using System;
using System.Collections.Generic;

namespace SchoolBridge.Helpers.AddtionalClases
{
    public class PriorityQueue<T> where T: class
    {
        int total_size;
        SortedDictionary<int, Queue<T>> storage;
        object _lock = new object();
        public PriorityQueue()
        {
            this.storage = new SortedDictionary<int, Queue<T>>();
            this.total_size = 0;
        }

        public bool IsEmpty()
        {
            lock (_lock)
            {
                return (total_size == 0);
            }
        }

        public T Dequeue()
        {
            lock (_lock)
            {
                if (IsEmpty())
                {
                    throw new Exception("Please check that priorityQueue is not empty before dequeing");
                }
                else
                    foreach (var q in storage.Values)
                    {
                        // we use a sorted dictionary
                        if (q.Count > 0)
                        {
                            total_size--;
                            return q.Dequeue();
                        }
                    }
                return null; // not supposed to reach here.
            }
        }

        // same as above, except for peek.

        public T Peek()
        {
            lock (_lock)
            {
                if (IsEmpty())
                throw new Exception("Please check that priorityQueue is not empty before peeking");
                else
                    foreach (var q in storage.Values)
                    {
                        if (q.Count > 0)
                            return q.Peek();
                    }
                return null; // not supposed to reach here.
            }
        }

        public object Dequeue(int prio)
        {
            lock (_lock)
            {
                total_size--;
                return storage[prio].Dequeue();
            }
        }

        public void Enqueue(T item, int prio)
        {
            lock (_lock)
            {
                if (!storage.ContainsKey(prio))
                {
                    storage.Add(prio, new Queue<T>());
                }
                storage[prio].Enqueue(item);
                total_size++;
            }
        }
    }
}
