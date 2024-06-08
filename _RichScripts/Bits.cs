using System;

namespace RichPackage
{
    public static class Bits
    {
        public static int CountSet(int mask)
        {
            int count = 0;
            while (mask != 0)
            {
                // Increment count if the least significant bit is set (i.e., num & 1 == 1)
                count += mask & 1;
                // Right shift num to check the next bit
                mask >>= 1;
            }

            return count;
        }

        public static int CountUnset(int mask)
        {
            // bits in an int
            return sizeof(int) * 8 - CountSet(mask);
        }
    }
}
