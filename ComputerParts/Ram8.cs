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
    class Ram8 : baseClasses.Base4x2
    {
        [Serialize]
        public int[] Memory = new int[256];
        public override void LogicTick()
        {
            UpdateValues();
            int newOut = 0;

            int dataIn  = PortValue[0][0] << 4 | PortValue[0][1];
            int address = PortValue[0][2] << 4 | PortValue[0][3];
            int dataOut = PortValue[1][0] << 4 | PortValue[1][1];
            int operation = PortValue[1][3];

            //write bit set
            if ((operation & 0x02) > 0)
            {
                Memory[address] = dataIn;
            }
            //read bit set
            if ((operation & 0x01) > 0)
            {
                newOut = Memory[address];
            }

            if (newOut != dataOut)
            {
                dataOut = newOut;
                this.GetComponent<LogicPorts>().SendSignal(Ram8Config.PORT_ID[1][0], dataOut >> 4 & 0b1111);
                this.GetComponent<LogicPorts>().SendSignal(Ram8Config.PORT_ID[1][1], dataOut & 0b1111);
            }
            UpdateVisuals();
        }
    }
}
