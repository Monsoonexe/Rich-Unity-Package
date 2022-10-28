using System;

namespace RichPackage.Threading
{
    public interface IDispatcher
    {
        bool Enabled { get; }

        void Invoke(Action action);
        void Run();
        void Stop();
    }
}