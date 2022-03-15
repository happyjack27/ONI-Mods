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
    class EdgeCounter : WireToRibbon
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
            CurrentOutput += (CurrentInput != LastInput) ? 1 : 0;
            CurrentOutput &= 0x0F;
            if (CurrentOutput != LastOutput)
            {
                this.GetComponent<LogicPorts>().SendSignal(EdgeCounterConfig.OUTPUT_PORT_ID, CurrentOutput);
                LastOutput = CurrentOutput;
            }
            UpdateVisuals();
        }
    }
}
