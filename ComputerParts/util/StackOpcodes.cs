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
        peek = 0b001,
        pop = 0b0010,
        push = 0b0011,
        //other
        increment = 0b0100,
        swap = 0b0101,
        copy_from = 0b0111,
        copy_to = 0b0110,
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
