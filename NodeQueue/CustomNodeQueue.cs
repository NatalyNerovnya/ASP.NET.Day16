using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NodeQueue
{
    public class CustomNodeQueue<T> : IEnumerable<T>
    {
        #region  Fields
        private LinkedList<T> queue;
        #endregion

        #region Properties
        public int Count { get { return queue.Count; } }
        #endregion
        #region Constructors
        public CustomNodeQueue()
        {
            queue = new LinkedList<T>();

        }

        public CustomNodeQueue(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();
            queue = new LinkedList<T>(collection);
        }
        public CustomNodeQueue(params T[] arr)
            : this((IEnumerable<T>)arr)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add element to the queue
        /// </summary>
        /// <param name="value">Element</param>
        public void Enqueue(T value)
        {
            if (value == null)
                throw new ArgumentNullException();
            queue.AddLast(new LinkedListNode<T>(value));
        }
        /// <summary>
        /// Remove element from the queue
        /// </summary>
        /// <returns>First element</returns>
        public T Dequeue()
        {
            if (ReferenceEquals(null, queue.First))
                throw new ArgumentNullException();
            T result = queue.First.Value;
            queue.RemoveFirst();
            return result;
        }


        /// <summary>
        /// Show the first element
        /// </summary>
        /// <returns>First element</returns>
        public T Peek()
        {
            if (queue.First == null)
                throw new ArgumentException();
            return queue.First.Value;
        }
        #endregion

        #region Interface implementation
        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListQueueIterator(queue);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Internal Class Iterator
        private class LinkedListQueueIterator : IEnumerator<T>
        {
            #region Fields
            private LinkedList<T> queue;
            private int position = -1;
            #endregion

            #region Constructor
            public LinkedListQueueIterator(LinkedList<T> source)
            {
                queue = new LinkedList<T>(source);
            }
            #endregion

            #region Implementation of IEnumerator<T> methods
            /// <summary>
            /// Go to the next position
            /// </summary>
            /// <returns>False, if collection is finished</returns>
            public bool MoveNext()
            {
                if (position == queue.Count + 1)
                {
                    Reset();
                    return false;
                }
                position++;
                return true;
            }

            /// <summary>
            /// Set index to the pre-begin
            /// </summary>
            public void Reset()
            {
                position = -1;
            }

            /// <summary>
            /// Return current element
            /// </summary>
            object IEnumerator.Current
            {
                get { return queue.ToArray()[position]; }
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
                get { return queue.ToArray()[position]; }
            }
            #endregion
        }
        #endregion

        #region Private Methods
        private static IEnumerable<T> DeepClone(IEnumerable<T> source)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Position = 0;
            return (IEnumerable<T>)formatter.Deserialize(stream);
        }
        #endregion
    }
}
