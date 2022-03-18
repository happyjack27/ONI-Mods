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
    class Ram8 : baseClasses.BaseLogic4x2OnChange, IMemoryContents
    {

        [Serialize]
        public byte[] Memory = new byte[1024];

        [Serialize]
        public string DisplayStyle = "HEX"; // HEX char int stackops instructions 
        static readonly char[] HEX = "0123456789ABCDEF".ToCharArray();
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
            ReadValues();
            UpdateValues();
            UpdateVisuals();
        }

        public bool setContents(int bank, string style, string value)
        {
            string s = value;
            //s = s.Replace(" ", "").Replace("\n", "").Replace("\r", "").ToUpper();
            int w = 0+bank*256;
            int h = 0;
            byte b = 0;
            for (int r = 0; r < s.Length; r++)
            {
                char c = s[r];
                if( c >= 'a' && c <= 'z')
                {
                    c = (char)(c - 'a' + 'A');
                }
                if( !HEX.Contains(c))
                {
                    continue;
                }
                if (h == 0)
                {
                    //b = 0;
                    b = REVERSE_HEX[c];
                    h = 1;
                } else
                if (h == 1)
                {
                    b <<= 4;
                    b |= REVERSE_HEX[c];
                    Memory[w] = b;
                    w++;
                    h = 0;
                }
            }
            ReadValues();
            UpdateValues();
            UpdateVisuals();
            return true;
        }

        public string getContents(int bank, string style) {
            //return "test string\n more text";
            int offset = bank * 256;
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            for( int i = 0; i < 256; i++) 
            {
                if (i != 0)
                {
                    if (i % 256 == 0)
                    {
                        sb.Append("\n\n\n\n");
                    }
                    else
                    if (i % 64 == 0)
                    {
                        sb.Append("\n\n");
                    }
                    else
                    if (i % 8 == 0)
                    {
                        sb.Append("\n");
                    }
                    else
                    if (i % 4 == 0)
                    {
                        sb.Append(" ");
                    }
                }
                byte b = Memory[i+offset];
                sb.Append((char)HEX[(b >> 4) & 0x0F]);
                sb.Append((char)HEX[(b >> 0) & 0x0F]);
                sb.Append(" ");
            }
            return sb.ToString();
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
        protected override void OnCopySettings(object data)
        {
            Ram8 component = ((GameObject)data).GetComponent<Ram8>();
            if (component == null) return;
            this.DisplayStyle = (string)component.DisplayStyle.Clone();
            this.Memory = (byte[])component.Memory.Clone();
            //public byte[] Memory = new byte[1024];

            //[Serialize]
            ReadValues();
            UpdateValues();
            UpdateVisuals();
        }

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

            int dataIn  = (PortValue01 << 4 | PortValue00) & 0xFF;
            int address = (PortValue03 << 4 | PortValue02) & 0xFF;
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

            int newOut1 = (newOut >> 4) & 0x0F;
            int newOut0 = newOut & 0x0F;

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
