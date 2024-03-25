using System;
using UnityEngine.Assertions;

public readonly struct ActionDisposable : IDisposable
{
    private readonly Action action;

    public ActionDisposable(Action action)
    {
        Assert.IsNotNull(action);
        this.action = action;
    }

    public void Dispose()
    {
        action();
    }
}
