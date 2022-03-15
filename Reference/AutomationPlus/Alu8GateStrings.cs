using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPlus
{
    class Alu8GateStrings
    {
        public static LocString NAME = STRINGS.UI.FormatAsLink("8-Bit Arithmetic Logical Unit", nameof(Alu8Gate));
        public static LocString DESC = AluGateStrings.DESC;
        public static LocString OUTPUT_NAME = "First 4 bits of OUTPUT";
        public static LocString OUTPUT_NAMEB = "Second 4 bits of OUTPUT";
        public static LocString OUTPUT_ACTIVE = AluGateStrings.OUTPUT_ACTIVE;
        public static LocString OUTPUT_INACTIVE = AluGateStrings.OUTPUT_INACTIVE;
        public static LocString EFFECT = "Performs 8-bit arithmetic operations on the input parameters";


        public static LocString INPUT_PORT1 = "First 4 bits of Lefthand Input";
        public static LocString INPUT_PORT1B = "Second 4 bits of Lefthand Input";
        public static LocString INPUT_PORT2 = "First 4 bits of Righthand Input";
        public static LocString INPUT_PORT2B = "Second 4 bits of Righthand Input";
        public static LocString INPUT_PORT_ACTIVE = AluGateStrings.INPUT_PORT_ACTIVE;
        public static LocString INPUT_PORT_INACTIVE = AluGateStrings.INPUT_PORT_INACTIVE;

        public static LocString OP_PORT_DESCRIPTION = AluGateStrings.OP_PORT_DESCRIPTION;
        public static LocString OP_CODE_ACTIVE = AluGateStrings.OP_CODE_ACTIVE;
        public static LocString OP_CODE_INACTIVE = AluGateStrings.OP_CODE_INACTIVE;
    }
}
