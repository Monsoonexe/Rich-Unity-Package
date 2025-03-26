
namespace RichPackage.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAnimateBase
    {
        bool IsAnimating { get; }
        float Duration { get; set; }
        void Stop();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAnimate : IAnimateBase
    {
        void Play();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAnimate<T> : IAnimateBase
    {
        void Play(T puppet);
    }
}
