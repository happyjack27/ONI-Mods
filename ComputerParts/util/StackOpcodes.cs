using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBComputing
{
    enum StackOpCodes
    {
        //stack
        standby = 0b0000,
        pop = 0b0001,
        push = 0b0010,
        replace = 0b0011,
        //other
        shift = 0b0100,
        increment = 0b0101,
        queue = 0b0110,
        push0 = 0b0111,
        //arithmetic
        add = 0b1000,
        subtract = 0b1001,
        multiply = 0b1010,
        divide = 0b1011,
        //bitwise
        or = 0b1100,
        and = 0b1101,
        xor = 0b1110,
        xnor = 0b1111,
    }
    class StackFlags
    {
        public static readonly byte zero = 0b0001;
        public static readonly byte sign = 0b0010;
        public static readonly byte carry = 0b0100;
        public static readonly byte fault = 0b1000;
    }
}
