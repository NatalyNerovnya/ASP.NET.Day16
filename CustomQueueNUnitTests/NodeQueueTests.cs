using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NodeQueue;

namespace CustomQueueNUnitTests
{
    class NodeQueueTests
    {
        #region EnqueueTests
        #region Properties
        public IEnumerable<TestCaseData> ObjectEnqueuElemData
        {
            get
            {
                yield return new TestCaseData
                    (new object[] { new List<object>(), "Good day!" }, 13,
                        new CustomNodeQueue<object>(new List<object>(), "Good day!", 13))
                    .Returns(true);
            }
        }
        public IEnumerable<TestCaseData> StructEnqueuElemData
        {
            get
            {
                yield return new TestCaseData
                    (new int[] { 1, 2, 3 }, 13,
                        new CustomNodeQueue<int>(1, 2, 3, 13)).Returns(true);
            }
        }
        #endregion

        #region Methods
        [Test, TestCaseSource(nameof(ObjectEnqueuElemData))]
        public bool EnqueueElemTest(object[] arr, object elem, CustomNodeQueue<object> expectedQueue)
        {
            CustomNodeQueue<object> queue = new CustomNodeQueue<object>(arr);
            queue.Enqueue(elem);
            return Same(queue, expectedQueue);
        }

        [Test, TestCaseSource(nameof(StructEnqueuElemData))]
        public bool EnqueueElemTest(int[] arr, int elem, CustomNodeQueue<int> expectedQueue)
        {
            CustomNodeQueue<int> queue = new CustomNodeQueue<int>(arr);
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
                        new CustomNodeQueue<object>("Good day!", 13))
                    .Returns(true);
            }
        }

        public IEnumerable<TestCaseData> StructDequeuElemData
        {
            get
            {
                yield return new TestCaseData
                    (new int[] { 1, 2, 3, 13 },
                        new CustomNodeQueue<int>(2, 3, 13))
                    .Returns(true);
            }
        }
        #endregion

        #region Methods
        [Test, TestCaseSource(nameof(ObjectDequeuElemData))]
        public bool DequeueElemTest(object[] arr, CustomNodeQueue<object> expectedQueue)
        {
            CustomNodeQueue<object> queue = new CustomNodeQueue<object>(arr);
            queue.Dequeue();
            return Same(queue, expectedQueue);
        }


        [Test, TestCaseSource(nameof(StructDequeuElemData))]
        public bool DequeueElemTest(int[] arr, CustomNodeQueue<int> expectedQueue)
        {
            CustomNodeQueue<int> queue = new CustomNodeQueue<int>(arr);
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
                yield return new TestCaseData(new CustomNodeQueue<int>()).Throws(typeof(ArgumentException));
                yield return new TestCaseData(new CustomNodeQueue<int>(1, 15, 23, 543)).Returns(1);
            }
        }

        [Test, TestCaseSource(nameof(PeekElementData))]
        public int PeekElement(CustomNodeQueue<int> queue)
        {
            return queue.Peek();
        }
        #endregion

        #region Private methods
        private static bool Same<T>(CustomNodeQueue<T> queue1, CustomNodeQueue<T> queue2)
        {
            if (queue1.Count != queue2.Count)
                return false;
            for (int i = 0; i < queue1.Count; i++)
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
