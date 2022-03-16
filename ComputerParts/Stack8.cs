using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;

namespace KBComputing
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class Stack8 : baseClasses.Base4x2
    {
        [Serialize]
        public int LastClock = 0;
        [Serialize]
        public Stack<byte> Stack = new Stack<byte>();

        protected override void ReadValues()
        {
            PortValue00 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID00) ?? 0;
            PortValue01 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID01) ?? 0;
            PortValue02 = this.GetComponent<LogicPorts>()?.GetOutputValue(Ram8Config.PORT_ID02) ?? 0;
            PortValue03 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID03) ?? 0;

            PortValue10 = this.GetComponent<LogicPorts>()?.GetOutputValue(Ram8Config.PORT_ID10) ?? 0;
            PortValue11 = this.GetComponent<LogicPorts>()?.GetOutputValue(Ram8Config.PORT_ID11) ?? 0;
            PortValue12 = 0;// this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID12) ?? 0;
            PortValue13 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID13) ?? 0;
        }

        protected override bool UpdateValues()
        {
            if( PortValue13 == LastClock)
            {
                return true;
            }
            LastClock = PortValue13;

            byte dataIn = (byte)((PortValue01 << 4 | PortValue00) & 0xFF);
            byte operation = (byte)PortValue03;
            byte flags = (byte)PortValue02;

            if( operation  == (byte)StackOpCodes.standby)
            {
                return true;
            }

            //clear fault flag
            flags = (byte)(flags & ~StackFlags.fault);

            try
            {
                bool done = true;
                switch (operation)
                {
                    //stack
                    case (int)StackOpCodes.pop:// = 0b0001,
                        Stack.Pop();
                        break;
                    case (int)StackOpCodes.push:// = 0b0010,
                        Stack.Push(dataIn);
                        break;
                    case (int)StackOpCodes.replace:// = 0b0011,
                        Stack.Pop();
                        Stack.Push(dataIn);
                        break;
                    //other
                    case (int)StackOpCodes.increment:// = 0b0101,
                        Stack.Push(1);
                        operation = (byte)StackOpCodes.add;
                        done = false;
                        break;
                    case (int)StackOpCodes.queue:// = 0b0110,
                        Stack.Append(dataIn);
                        break;
                    case (int)StackOpCodes.push0:// = 0b0111,
                        Stack.Push(0);
                        break;
                    default:
                        done = false;
                        break;
                }
                if( done)
                {
                    sendOutput(flags);
                    return true;
                }

                //sbyte srhs = (sbyte)rhs;
                //sbyte slhs = (sbyte)lhs;
                int irhs = (int)Stack.Pop();
                int ilhs = (int)Stack.Pop();
                byte rhs = (byte)irhs;
                byte lhs = (byte)ilhs;

                //clear carry flag
                flags = (byte)(flags & ~StackFlags.carry);

                switch (operation)
                {
                    //other
                    case (int)StackOpCodes.shift:// = 0b0100,
                        lhs = (byte)(rhs >= 0 ? lhs << rhs : lhs >> rhs);
                        Stack.Push(lhs);
                        break;
                    //arithmetic
                    case (int)StackOpCodes.add:// = 0b1000,
                        flags = setCarry(flags, ilhs + irhs);
                        Stack.Push((byte)(ilhs+irhs));
                        break;
                    case (int)StackOpCodes.subtract://= 0b1001,
                        flags = setCarry(flags, ilhs - irhs);
                        Stack.Push((byte)(ilhs-irhs));
                        break;
                    case (int)StackOpCodes.multiply:// = 0b1010,
                        flags = setCarry(flags, ilhs * irhs);
                        int mresult = ilhs * irhs;
                        Stack.Push((byte)(mresult >> 8));
                        Stack.Push((byte)(mresult & 0xFF));
                        break;
                    case (int)StackOpCodes.divide:// = 0b1011,
                        int div = ilhs / irhs;
                        int mod = ilhs % irhs;
                        Stack.Push((byte)div);
                        Stack.Push((byte)mod);
                        break;
                    //bitwise
                    case (int)StackOpCodes.or:// = 0b1100,
                        Stack.Push((byte)(lhs | rhs));
                        break;
                    case (int)StackOpCodes.and:// = 0b1101,
                        Stack.Push((byte)(lhs & rhs));
                        break;
                    case (int)StackOpCodes.xor:// = 0b1110,
                        Stack.Push((byte)(lhs ^ rhs));
                        break;
                    case (int)StackOpCodes.xnor:// = 0b1111,
                        Stack.Push((byte)~(lhs ^ rhs));
                        break;
                }
            }
            catch (InvalidOperationException e)
            {
                flags = (byte)(flags | StackFlags.fault);
            }
            sendOutput(flags);
            return true;
        }
        public byte setCarry(byte flag, int result)
        {
            bool carry = ((result & 0xFF00) >> 8) != 0;
            if( carry) flag |= StackFlags.carry;
            return flag;
        }
        public void sendOutput(byte flags)
        {
            sbyte dataOut = (sbyte)(Stack.Count <= 0 ? 0 : Stack.Peek());
            byte outHigh = (byte)((dataOut >> 4) & 0xFF);
            byte outLow = (byte)(dataOut & 0xFF);
            PortValue10 = outLow;
            PortValue11 = outHigh;
            this.GetComponent<LogicPorts>().SendSignal(Stack8Config.PORT_ID10, PortValue10);
            this.GetComponent<LogicPorts>().SendSignal(Stack8Config.PORT_ID11, PortValue11);

            //clear all but carry and fault flag
            flags &= (byte)~(StackFlags.fault | StackFlags.carry);

            //now set flags
            flags |= (byte)(dataOut == 0 ? StackFlags.zero : 0);
            flags |= (byte)(dataOut  < 0 ? StackFlags.sign : 0);
            if (Stack.Count > 256)
            {
                flags |= (byte)StackFlags.fault;
                while(Stack.Count > 256)
                    Stack.Pop();
            }

            PortValue02 = flags;
            this.GetComponent<LogicPorts>().SendSignal(Stack8Config.PORT_ID02, PortValue02);
            return;
        }
    }
}

            /*
00 - data
	00 - no operation
	01 - pop (discard)
	10 - push
	11 - replace
01 - other
	00 - pop and shift (sign determines direction)
	01 - pop and rotate (sign determines direction)
	10 - queue (put on top of stack)
	11 - push 0
10 - arithmetic
	00 - pop and add
	01 - pop and subtract
	10 - pop and multiply (put high bits on stack)
	11 - pop and divide (put modulus on stack)
11 - bitwise logic
	00 - pop and or
	01 - pop and and
	10 - pop and xor
	11 - pop and xnor

compound operations to do a unary operation:
	to negate (take additive inverse): push 0, push value, subtract
	to not (bitwise inverse): push 0, push value, xnor.
	to push a -1 (all bits on): push 0, push 0, xnor
	to push a 1: push 0, push 0, push 0, xnor, subtract
	to check if certain bits are set/unset: push value, push care bits as 1s, and, push desired bit values, xnor, zero flag signifies match.

four status flags:
	00 - zero
	01 - sign
	01 - carry / borrow out
	11 - stack empty             */
            /*
            int newOut = 0;

            int dataIn  = (PortValue00 << 4 | PortValue01) & 0xFF;
            int address = (PortValue02 << 4 | PortValue03) & 0xFF;
            int operation = PortValue13;
            int page_select = (PortValue13 >> 2) & 0x03;
            address = address | (page_select << 8);

            //write bit set
            if ((operation & 0x02) > 0)
            {
                Memory[address] = (byte)(dataIn & 0xFF);
            }
            //read bit set
            if ((operation & 0x01) > 0)
            {
                newOut = Memory[address] & 0xFF;
            }

            int newOut0 = (newOut >> 4) & 0x0F;
            int newOut1 = newOut & 0x0F;

            if (newOut0 != PortValue10)
            {
                PortValue10 = newOut0;
                this.GetComponent<LogicPorts>().SendSignal(Stack8Config.PORT_ID10, PortValue10);
            }
            
            if (newOut1 != PortValue11)
            {
                PortValue11 = newOut1;
                this.GetComponent<LogicPorts>().SendSignal(Stack8Config.PORT_ID11, PortValue11);
            }
            */

