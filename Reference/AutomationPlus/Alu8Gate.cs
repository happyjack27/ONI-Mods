using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPlus
{
    class Alu8Gate : AluGate
    {
        public static readonly HashedString INPUT_PORT_ID1B = new HashedString("AluGateInput1B");
        public static readonly HashedString INPUT_PORT_ID2B = new HashedString("AluGateInput2B");
        public static readonly HashedString OUTPUT_PORT_IDB = new HashedString("AluGateOutputB");

        public Alu8Gate() : base()
        {
            this.bits = 8;
            this.maxValue = 0xff;
        }

        public override int GetInputValue1()
        {
            var val1 = base.GetInputValue1();
            var val2 = this.ports?.GetInputValue(Alu8Gate.INPUT_PORT_ID1B) ?? 0;

            return (val1 << 4) | val2;

        }

        public override int GetInputValue2()
        {
            var val1 = base.GetInputValue2();
            var val2 = this.ports?.GetInputValue(Alu8Gate.INPUT_PORT_ID2B) ?? 0;

            return (val1 << 4) | val2;

        }

        protected override bool IsOnAnimation()
        {
            var nw1b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.INPUT_PORT_ID1B));
            var nw2b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.INPUT_PORT_ID2B));
            var nwOutb = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.OUTPUT_PORT_IDB));
            return base.IsOnAnimation() && nw1b != null && nw2b != null && nwOutb != null; 
        }

        protected override bool ShouldRecalcValue(LogicValueChanged logicValueChanged)
        {
            return base.ShouldRecalcValue(logicValueChanged)
                && logicValueChanged.portID != Alu8Gate.OUTPUT_PORT_IDB;
        }

        protected override void UpdateValue()
        {
            var val1 = this.currentValue >> 4;
            var val2 = this.currentValue & 0xf;
            this.GetComponent<LogicPorts>().SendSignal(AluGate.OUTPUT_PORT_ID, val1);
            this.GetComponent<LogicPorts>().SendSignal(Alu8Gate.OUTPUT_PORT_IDB, val2);
        }

        protected override bool HasPort(HashedString portId)
        {
            return base.HasPort(portId)
                || portId == Alu8Gate.INPUT_PORT_ID1B
                || portId == Alu8Gate.INPUT_PORT_ID2B
                || portId == Alu8Gate.OUTPUT_PORT_IDB;
        }

        protected override void ToggleBlooms()
        {
            var nw1 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.INPUT_PORT_ID1));
            var nw1b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.INPUT_PORT_ID1B));
            var nw2 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.INPUT_PORT_ID2));
            var nw2b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.INPUT_PORT_ID2B));
            var nwOp = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.OP_PORT_ID));
            var nwOut = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.OUTPUT_PORT_ID));
            var nwOutb = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Alu8Gate.OUTPUT_PORT_IDB));
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
    }
}
