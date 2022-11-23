using System;

namespace RichPackage.Threading
{
    public partial interface IDispatcher
    {
        bool Enabled { get; }

        /// <summary>
        /// Dispatch <paramref name="action"/> to be invoked on the main thread.
        /// </summary>
        void Invoke(Action action);

        /// <summary>
        /// Begin message pumping.
        /// </summary>
        void Run();

        /// <summary>
        /// Stop pumping messages.
        /// </summary>
        void Stop();
    }
}
