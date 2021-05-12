using System;

///<summary>
/// Thrown when another instance attempts to set itself as the Singleton.
///</summary>
public class SingletonException : Exception
{
    public SingletonException()
    {

    }
    public SingletonException(string msg)
        :base(msg)
    {

    }
}
