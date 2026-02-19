namespace RichPackage.RNG
{
    public static class Rng
    {
        public static readonly UnityRandom Unity = new UnityRandom(); // a solid choice
        public static readonly SystemRandom System = new SystemRandom(); // what, do you want more control?
        public static readonly CryptoRandom Crypto = new CryptoRandom(); // what, are you making a slot machine?

        // you know, do whatever you want
        public static IRandomNumberGenerator Current = Unity;

        public static float Next() => Current.Next();
        public static float Range(float min, float max) => Current.Range(min, max);
        public static int Range(int min, int max) => Current.Range(min, max);

        /// <summary>
        /// Makes a random draw against <paramref name="against"/>.
        /// </summary>
        /// <param name="against">[0,1] Chance of success.</param>
        public static bool Check(this IRandomNumberGenerator rng, float against)
        {
#if UNITY_2020_OR_NEWER
            Assert.IsTrue(against is >= 0 and <= 1, $"Out of range '{against}' [0,1]");
#endif

            return rng.Next() <= against;
        }
    }

    /// <summary>
    /// Base class for a random number generator.
    /// </summary>
    public abstract class ARandomNumberGenerator : IRandomNumberGenerator
    {
        public abstract float Next();
        public abstract float Range(float min, float max);
        public abstract int Range(int min, int max);
    }

    public sealed class SystemRandom : ARandomNumberGenerator, IRandomNumberGenerator
    {
        private readonly System.Random random;

        public SystemRandom()
        {
            random = new System.Random();
        }

        public SystemRandom(int seed)
        {
            random = new System.Random(seed);
        }

        public override float Next() => random.Next();
        public override float Range(float min, float max)
            => min + (float)(Next() * (max - min + 1));
        public override int Range(int min, int max)
            => min + (int)(Next() * (max - min + 1));
    }

    public sealed class UnityRandom : ARandomNumberGenerator, IRandomNumberGenerator
    {
        public UnityRandom() { }
        public UnityRandom(int seed)
        {
            UnityEngine.Random.InitState(seed);
        }
        public override float Next() => UnityEngine.Random.value;
        public override float Range(float min, float max) => UnityEngine.Random.Range(min, max);
        public override int Range(int min, int max) => UnityEngine.Random.Range(min, max);

    }

    public sealed class CryptoRandom : ARandomNumberGenerator, IRandomNumberGenerator, System.IDisposable
    {
        private readonly System.Security.Cryptography.RandomNumberGenerator random;
        private readonly byte[] buffer = new byte[4];

        public CryptoRandom()
        {
            random = System.Security.Cryptography.RandomNumberGenerator.Create();
        }

        public override float Next()
        {
            random.GetBytes(buffer);
            return System.BitConverter.ToSingle(buffer, 0);
        }

        public override float Range(float min, float max)
            => min + (float)(Next() * (max - min + 1));

        public override int Range(int min, int max)
            => min + (int)(Next() * (max - min + 1));

        void System.IDisposable.Dispose() => random.Dispose();
    }

    public interface IRandomNumberGenerator
    {
        /// <returns>[0,1]</returns>
        float Next();
        /// <returns>[min, max]</returns>
        float Range(float min, float max);
        /// <returns>[min, max)</returns>
        int Range(int min, int max);
    }
}
