public interface IAnimateBase
{
    bool IsAnimating { get; }
    void Stop();
}

/// <summary>
/// 
/// </summary>
public interface IAnimate : IAnimateBase
{
    void Play();
}

public interface IAnimate<T> : IAnimateBase
{
    void Play(T puppet);
}