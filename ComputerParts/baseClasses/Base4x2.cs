using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;

namespace KBComputing.baseClasses
{
    [SerializationConfig(MemberSerialization.OptIn)]
    abstract class Base4x2 :
        //baseClasses.BaseLogicEveryTick
        baseClasses.BaseLogicOnChange
    {
        [Serialize] public int PortValue00 = 0;
        [Serialize] public int PortValue01 = 0;
        [Serialize] public int PortValue02 = 0;
        [Serialize] public int PortValue03 = 0;

        [Serialize] public int PortValue10 = 0;
        [Serialize] public int PortValue11 = 0;
        [Serialize] public int PortValue12 = 0;
        [Serialize] public int PortValue13 = 0;

        public void ReadValues()
        {
            PortValue00 = this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID00) ?? 0;
            PortValue01 = this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID01) ?? 0;
            PortValue02 = this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID02) ?? 0;
            PortValue03 = this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID03) ?? 0;

            PortValue10 = this.GetComponent<LogicPorts>()?.GetOutputValue(Base4x2Config.PORT_ID10) ?? 0;
            PortValue11 = this.GetComponent<LogicPorts>()?.GetOutputValue(Base4x2Config.PORT_ID11) ?? 0;
            PortValue12 = 0;// this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID12) ?? 0;
            PortValue13 = this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID13) ?? 0;
        }
        public override void UpdateVisuals()
        {
            ShowSymbolConditionally2(PortValue13, $"light{24}_bloom_green", $"light{24}_bloom_red");

            //16-19 outb
            ShowSymbolConditionally2(PortValue11 & (0x01 << 3), $"light{19}_bloom_green", $"light{19}_bloom_red");
            ShowSymbolConditionally2(PortValue11 & (0x01 << 2), $"light{18}_bloom_green", $"light{18}_bloom_red");
            ShowSymbolConditionally2(PortValue11 & (0x01 << 1), $"light{17}_bloom_green", $"light{17}_bloom_red");
            ShowSymbolConditionally2(PortValue11 & (0x01 << 0), $"light{16}_bloom_green", $"light{16}_bloom_red");
            // 20-23 out
            ShowSymbolConditionally2(PortValue10 & (0x01 << 3), $"light{23}_bloom_green", $"light{23}_bloom_red");
            ShowSymbolConditionally2(PortValue10 & (0x01 << 2), $"light{22}_bloom_green", $"light{22}_bloom_red");
            ShowSymbolConditionally2(PortValue10 & (0x01 << 1), $"light{21}_bloom_green", $"light{21}_bloom_red");
            ShowSymbolConditionally2(PortValue10 & (0x01 << 0), $"light{20}_bloom_green", $"light{20}_bloom_red");

            //in1b 0-3
            ShowSymbolConditionally2(PortValue01 & (0x01 << 3), $"light{3}_bloom_green", $"light{3}_bloom_red");
            ShowSymbolConditionally2(PortValue01 & (0x01 << 2), $"light{2}_bloom_green", $"light{2}_bloom_red");
            ShowSymbolConditionally2(PortValue01 & (0x01 << 1), $"light{1}_bloom_green", $"light{1}_bloom_red");
            ShowSymbolConditionally2(PortValue01 & (0x01 << 0), $"light{0}_bloom_green", $"light{0}_bloom_red");
            //in1 4-7
            ShowSymbolConditionally2(PortValue00 & (0x01 << 3), $"light{7}_bloom_green", $"light{7}_bloom_red");
            ShowSymbolConditionally2(PortValue00 & (0x01 << 2), $"light{6}_bloom_green", $"light{6}_bloom_red");
            ShowSymbolConditionally2(PortValue00 & (0x01 << 1), $"light{5}_bloom_green", $"light{5}_bloom_red");
            ShowSymbolConditionally2(PortValue00 & (0x01 << 0), $"light{4}_bloom_green", $"light{4}_bloom_red");

            //in2b 8-11
            ShowSymbolConditionally2(PortValue03 & (0x01 << 3), $"light{11}_bloom_green", $"light{11}_bloom_red");
            ShowSymbolConditionally2(PortValue03 & (0x01 << 2), $"light{10}_bloom_green", $"light{10}_bloom_red");
            ShowSymbolConditionally2(PortValue03 & (0x01 << 1), $"light{9}_bloom_green", $"light{9}_bloom_red");
            ShowSymbolConditionally2(PortValue03 & (0x01 << 0), $"light{8}_bloom_green", $"light{8}_bloom_red");
            //in2 12-15
            ShowSymbolConditionally2(PortValue02 & (0x01 << 3), $"light{15}_bloom_green", $"light{15}_bloom_red");
            ShowSymbolConditionally2(PortValue02 & (0x01 << 2), $"light{14}_bloom_green", $"light{14}_bloom_red");
            ShowSymbolConditionally2(PortValue02 & (0x01 << 1), $"light{13}_bloom_green", $"light{13}_bloom_red");
            ShowSymbolConditionally2(PortValue02 & (0x01 << 0), $"light{12}_bloom_green", $"light{12}_bloom_red");
        }
        protected void ShowSymbolConditionally2(
            int b,
          KAnimHashedString ifTrue,
          KAnimHashedString ifFalse)
        {
            kbac.SetSymbolVisiblity(ifTrue, b != 0);
            kbac.SetSymbolVisiblity(ifFalse, b == 0);
        }
    }
}
