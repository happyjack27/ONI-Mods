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
        public string DisplayMode = "HEX"; // HEX char int stackops instructions

        [Serialize]
        public int DisplayBank = 0; // HEX char int stackops instructions
        
        [Serialize]
        public int DisplayOffset = 0; // HEX char int stackops instructions


        public string ContentDisplayMode { get { return DisplayMode; } set { DisplayMode = value; } }
        public int ContentDisplayBank { get { return DisplayBank; } set { DisplayBank = value; } }
        public int ContentDisplayOffset { get { return DisplayOffset; } set { DisplayOffset = value; } }

        public byte[] getBytes(int offset, int size)
        {
            if( offset + size > Memory.Length )
            {
                size = Memory.Length - offset;
            }
            byte[] bankBytes = new byte[size];
            for (int i = 0; i < bankBytes.Length; i++)
            {
                bankBytes[i] = Memory[i + offset];
            }
            return bankBytes;
        }

        public void setBytes(int offset, byte[] bytes)
        {
            int size = bytes.Length;
            if (offset + size > Memory.Length)
            {
                size = Memory.Length - offset;
            }
            for (int i = 0; i < size; i++)
            {
                Memory[i + offset] = bytes[i];
            }
            ReadValues();
            UpdateValues();
            UpdateVisuals();
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
            this.DisplayBank = component.DisplayBank;
            this.DisplayOffset = component.DisplayOffset;
            this.DisplayMode = (string)component.DisplayMode.Clone();
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

        protected override void UpdateValues()
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
        }
    }
}
