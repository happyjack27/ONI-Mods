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
    class Ram8 : baseClasses.Base4x2, IMemoryContents
    {

        [Serialize]
        public byte[] Memory = new byte[1024];

        [Serialize]
        public string DisplayStyle = "HEX"; // HEX char int stackops instructions 
        static readonly string HEX = "0123456789ABCDEF";
        static readonly byte[] REVERSE_HEX;

        public string ContentDisplayStyle { get { return DisplayStyle; } set { DisplayStyle = value; } }

        static Ram8() {
            REVERSE_HEX = new byte[256];
            for( byte i = 0; i < HEX.Length; i++)
            {
                REVERSE_HEX[HEX[i]] = i;
            }
        }
        public void ClearContents(int bank)
        {
            int start = bank * 256;
            int end = start + 256;
            for( int i = start; i < end; i++ )
            {
                Memory[i] = 0;
            }
        }

        public bool setContents(int bank, string value)
        {
            string s = "" + value;
            s = s.Replace(" ", "").Replace("\n", "").ToUpper();
            int w = 0+bank*256;
            int h = 0;
            byte b = 0;
            for (int r = 0; r < s.Length; r++)
            {
                if (h == 0)
                {
                    b = REVERSE_HEX[s[r]];
                    h = 1;
                }
                if (h == 1)
                {
                    b <<= 4;
                    b |= REVERSE_HEX[s[r]];
                    Memory[w] = b;
                    w++;
                    h = 0;
                }
            }
            return true;
        }

        public string getContents(int bank) {
            string s = "";
            int offset = bank * 256;
            for( int i = 0; i < 256; i++) 
            {
                if (i % 256 == 0)
                {
                    s += "\n\n\n\n";
                }
                if (i % 64 == 0)
                {
                    s += "\n\n";
                }
                if (i % 16 == 0)
                {
                    s += "\n";
                }
                else
                if (i % 4 == 0)
                {
                    s += " ";
                }
                byte b = Memory[i+offset];
                s += HEX[(b >> 4) & 0x0F];
                s += HEX[(b >> 0) & 0x0F];
                s += " ";
            }
            return s;
        }

        /*
    {
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,

        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,

        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,

        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
        0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,

    };
        */
        protected override void ReadValues()
        {
            PortValue00 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID00) ?? 0;
            PortValue01 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID01) ?? 0;
            PortValue02 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID02) ?? 0;
            PortValue03 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID03) ?? 0;

            PortValue10 = this.GetComponent<LogicPorts>()?.GetOutputValue(Ram8Config.PORT_ID10) ?? 0;
            PortValue11 = this.GetComponent<LogicPorts>()?.GetOutputValue(Ram8Config.PORT_ID11) ?? 0;
            PortValue12 = 0;// this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID12) ?? 0;
            PortValue13 = this.GetComponent<LogicPorts>()?.GetInputValue(Ram8Config.PORT_ID13) ?? 0;
        }

        protected override bool UpdateValues()
        {
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
                this.GetComponent<LogicPorts>().SendSignal(Ram8Config.PORT_ID10, PortValue10);
            }
            
            if (newOut1 != PortValue11)
            {
                PortValue11 = newOut1;
                this.GetComponent<LogicPorts>().SendSignal(Ram8Config.PORT_ID11, PortValue11);
            }
            
            return true;
        }

    }
}
