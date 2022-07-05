using System;
using UnityEngine;

namespace RichPackage.UI
{
    [Serializable]
    public struct ColorLimit
    {
        public int percentile;
        public Color color;

        public ColorLimit(int percentile, Color color)
        {
            this.percentile = percentile;
            this.color = color;
        }
    }
}
