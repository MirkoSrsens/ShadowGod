using System;

namespace Razorhead.Core
{
    public sealed class DoubleBuffer<T> where T : class, new()
    {
        private T frontBuffer;
        private T backBuffer;
        private readonly object lockObj = new();

        public DoubleBuffer()
        {
            frontBuffer = new T();
            backBuffer = new T();
        }

        public DoubleBuffer(Func<T> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            frontBuffer = factory();
            backBuffer = factory();
        }

        /// <summary>
        /// Gets the current front buffer (read-only).
        /// </summary>
        public T Front
        {
            get
            {
                lock (lockObj)
                {
                    return frontBuffer;
                }
            }
        }

        /// <summary>
        /// Gets the current back buffer (write).
        /// </summary>
        public T Back
        {
            get
            {
                lock (lockObj)
                {
                    return backBuffer;
                }
            }
        }

        /// <summary>
        /// Swap front and back buffers.
        /// </summary>
        public void Swap()
        {
            lock (lockObj)
            {
                (backBuffer, frontBuffer) = (frontBuffer, backBuffer);
            }
        }
    }

}
