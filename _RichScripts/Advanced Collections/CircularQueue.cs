using System;
using System.Collections.Generic;

namespace RichPackage.Collections
{
    public class CircularQueue<T>
    {
        private readonly T[] buffer;
        private readonly int capacity;
        private int head;
        private int tail;
        private int count;

        public CircularQueue(int size)
        {
            capacity = size;
            buffer = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }

        public bool IsEmpty => count == 0;
        public bool IsFull => count == capacity;
        public int Count => count;

        public void Enqueue(T item)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Queue is full.");
            }

            buffer[tail] = item;
            tail = (tail + 1) % capacity;
            count++;
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            T item = buffer[head];
            head = (head + 1) % capacity;
            count--;
            return item;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            return buffer[head];
        }

        public void Clear()
        {
            head = 0;
            tail = 0;
            count = 0;
        }

        public override string ToString()
        {
            var items = new List<T>();
            int index = head;
            for (int i = 0; i < count; i++)
            {
                items.Add(buffer[index]);
                index = (index + 1) % capacity;
            }

            return $"[{string.Join(", ", items)}]";
        }
    }
}
