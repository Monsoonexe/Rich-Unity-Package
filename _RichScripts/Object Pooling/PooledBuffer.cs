//using RichPackage.GuardClauses;
using System;
//using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace RichPackage.Pooling
{
    /// <summary>
    /// A lightweight class that wraps calls to <see cref="ArrayPool{T}"/>
    /// and implements the <see cref="IDisposable"/> pattern.
    /// 
    /// <para/>
    /// Usage: <br/>
    /// using (var buffer = new PooledBuffer{T}(1024)) <br/>
    /// { <br/>
    ///     //use buffer <br/>
    /// } <br/>
    /// </summary>
    public class PooledBuffer<T> : IDisposable, IEnumerable<T>
    {
        private bool isDisposed;
        private T[] buffer;

        public int Length => buffer.Length;

        #region Constructors

        public PooledBuffer(int minimumLength)
        {
            //validate
            //GuardAgainst.IsZeroOrNegative(minimumLength, nameof(minimumLength));

            //init
            buffer = ArrayPool<T>.Shared.Rent(minimumLength);
        }

        ~PooledBuffer()
        {
            Dispose();
        }

        #endregion Constructors

        #region IDisposable

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                //always clear reference types to prevent keeping dead references alive
                bool clear = !typeof(T).IsValueType;
                ArrayPool<T>.Shared.Return(buffer, clear);
                buffer = null;
                GC.SuppressFinalize(this); // no need to dispose again on clean up
            }
        }

        #endregion IDisposable

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
            => buffer.GetEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => buffer.GetEnumerator();

        #endregion IEnumerable

        public T this[int i]
        {
            get
            {
                //validate
                if (isDisposed)
                    throw new ObjectDisposedException($"{typeof(PooledBuffer<T>)}");

                return buffer[i];
            }

            set
            {
                //validate
                if (isDisposed)
                    throw new ObjectDisposedException($"{typeof(PooledBuffer<T>)}");

                buffer[i] = value;
            }
        }

        public static implicit operator T[] (PooledBuffer<T> a) => a.buffer;
    }
}
