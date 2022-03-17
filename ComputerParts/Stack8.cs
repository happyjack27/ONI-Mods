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

            //clear fault flag
            flags = (byte)(flags & ~StackFlags.fault);

            try
            {
                bool done = true;
                switch (operation)
                {
                    //stack
                    case (int)StackOpCodes.standby:// = 0b0001,
                    case (int)StackOpCodes.peek:// = 0b0001,
                        break;
                    case (int)StackOpCodes.pop:// = 0b0001,
                        Stack.Pop();
                        break;
                    case (int)StackOpCodes.push:// = 0b0010,
                        Stack.Push(dataIn);
                        break;
                    //special
                    case (int)StackOpCodes.increment:// = 0b0101,
                        Stack.Push(1);
                        operation = (byte)StackOpCodes.add;
                        done = false;
                        break;
                    case (int)StackOpCodes.swap:// = 0b1000,
                        {
                            int a = Stack.Pop();
                            int b = Stack.Pop();
                            Stack.Push((byte)a);
                            Stack.Push((byte)b);
                        }
                        break;
                    case (int)StackOpCodes.copy_from:// = 0b1000,
                        {
                            int a = Stack.Pop();
                            byte b = a == 0 ? (byte)Stack.Count : Stack.ElementAt(a);
                            Stack.Push(b);
                        }
                        break;
                    default:
                        done = false;
                        break;
                }
                if( done)
                {
                    sendOut(flags, operation == (byte)StackOpCodes.peek);
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
            sendOut(flags, false);
            return true;
        }
        public byte setCarry(byte flag, int result)
        {
            bool carry = ((result & 0xFF00) >> 8) != 0;
            if( carry) flag |= StackFlags.carry;
            return flag;
        }

        public void sendOut(byte flags, bool sendData)
        {
            sbyte dataOut = (sbyte)(Stack.Count <= 0 ? 0 : Stack.Peek());
            byte outHigh = (byte)((dataOut >> 4) & 0xFF);
            byte outLow = (byte)(dataOut & 0xFF);
            PortValue10 = sendData ? outLow  : 0;
            PortValue11 = sendData ? outHigh : 0;
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
