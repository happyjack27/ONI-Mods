using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPlus
{
    public class DELAYGATE
    {
        public static LocString NAME = (LocString)STRINGS.UI.FormatAsLink("Signal Delay", nameof(DELAYGATE));
        public static LocString DESC = (LocString)"This gate introduces latency and propagates the input signal at a later time based on the selected delay.";
        public static LocString EFFECT = (LocString)($"Delays the input signal based on the selected delay time.  Different than a buffer gate since it has an internal memory that will propagate all signals be they {UI.FormatAsAutomationState("green", UI.AutomationState.Active)} or {UI.FormatAsAutomationState("red", UI.AutomationState.Standby)}");

        public static string LOGIC_PORT = $"Input Port";
        public static string INPUT_PORT_ACTIVE = $"{STRINGS.UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}: Repeats a {STRINGS.UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} based on the specified delay";
        public static string INPUT_PORT_INACTIVE = $"{STRINGS.UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)}: Repeats a {STRINGS.UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} based on the specified delay";
        public static string LOGIC_PORT_OUTPUT = $"OUTPUT PORT";
        public static string OUTPUT_PORT_ACTIVE = $"{STRINGS.UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}: if the input was previously {STRINGS.UI.FormatAsAutomationState("active", UI.AutomationState.Active)} based on the delay";
        public static string OUTPUT_PORT_INACTIVE = $"{STRINGS.UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)}: if the input was previously {STRINGS.UI.FormatAsAutomationState("inactive", UI.AutomationState.Standby)} based on the delay";

        public class DELAYGATE_SIDESCREEN
        {
            public static LocString TITLE = (LocString)"Signal delay time";
            public static LocString TOOLTIP = (LocString)($"Will delay the current {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} or {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} for <b>{0} seconds</b>");
        }
    }
}
