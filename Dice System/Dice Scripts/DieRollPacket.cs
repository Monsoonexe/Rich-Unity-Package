
namespace RichPackage.DiceSystem
{
    /// <summary>
    /// 2d6 + 3 and the result thereof.
    /// </summary>
    public struct DieRollPacket
    {
        public int dieCount;
        public int dieFaces;
        public int modifier;
        public int result;
        
        #region Constructors
        
        public DieRollPacket(int dieCount = 1,
            int dieFaces = 6, int modifier = 0, int result = -1)
        {
            this.dieCount = dieCount;
            this.dieFaces = dieFaces;
            this.modifier = modifier;
            this.result = result;
        }
        
        #endregion
    }
}
