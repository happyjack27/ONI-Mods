using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerParts
{
    public class BinaryUtils
    {
        private static Dictionary<int, int> _minValues = new Dictionary<int, int>()
        {
            {4, 1 << 3 },
            {8, 1 << 7 }
        };

        public static bool IsNegative(int value, int bits)
        {
            var minValue = _minValues[bits];
            var isNegative = (value & minValue) == minValue;
            return isNegative;
        }

        public static int GetTwosComplement(int value, int bits)
        {
            var minValue = _minValues[bits];
            if (IsNegative(value, bits))
            {
                return (value ^ minValue) + 1;
            }
            else
            {
                return value;
            }
        }

        public static int GetValue(int value, int bits)
        {
            var minValue = _minValues[bits];
            if (IsNegative(value, bits))
            {
                return -((value ^ minValue) + 1);
            }
            else
            {
                return value;
            }
        }
    }
}
