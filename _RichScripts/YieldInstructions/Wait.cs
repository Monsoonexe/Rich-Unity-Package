namespace UnityEngine
{
    public static class Wait
    {
        public static readonly WaitForSeconds ForOneSecond = new WaitForSeconds(1.0f);
        public static readonly WaitForSeconds ForHalfSecond = new WaitForSeconds(0.5f);
        public static readonly WaitForSeconds ForQuarterSecond = new WaitForSeconds(0.25f);
        public static readonly WaitForSeconds ForTenthSecond = new WaitForSeconds(0.1f);
        public const object ForNextFrame = null;
    }
}
