using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;

namespace EdgeDetectors
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class RisingEdgeGate : UnaryGate
    {
        public override void LogicTick()
        {
            if (GetInputNetwork() == null || GetOutputNetwork() == null)
            {
                kbac.Play((HashedString)"off");
                return;
            }

            LastInput = CurrentInput;
            CurrentInput = GetInputValue();
            CurrentOutput = CurrentInput == 1 && LastInput == 0 ? 1 : 0;
            if (CurrentOutput != LastOutput)
            {
                this.GetComponent<LogicPorts>().SendSignal(RisingEdgeGateConfig.OUTPUT_PORT_ID, CurrentOutput);
                LastOutput = CurrentOutput;
            }
            UpdateVisuals(CurrentInput, CurrentOutput);
        }
    }
}
