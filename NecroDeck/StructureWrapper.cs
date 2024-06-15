namespace NecroDeck
{
    interface StructureWrapper<T>
    {
        void Enqueue(T item);
        T Dequeue();
        bool Any();
    }
}
