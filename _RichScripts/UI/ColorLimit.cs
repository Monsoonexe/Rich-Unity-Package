using System;
using UnityEngine;

namespace RichPackage.UI
{
    [Serializable]
    public struct ColorLimit
    {
        public int limit;
        public Color color;

        public ColorLimit(int limit, Color color)
        {
            this.limit = limit;
            this.color = color;
        }
    }
}
