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
    class Rom4 : baseClasses.BaseLogicOnChange, IROM
    {


        //note: also add overflow bit, making it a clock divider
        //add to rising and falling edge detecotr: mode: whether to pulse or to toggle (falling togle serves as clock divider

        [Serialize]
        public byte[] Memory = new byte[]
        {
			0x00,0x01,0x02,0x03,
			0x04,0x05,0x06,0x07,
			0x08,0x09,0x0A,0x0B,
			0x0C,0x0D,0x0E,0x0F,
		};

		[Serialize] public int PortValue00 = 0;
		[Serialize] public int PortValue10 = 0;


		private static readonly Color COLOR_ON = new Color(0.3411765f, 0.7254902f, 0.3686275f);
        private static readonly Color COLOR_OFF = new Color(0.9529412f, 0.2901961f, 0.2784314f);
        private static readonly Color COLOR_DISABLED = new Color(1.0f, 1.0f, 1.0f);

        private static readonly KAnimHashedString[] IN_DOT = { "b1", "b2", "b3", "b4" };
        private static readonly KAnimHashedString[] OUT_DOT = { "c1", "c2", "c3", "c4" };
        private static readonly KAnimHashedString[] IN_LINE = { "in1", "in2", "in3", "in4" };
        private static readonly KAnimHashedString[] OUT_LINE = { "out1", "out2", "out3", "out4" };

        public const int NO_BIT = -1;

        protected override void OnCopySettings(object data)
        {
            Rom4 component = ((GameObject)data).GetComponent<Rom4>();
            if (component == null) return;
            this.Memory = (byte[])component.Memory.Clone();

            ReadValues();
            UpdateValues();
            UpdateVisuals();
        }

        protected override void ReadValues()
        {
			PortValue00 = this.GetComponent<LogicPorts>()?.GetInputValue(Rom4Config.PORT_ID00) ?? 0;
			PortValue10 = this.GetComponent<LogicPorts>()?.GetOutputValue(Rom4Config.PORT_ID10) ?? 0;
		}

		protected override bool UpdateValues()
        {
			int new_out = Memory[PortValue00];
			if( PortValue10 != new_out)
            {
				PortValue10 = new_out;
				this.GetComponent<LogicPorts>().SendSignal(Rom4Config.PORT_ID10, PortValue10);
			}

			return true;
        }

		protected override void UpdateVisuals()
		{
			// when there is not an output, we are supposed to play the off animation
			// set the tints for the wiring bits on the edges of the remapping (not the central connectors)
			int inVal = PortValue00;
			int curOut = PortValue10;
			for (int i = 0; i < 4; i++)
			{
				kbac.SetSymbolTint(IN_DOT[i], BitOn(inVal, i) ? COLOR_ON : COLOR_OFF);
				kbac.SetSymbolTint(IN_LINE[i], BitOn(inVal, i) ? COLOR_ON : COLOR_OFF);
				kbac.SetSymbolTint(OUT_DOT[i], BitOn(curOut, i) ? COLOR_ON : COLOR_OFF);
				kbac.SetSymbolTint(OUT_LINE[i], BitOn(curOut, i) ? COLOR_ON : COLOR_OFF);
			}

			// turn off all of the lights (there are two pairs of lights, one on each side)
			for (int i = 0; i < 4 * 2; i++)
			{
				kbac.SetSymbolVisiblity(Light(i, 0), false);
				kbac.SetSymbolVisiblity(Light(i, 1), false);
			}
			// turn on only the lights that should be shown (pick green vs red based on the values of the logic wires)
			for (int i = 0; i < 4; i++)
			{
				kbac.SetSymbolVisiblity(Light(i, BitOn(inVal, i) ? 0 : 1), true);
				kbac.SetSymbolVisiblity(Light(4 + i, BitOn(curOut, i) ? 0 : 1), true);
			}

			kbac.Play("on", KAnim.PlayMode.Once, 1f, 0.0f);

		}

		private string Light(int pos, int state)
		{
			return $"light_bloom_{pos}_{state}";

		}

		private bool BitOn(int wire, int pos)
		{
			return (wire & (0x1 << pos)) > 0;
		}

        public byte[] getBytes()
        {
			return Memory;
        }

        public void setBytes(byte[] values)
        {
            Memory = values;

			ReadValues();
			UpdateValues();
			UpdateVisuals();
		}
	}
}
