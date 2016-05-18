using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CyclicArrayQueue
{
    public class CustomQueue<T> : IEnumerable<T>
    {
        #region Fields
        private T[] queue;
        private int capacity, head, tail;
        private static readonly int defaultCapacity = 8;
        #endregion

        #region Constructors
        /// <summary>
        /// Special constructor
        /// </summary>
        public CustomQueue() : this(defaultCapacity) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">The length of queue</param>
        public CustomQueue(int n)
        {
            if (n > 0)
            {
                queue = new T[n];
                capacity = n;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            head = 0;
            tail = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arr">array of values</param>
        public CustomQueue(params T[] arr)
            : this((IEnumerable<T>)arr)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arr">Array of elements</param>
        public CustomQueue(IEnumerable<T> arr)
    : this(arr.ToArray().Length)
        {
            if (arr == null)
                return;
            queue = DeepClone(arr) as T[];
            tail = capacity - 1;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Add element to the tail of queue
        /// </summary>
        /// <param name="element">element</param>
        public void Enqueue(T element)
        {
            if ((tail + 1) % capacity == head % capacity)
            {
                Extend();
                queue[capacity] = element;
                tail = capacity;
                head = 0;
                capacity *= 2;
            }
            else
            {
                queue[++tail % capacity] = element;
                tail = tail % capacity;
            }

        }

        /// <summary>
        /// Return the element from the head of the queue and delete it 
        /// </summary>
        /// <returns>The first  element</returns>
        public T Dequeue()
        {
            if (IsEmpty())
                throw new ArgumentException();

            T returnValue = Peek();
            DeleteHead();
            return returnValue;
        }

        /// <summary>
        /// Return first element
        /// </summary>
        /// <returns>First element</returns>
        public T Peek()
        {
            if (IsEmpty())
                throw new ArgumentException();
            return queue[head];
        }

        #endregion

        #region Interface implementation

        public IEnumerator<T> GetEnumerator()
        {
            return new CyclicArrayQueueIterator(queue, tail, head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private Methods
        private void Extend()
        {
            T[] wideQueue = new T[capacity * 2];

            for (int i = head, j = 0; j < capacity; j++)
            {
                wideQueue[j] = queue[i];
                i = ++i % capacity;
            }
            queue = wideQueue;
        }

        private void DeleteHead()
        {
            //queue[head] = null;
            head = ++head % capacity;
        }

        private bool IsEmpty()
        {
            return tail % capacity == head % capacity;
        }

        private static IEnumerable<T> DeepClone(IEnumerable<T> source)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Position = 0;
            return (IEnumerable<T>)formatter.Deserialize(stream);
        }

        #endregion

        #region Internal Class Iterator
        private class CyclicArrayQueueIterator : IEnumerator<T>
        {
            #region Fields
            private T[] array;
            private int position;
            private readonly int tailPosition, headPosition;
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="list">Array of elements</param>
            /// <param name="tail">Index of last element</param>
            /// <param name="head">Index of first element</param>
            public CyclicArrayQueueIterator(T[] list, int tail, int head)
            {
                array = list;
                position = (head - 1) % list.Length;
                tailPosition = tail;
                headPosition = head;
            }
            #endregion

            #region Implementation of IEnumerator<T> methods
            /// <summary>
            /// Go to the next position
            /// </summary>
            /// <returns>False, if collection is finished</returns>
            public bool MoveNext()
            {
                if (position == tailPosition)
                {
                    Reset();
                    return false;
                }
                position = ++position % array.Length;
                return true;
            }

            /// <summary>
            /// Set index to the pre-begin
            /// </summary>
            public void Reset()
            {
                position = (headPosition - 1) % array.Length;
            }

            /// <summary>
            /// Return current element
            /// </summary>
            object IEnumerator.Current
            {
                get { return array[position]; }
            }

            public void Dispose()
            {
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Return current element
            /// </summary>
            public T Current
            {
                get { return array[position]; }
            }
            #endregion
        }
        #endregion
    }
}
