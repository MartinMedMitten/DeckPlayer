using System.Collections.Generic;
using System.Linq;

namespace NecroDeck
{
    class QueueWrapper<T> : StructureWrapper<T>
    {
        private Queue<T> b = new Queue<T>();

        public bool Any()
        {
            return b.Any();
        }

        public T Dequeue()
        {
            return b.Dequeue();
        }

        public void Enqueue(T item)
        {
            b.Enqueue(item);
        }
    }
}
