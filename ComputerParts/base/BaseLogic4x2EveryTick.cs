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
    abstract class BaseLogic4x2EveryTick :
        //baseClasses.BaseLogicEveryTick
        baseClasses.BaseLogicEveryTick
    {
        [Serialize] public int PortValue00 = 0;
        [Serialize] public int PortValue01 = 0;
        [Serialize] public int PortValue02 = 0;
        [Serialize] public int PortValue03 = 0;

        [Serialize] public int PortValue10 = 0;
        [Serialize] public int PortValue11 = 0;
        [Serialize] public int PortValue12 = 0;
        [Serialize] public int PortValue13 = 0;

        protected override void UpdateVisuals()
        {
            if( !this.isActiveAndEnabled)
            {
                this.kbac.Play("off");
                return;
            }
            this.kbac.Play("on_0");


            ShowSymbolConditionally2(PortValue13, $"light{24}_bloom_green", $"light{24}_bloom_red");
            //in1b 0-3
            ShowSymbolConditionally2(PortValue00 & (0x01 << 3), $"light{3}_bloom_green", $"light{3}_bloom_red");
            ShowSymbolConditionally2(PortValue00 & (0x01 << 2), $"light{2}_bloom_green", $"light{2}_bloom_red");
            ShowSymbolConditionally2(PortValue00 & (0x01 << 1), $"light{1}_bloom_green", $"light{1}_bloom_red");
            ShowSymbolConditionally2(PortValue00 & (0x01 << 0), $"light{0}_bloom_green", $"light{0}_bloom_red");
            //in1 4-7
            ShowSymbolConditionally2(PortValue01 & (0x01 << 3), $"light{7}_bloom_green", $"light{7}_bloom_red");
            ShowSymbolConditionally2(PortValue01 & (0x01 << 2), $"light{6}_bloom_green", $"light{6}_bloom_red");
            ShowSymbolConditionally2(PortValue01 & (0x01 << 1), $"light{5}_bloom_green", $"light{5}_bloom_red");
            ShowSymbolConditionally2(PortValue01 & (0x01 << 0), $"light{4}_bloom_green", $"light{4}_bloom_red");
            //in2b 8-11
            ShowSymbolConditionally2(PortValue02 & (0x01 << 3), $"light{11}_bloom_green", $"light{11}_bloom_red");
            ShowSymbolConditionally2(PortValue02 & (0x01 << 2), $"light{10}_bloom_green", $"light{10}_bloom_red");
            ShowSymbolConditionally2(PortValue02 & (0x01 << 1), $"light{9}_bloom_green", $"light{9}_bloom_red");
            ShowSymbolConditionally2(PortValue02 & (0x01 << 0), $"light{8}_bloom_green", $"light{8}_bloom_red");
            //in2 12-15
            ShowSymbolConditionally2(PortValue03 & (0x01 << 3), $"light{15}_bloom_green", $"light{15}_bloom_red");
            ShowSymbolConditionally2(PortValue03 & (0x01 << 2), $"light{14}_bloom_green", $"light{14}_bloom_red");
            ShowSymbolConditionally2(PortValue03 & (0x01 << 1), $"light{13}_bloom_green", $"light{13}_bloom_red");
            ShowSymbolConditionally2(PortValue03 & (0x01 << 0), $"light{12}_bloom_green", $"light{12}_bloom_red");
            //16-19 outb
            ShowSymbolConditionally2(PortValue10 & (0x01 << 3), $"light{19}_bloom_green", $"light{19}_bloom_red");
            ShowSymbolConditionally2(PortValue10 & (0x01 << 2), $"light{18}_bloom_green", $"light{18}_bloom_red");
            ShowSymbolConditionally2(PortValue10 & (0x01 << 1), $"light{17}_bloom_green", $"light{17}_bloom_red");
            ShowSymbolConditionally2(PortValue10 & (0x01 << 0), $"light{16}_bloom_green", $"light{16}_bloom_red");
            // 20-23 out
            ShowSymbolConditionally2(PortValue11 & (0x01 << 3), $"light{23}_bloom_green", $"light{23}_bloom_red");
            ShowSymbolConditionally2(PortValue11 & (0x01 << 2), $"light{22}_bloom_green", $"light{22}_bloom_red");
            ShowSymbolConditionally2(PortValue11 & (0x01 << 1), $"light{21}_bloom_green", $"light{21}_bloom_red");
            ShowSymbolConditionally2(PortValue11 & (0x01 << 0), $"light{20}_bloom_green", $"light{20}_bloom_red");

            HideOperator();
        }
        protected void ShowSymbolConditionally2(
            int b,
          KAnimHashedString ifTrue,
          KAnimHashedString ifFalse)
        {
            kbac.SetSymbolVisiblity(ifTrue, b != 0);
            kbac.SetSymbolVisiblity(ifFalse, b == 0);
        }
        private void HideOperator()
        {
            kbac.SetSymbolVisiblity("op_add",false);
            kbac.SetSymbolVisiblity("op_div",false);
            kbac.SetSymbolVisiblity("op_exp",false);
            kbac.SetSymbolVisiblity("op_bits_left",false);
            kbac.SetSymbolVisiblity("op_bits_right",false);
            kbac.SetSymbolVisiblity("op_mod",false);
            kbac.SetSymbolVisiblity("op_mul",false);
            kbac.SetSymbolVisiblity("op_minus",false);
            kbac.SetSymbolVisiblity("op_less",false);
            kbac.SetSymbolVisiblity("op_lessThanOrEqual",false);
            kbac.SetSymbolVisiblity("op_more",false);
            kbac.SetSymbolVisiblity("op_moreThanOrEqual",false);
            kbac.SetSymbolVisiblity("op_addadd",false);
            kbac.SetSymbolVisiblity("op_equality",false);
            kbac.SetSymbolVisiblity("op_not_equality",false);

        }

        private void ToggleOperator(
          bool isOperator,
          KAnimHashedString anim)
        {
            kbac.SetSymbolVisiblity(anim, isOperator);
        }
    }
}
