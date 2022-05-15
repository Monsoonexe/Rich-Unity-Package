using System;

namespace RichPackage.DiceSystem
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct Die
    {
        public int[] faceValues;

        public int FaceCount { get => faceValues.Length; }

        public int faceUpValue;

        #region Constructors

        public Die(int[] faceValues = null, int faceUpValue = -1)
        {
            this.faceUpValue = faceUpValue;
            this.faceValues = faceValues;
        }

        #endregion
    }
}
