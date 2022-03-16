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
    abstract class Base4x2 : baseClasses.BaseLogicOnChange
    {
        [Serialize]
        public int[][] PortValue = new int[2][] { new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };

        public void ReadValues()
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 1 && j == 2)
                        {
                            PortValue[i][j] = 0;
                            continue;
                        }
                        PortValue[i][j] = this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID[i][j]) ?? 0;
                    }
                }
            }
            catch
            {

            }
        }
        public override void UpdateVisuals()
        {
            try
            {
                var nw1 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][0]));
                var nw1b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][1]));
                var nw2 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][2]));
                var nw2b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][3]));

                var nwOut = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[1][0]));
                var nwOutb = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[1][1]));

                var nwOp = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[1][3]));

                ShowSymbolConditionally(nwOp, () => nwOp.OutputValue > 0, $"light{24}_bloom_green", $"light{24}_bloom_red");


                //16-19 outb
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(3, nwOutb.OutputValue), $"light{19}_bloom_green", $"light{19}_bloom_red");
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(2, nwOutb.OutputValue), $"light{18}_bloom_green", $"light{18}_bloom_red");
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(1, nwOutb.OutputValue), $"light{17}_bloom_green", $"light{17}_bloom_red");
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(0, nwOutb.OutputValue), $"light{16}_bloom_green", $"light{16}_bloom_red");
                // 20-23 out
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(3, nwOut.OutputValue), $"light{23}_bloom_green", $"light{23}_bloom_red");
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(2, nwOut.OutputValue), $"light{22}_bloom_green", $"light{22}_bloom_red");
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(1, nwOut.OutputValue), $"light{21}_bloom_green", $"light{21}_bloom_red");
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(0, nwOut.OutputValue), $"light{20}_bloom_green", $"light{20}_bloom_red");

                //in1b 0-3
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(3, nw1b.OutputValue), $"light{3}_bloom_green", $"light{3}_bloom_red");
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(2, nw1b.OutputValue), $"light{2}_bloom_green", $"light{2}_bloom_red");
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(1, nw1b.OutputValue), $"light{1}_bloom_green", $"light{1}_bloom_red");
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(0, nw1b.OutputValue), $"light{0}_bloom_green", $"light{0}_bloom_red");
                //in1 4-7
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(3, nw1.OutputValue), $"light{7}_bloom_green", $"light{7}_bloom_red");
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(2, nw1.OutputValue), $"light{6}_bloom_green", $"light{6}_bloom_red");
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(1, nw1.OutputValue), $"light{5}_bloom_green", $"light{5}_bloom_red");
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(0, nw1.OutputValue), $"light{4}_bloom_green", $"light{4}_bloom_red");

                //in2b 8-11
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(3, nw2b.OutputValue), $"light{11}_bloom_green", $"light{11}_bloom_red");
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(2, nw2b.OutputValue), $"light{10}_bloom_green", $"light{10}_bloom_red");
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(1, nw2b.OutputValue), $"light{9}_bloom_green", $"light{9}_bloom_red");
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(0, nw2b.OutputValue), $"light{8}_bloom_green", $"light{8}_bloom_red");
                //in2 12-15
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(3, nw2.OutputValue), $"light{15}_bloom_green", $"light{15}_bloom_red");
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(2, nw2.OutputValue), $"light{14}_bloom_green", $"light{14}_bloom_red");
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(1, nw2.OutputValue), $"light{13}_bloom_green", $"light{13}_bloom_red");
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(0, nw2.OutputValue), $"light{12}_bloom_green", $"light{12}_bloom_red");

            }
            catch (Exception ex)
            {

            }
        }
        protected void ShowSymbolConditionally(
            object nw,
          Func<bool> active,
          KAnimHashedString ifTrue,
          KAnimHashedString ifFalse)
        {
            var connected = nw != null;
            kbac.SetSymbolVisiblity(ifTrue, connected && active());
            kbac.SetSymbolVisiblity(ifFalse, connected && !active());
        }
    }
}
