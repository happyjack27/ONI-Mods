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
    class Reg4 : baseClasses.Base2x2
    {
        public override void LogicTick()
        {
            if (GetInputNetwork() == null || GetOutputNetwork() == null)
            {
                kbac.Play((HashedString)"off");
                return;
            }

            var LastOutput = OutputValue;
            InputValue = this.GetComponent<LogicPorts>()?.GetInputValue(Reg4Config.INPUT_PORT_ID) ?? 0;
            ControlPort1Value = this.GetComponent<LogicPorts>()?.GetInputValue(Reg4Config.CONTROL_PORT_ID1) ?? 1;
            ControlPort2Value = this.GetComponent<LogicPorts>()?.GetInputValue(Reg4Config.CONTROL_PORT_ID2) ?? 1;
            if (null == Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Reg4Config.CONTROL_PORT_ID1)))
            {
                ControlPort1Value = 1;
            }
            if (null == Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Reg4Config.CONTROL_PORT_ID2)))
            {
                ControlPort2Value = 1;
            }

            if ( ControlPort1Value > 0)
            {
                StoredValue = InputValue;
            }
            OutputValue = ControlPort2Value > 0 ? StoredValue : 0;

            if (OutputValue != LastOutput)
            {
                this.GetComponent<LogicPorts>().SendSignal(Reg4Config.OUTPUT_PORT_ID, OutputValue);
            }
            UpdateVisuals();
        }
    }
}
