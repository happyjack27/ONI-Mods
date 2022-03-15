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
    class FallingEdgeGate : UnaryGate
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
            CurrentOutput = CurrentInput == 0 && LastInput == 1 ? 1 : 0;
            if (CurrentOutput != LastOutput)
            {
                this.GetComponent<LogicPorts>().SendSignal(FallingEdgeGateConfig.OUTPUT_PORT_ID, CurrentOutput);
                LastOutput = CurrentOutput;
            }
            UpdateVisuals(CurrentInput, CurrentOutput);
        }
    }
}
