using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerParts
{
/*
four control bits:

00 - stack operations
0000 - standby
0001 - pop (discard)
0010 - push
0011 - replace bottom
01 - reserved (unary ops?) (shift? rotate?)
//increment, decrement, negate, not, push zero, clear, shift, rotate, count...
0100 - shift
0101 - rotate
0110 - increment (only if no carry in connected)
0111 - push zero
10 - arithmetic
1000 - add
1001 - subtract
1010 - multiply (put high bits on stack)
1011 - divide (put modulus on stack)
11 - logic
1100 - or
1101 - and
1110 - xor
1111 - xand

status output:
bit 0: zero
bit 1: carry / borrow out
bit 2: sign
bit 3: stack empty
*/
    enum ALU_opcodes
    {
        standby = 0x0,
        pop = 0x1,
        push = 0x2,
        replace = 0x3,
        sp_0 = 0x4,
        sp_1 = 0x5,
        sp_2 = 0x6,
        sp_3 = 0x7,
        add = 0x8,
        subtract = 0x9,
        multiply = 0xA,
        divide = 0xB,
        or = 0xC,
        and = 0xD,
        xor = 0xE,
        xand = 0xF,
    }
}
