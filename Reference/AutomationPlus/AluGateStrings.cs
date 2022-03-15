using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPlus
{
    class AluGateStrings
    {
        public static LocString NAME = (LocString)STRINGS.UI.FormatAsLink("4-Bit Arithmetic Logical Unit", nameof(AluGate));
        public static LocString DESC = (LocString)$@"The ALU can perform multiple operations on the inputs.
Operator Codes:   
add = 0x1 (0001)
subtract = 0x2 (0010)
multiply = 0x3 (0011)
modulus = 0x4 (0100)
exp = 0x5 (0101)
divide = 0x6 (0110)
logicalBitRight = 0x7 (0111)
logicalBitLeft = 0x8 (1000)
equal = 0x9 (1001)
notEqual = 0xA (1010)
lessThan = 0xB (1011)
greaterThan = 0xC (1100)
lessThanOrEqual = 0xD (1101)
greaterThanOrEqual = 0xE (1110)
plusPlus = 0xF (1111)

Signed numbers when enabled sets the ALU into using a twos complement binary number system. When enabled, the first bit when active {UI.FormatAsAutomationState("active", UI.AutomationState.Active)} signifies a negative number.";

        public static LocString EFFECT = ("Performs 4-bit arithmetic operations on the input parameters");
        public static LocString OUTPUT_NAME = "OUTPUT";
        public static LocString OUTPUT_ACTIVE = ($"Sends a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} on each bit based on the result of the operation" );
        public static LocString OUTPUT_INACTIVE = ($"Sends a {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} on each bit based on the result of the operation");

        public static LocString INPUT_PORT1 = "Lefthand value";
        public static LocString INPUT_PORT2 = "Righthand Value";
        public static LocString INPUT_PORT_ACTIVE = $"The input number based on binary format";
        public static LocString INPUT_PORT_INACTIVE = $"The input number based on binary format";

        public static LocString OP_PORT_DESCRIPTION = "OPERATOR PORT";
        public static LocString OP_CODE_ACTIVE = "See Description for op codes and meanings";
        public static LocString OP_CODE_INACTIVE = "See Description for op codes and meanings";

        public static LocString SIDESCREEN_TITLE = "Alu Configuration";

        public class SIDESCREEN
        {
            public static LocString TITLE = "Alu Configuration";
            public static LocString TOOLTIP = $"Will delay the current {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} or {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} for <b>{0} seconds</b>";
        }
    }
}
