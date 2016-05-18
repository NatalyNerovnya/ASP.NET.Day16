using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CyclicArrayQueue;

namespace CustomQueueNUnitTests
{
    public class CyclicArrayQueueTests
    {
        #region EnqueueTests
        #region Properties
        public IEnumerable<TestCaseData> ObjectEnqueuElemData
        {
            get
            {
                yield return new TestCaseData
                    (new object[] { new List<object>(), "Good day!" }, 13,
                        new CustomQueue<object>(new List<object>(), "Good day!", 13))
                    .Returns(true);
            }
        }
        public IEnumerable<TestCaseData> StructEnqueuElemData
        {
            get
            {
                yield return new TestCaseData
                    (new int[] { 1, 2, 3 }, 13,
                        new CustomQueue<int>(1, 2, 3, 13)).Returns(true);
            }
        }
        #endregion

        #region Methods
        [Test, TestCaseSource(nameof(ObjectEnqueuElemData))]
        public bool EnqueueElemTest(object[] arr, object elem, CustomQueue<object> expectedQueue)
        {
            CustomQueue<object> queue = new CustomQueue<object>(arr);
            queue.Enqueue(elem);
            return Same(queue, expectedQueue);
        }

        [Test, TestCaseSource(nameof(StructEnqueuElemData))]
        public bool EnqueueElemTest(int[] arr, int elem, CustomQueue<int> expectedQueue)
        {
            CustomQueue<int> queue = new CustomQueue<int>(arr);
            queue.Enqueue(elem);
            return Same(queue, expectedQueue);
        }
        #endregion
        #endregion

        #region Dequeue tests
        #region Properties
        public IEnumerable<TestCaseData> ObjectDequeuElemData
        {
            get
            {
                yield return new TestCaseData
                    (new object[] { new List<object>(), "Good day!", 13 },
                        new CustomQueue<object>("Good day!", 13))
                    .Returns(true);
            }
        }

        public IEnumerable<TestCaseData> StructDequeuElemData
        {
            get
            {
                yield return new TestCaseData
                    (new int[] { 1, 2, 3, 13 },
                        new CustomQueue<int>(2, 3, 13))
                    .Returns(true);
            }
        }
        #endregion

        #region Methods
        [Test, TestCaseSource(nameof(ObjectDequeuElemData))]
        public bool DequeueElemTest(object[] arr, CustomQueue<object> expectedQueue)
        {
            CustomQueue<object> queue = new CustomQueue<object>(arr);
            queue.Dequeue();
            return Same(queue, expectedQueue);
        }


        [Test, TestCaseSource(nameof(StructDequeuElemData))]
        public bool DequeueElemTest(int[] arr, CustomQueue<int> expectedQueue)
        {
            CustomQueue<int> queue = new CustomQueue<int>(arr);
            queue.Dequeue();
            return Same(queue, expectedQueue);
        }
        #endregion
        #endregion


        #region Peek tests
        public IEnumerable<TestCaseData> PeekElementData
        {
            get
            {
                yield return new TestCaseData(new CustomQueue<int>()).Throws(typeof(ArgumentException));
                yield return new TestCaseData(new CustomQueue<int>(1, 15, 23, 543)).Returns(1);
            }
        }

        [Test, TestCaseSource(nameof(PeekElementData))]
        public int PeekElement(CustomQueue<int> queue)
        {
            return queue.Peek();
        }
        #endregion

        #region Private methods
        private static bool Same<T>(CustomQueue<T> queue1, CustomQueue<T> queue2)
        {
            foreach (var variable in queue1)
            {
                if (queue1.Peek().GetType() != queue2.Peek().GetType() && !queue1.Dequeue().Equals(queue2.Dequeue()))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
