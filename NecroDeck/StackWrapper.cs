using System.Collections.Generic;
using System.Linq;

namespace NecroDeck
{
    class StackWrapper<T> : StructureWrapper<T>
    {
        private Stack<T> b = new Stack<T>();

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
