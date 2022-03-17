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
    class TMux : baseClasses.BaseLogicOnChange, IMultiplexed
    {
        [Serialize]
        protected int ConfigClockDivider = 1; //1,2,4,8,16
        [Serialize]
        protected int ConfigMultiplexAmount = 1; //1,2,4,8,(16)  (default for multiplexers is 8)
        public int ClockDivision { get { return ConfigClockDivider; } set { ConfigClockDivider = value; } }
        public int MultiplexAmount { get { return ConfigMultiplexAmount; } set { ConfigMultiplexAmount = value; } }


        // cpus only
        [Serialize]
        protected int ConfigDataPinsUsed = 8; //

        //output only  = ConfigMultiplexLevel * ConfigDataPinsUsed
        [Serialize]
        protected int ConfigEffectiveBitWidth = 8;
        //output only  = ConfigDataPinsUsed / ConfigMultiplexClockMultiple
        [Serialize]
        protected int ConfigEffectiveThroughput = 8;

        [Serialize]
        protected int InputValue1 = 0;
        [Serialize]
        protected int InputValue2 = 0;
        [Serialize]
        protected int InputValue = 0;
        [Serialize]
        protected int OutputValue = 0;
        [Serialize]
        protected int ControlPort1Value = 0;

        protected override void ReadValues()
        {
        }

        protected override bool UpdateValues()
        {
            int PreviousControlPortValue = ControlPort1Value;
            int LastOutput = OutputValue;
            ControlPort1Value = this.GetComponent<LogicPorts>()?.GetInputValue(TMuxConfig.CONTROL_PORT_ID1) ?? 0;
            OutputValue = this.GetComponent<LogicPorts>()?.GetInputValue(TMuxConfig.OUTPUT_PORT_ID1) ?? 0;
            InputValue1 = this.GetComponent<LogicPorts>()?.GetInputValue(TMuxConfig.INPUT_PORT_ID1) ?? 0;
            InputValue2 = this.GetComponent<LogicPorts>()?.GetInputValue(TMuxConfig.INPUT_PORT_ID2) ?? 0;
            InputValue = (InputValue2 << 4) | InputValue1;

            ControlPort1Value &= 0b0111;
            //if rising edge detected, reset
            //if (PreviousControlPortValue != ControlPort1Value)
            //{

            this.OutputValue = (this.InputValue >> ControlPort1Value) & 0x01;

            if (OutputValue != LastOutput)
            {
                this.GetComponent<LogicPorts>().SendSignal(TMuxConfig.OUTPUT_PORT_ID1, OutputValue);
            }
            return true;
        }

        protected override void UpdateVisuals()
        {
            int bit0 = 0, bit1 = 0, bit2 = 0, bit3 = 0;
            bit0 = OutputValue > 0 ? 2 : 0;// Ticks == 0 ? 0 : 1;
            bit1 = ControlPort1Value > 0 ? 1 : 0;
            bit2 = InputValue2 > 0 ? 1 : 0; ;// Ticks >= 4 ? 1 : 0; //ControlPort2Value > 0 ? 1 : 0;
            bit3 = InputValue1 > 0 ? 1 : 0;
            kbac.Play("on_" + (bit0 + 3 * bit1 + 6 * bit2 + 12 * bit3), KAnim.PlayMode.Once, 1f, 0.0f);
        }

    }
}
