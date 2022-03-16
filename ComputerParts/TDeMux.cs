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
    class TDeMux : baseClasses.BaseLogicOnChange
    {
        [Serialize]
        protected int ControlPort1Value = 0;
        [Serialize]
        protected int InputValue = 0;
        [Serialize]
        protected int OutputValue = 0;
        [Serialize]
        protected int OutputValue1 = 0;
        [Serialize]
        protected int OutputValue2 = 0;
        [Serialize]
        protected int StoredValue = 0;
        [Serialize]
        protected int ControlReadyBit = 0;
        [Serialize]
        protected int LastControlReadyBit = 0;
        [Serialize]
        protected int Ticks = 0;

        protected override void ReadValues()
        {
        }

        protected override bool UpdateValues()
        {
            int PreviousControlPortValue = ControlPort1Value;
            int LastOutput = OutputValue;
            InputValue       = this.GetComponent<LogicPorts>()?.GetInputValue(TDeMuxConfig.INPUT_PORT_ID) ?? 0;
            ControlPort1Value = this.GetComponent<LogicPorts>()?.GetInputValue(TDeMuxConfig.CONTROL_PORT_ID1) ?? 0;

            InputValue = InputValue > 0 ? 1 : 0;

            //update output if control bit changed
            ControlReadyBit = (ControlPort1Value >> 3) & 0x01;
            if (LastControlReadyBit != ControlReadyBit )
            {
                OutputValue = StoredValue + 0;
                OutputValue1 = OutputValue & 0x0F;
                OutputValue2 = (OutputValue >> 4) & 0x0F;
            }
            LastControlReadyBit = ControlReadyBit;

            int updateBit = ControlPort1Value & 0b_0000_0111;
            if( updateBit == 0)
            {
                //StoredValue = 0;
            }
            int and_mask = ~(0x01 << updateBit);
            int set_mask = InputValue << updateBit;
            this.StoredValue &= and_mask;
            this.StoredValue |= set_mask;
            this.StoredValue &= 0xFF;


            if (OutputValue != LastOutput)
            {
                //this.GetComponent<LogicPorts>().SendSignal(TDeMuxConfig.OUTPUT_PORT_ID1, 0b_0101);
                //this.GetComponent<LogicPorts>().SendSignal(TDeMuxConfig.OUTPUT_PORT_ID2, ControlPort1Value);
                this.GetComponent<LogicPorts>().SendSignal(TDeMuxConfig.OUTPUT_PORT_ID1, OutputValue1);
                this.GetComponent<LogicPorts>().SendSignal(TDeMuxConfig.OUTPUT_PORT_ID2, OutputValue2);
            }
            return true;
        }


        protected override void UpdateVisuals()
        {
            int bit0 = 0, bit1 = 0, bit2 = 0, bit3 = 0;
            bit0 = Ticks == 8 ? 2 : 0;// Ticks == 0 ? 0 : 1;
            bit1 = ControlPort1Value > 0 ? 1 : 0;
            bit2 = Ticks >= 4 ? 1 : 0; //ControlPort2Value > 0 ? 1 : 0;
            bit3 = InputValue > 0 ? 1 : 0;
            kbac.Play("on_" + (bit0 + 3 * bit1 + 6 * bit2 + 12 * bit3), KAnim.PlayMode.Once, 1f, 0.0f);
        }
    }
}
