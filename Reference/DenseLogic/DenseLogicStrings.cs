/*
 * Copyright 2020 Dense Logic Team
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using STRINGS;

using ONI_UI = STRINGS.UI;
using AutomationState = STRINGS.UI.AutomationState;

namespace ONI_DenseLogic {
	/// <summary>
	/// Strings used in Dense Logic.
	/// </summary>
	public static class DenseLogicStrings {
		public static class BUILDINGS {
			// These will be resolved in the same order anyways so save some space
			private static readonly string GREEN = ONI_UI.FormatAsAutomationState("Green", AutomationState.Active);
			private static readonly string RED = ONI_UI.FormatAsAutomationState("Red", AutomationState.Standby);
			private static readonly string GREEN_SIGNAL = ONI_UI.FormatAsAutomationState("Green Signal", AutomationState.Active);
			private static readonly string RED_SIGNAL = ONI_UI.FormatAsAutomationState("Red Signal", AutomationState.Standby);
			private static readonly string AUTOMATION = ONI_UI.FormatAsLink("Automation", "LOGIC");

			// Already has a link
			private static readonly string RIBBON_CABLE = STRINGS.BUILDINGS.PREFABS.LOGICRIBBON.NAME;

			public static class PREFABS {
				public static class DENSELOGICTEAM_DENSEGATE {
					public static LocString NAME = ONI_UI.FormatAsLink("Dense Multigate",
						DenseLogicGateConfig.ID);
					public static LocString DESC = $"Banhi told us she didn't like the old logic gates. Banhi is weird.";
					public static LocString EFFECT = $"Switches between different logic functions easily in the UI.\n\nPerforms logic on each bit in {RIBBON_CABLE} based on the specified mode.\n\nAND:\nOutputs {GREEN} when both Input A <b>AND</b> Input B are receiving {GREEN}.\n\nOR:\nOutputs {GREEN} when either Input A <b>OR</b> Input B are receiving {GREEN}.\n\nXOR:\nOutputs {GREEN} when <b>EXACTLY ONE</b> of Input A and Input B are receiving {GREEN}.\n\nOutputs {RED_SIGNAL}s if none of the above are true.";
					public static LocString INPUT_LOGIC_PORT_ONE = "4-Bit Input A";
					public static LocString INPUT_LOGIC_PORT_TWO = "4-Bit Input B";
					public static LocString OUTPUT_LOGIC_PORT = "4-Bit Logic Output";
					public static LocString PORTIN_ACTIVE = $"Reads {GREEN} signals from the {RIBBON_CABLE}.";
					public static LocString PORTIN_INACTIVE = $"Reads {RED} signals from the {RIBBON_CABLE}.";
					public static LocString PORTOUT_ACTIVE = $"Writes {GREEN} signals to the {RIBBON_CABLE}.";
					public static LocString PORTOUT_INACTIVE = $"Writes {RED} signals to the {RIBBON_CABLE}.";
				}

				public static class DENSELOGICTEAM_LOGICGATE {
					public static LocString INPUT_LOGIC_PORT_ONE = "Input A";
					public static LocString INPUT_LOGIC_PORT_TWO = "Input B";
					public static LocString OUTPUT_LOGIC_PORT = "Logic Output";
					public static LocString PORTIN_ACTIVE = $"Reads a {GREEN} signal from the {AUTOMATION} wire.";
					public static LocString PORTIN_INACTIVE = $"Reads a {RED} signal from the {AUTOMATION} wire.";
					public static LocString PORTOUT_ACTIVE = $"Writes a {GREEN} signal to the {AUTOMATION} wire.";
					public static LocString PORTOUT_INACTIVE = $"Writes a {RED} signal to the {AUTOMATION} wire.";
				}

				public static class DENSELOGICTEAM_INLINEGATE {
					public static LocString NAME = ONI_UI.FormatAsLink("Inline Logic Gate",
						InlineLogicGateConfig.ID);
					public static LocString DESC = $"Just when we thought that Moore's law could go no further, Banhi found a way to make logic gates smaller than ever.";
					public static LocString EFFECT = $"Performs logic on the selected bits in {RIBBON_CABLE} and outputs the result to another selected bit.\n\nAND:\nOutputs {GREEN} when both Input A <b>AND</b> Input B are receiving {GREEN}.\n\nOR:\nOutputs {GREEN} when either Input A <b>OR</b> Input B are receiving {GREEN}.\n\nXOR:\nOutputs {GREEN} when <b>EXACTLY ONE</b> of Input A and Input B are receiving {GREEN}.\n\nOutputs {RED} if none of the above are true.";
					public static LocString PORT_ACTIVE = $"Reads and writes {GREEN} signals on {RIBBON_CABLE} bits.";
					public static LocString PORT_INACTIVE = $"Reads and writes {RED} signals on {RIBBON_CABLE} bits.";

					public static LocString LOGIC_PORT_IO = "Bit Reading and Writing";
				}
				
				public static class DENSELOGICTEAM_LOGICDATA {
					public static LocString NAME = ONI_UI.FormatAsLink("Data Latch",
						LogicDataConfig.ID);
					public static LocString DESC = $"A Memory stores an {AUTOMATION} signal received in the Data Port (D) when the Set Port (S) receives a {GREEN_SIGNAL}.";
					public static LocString EFFECT = $"Contains an internal Memory, and will output whatever signal is stored in that Memory. Signals sent to the Inputs <i>only</i> affect the Memory, and do not pass through to the Output.\n\nSending a {GREEN_SIGNAL} to the Set Port (S) will set the memory to the value of the Data Port (D).";
					public static LocString DATA_PORT = "DATA PORT (D)";
					public static LocString DATA_PORT_ACTIVE = $"{GREEN_SIGNAL}: The internal Memory will be set to {GREEN} if the Set Port (S) receives a {GREEN_SIGNAL}";
					public static LocString DATA_PORT_INACTIVE = $"{RED_SIGNAL}: The internal Memory will be set to {RED} if the Set Port (S) receives a {GREEN_SIGNAL}";
					public static LocString SET_PORT = "SET PORT (S)";
					public static LocString SET_PORT_ACTIVE = $"{GREEN_SIGNAL}: Set the internal Memory to the {AUTOMATION} signal received in the Data Port (D)";
					public static LocString SET_PORT_INACTIVE = $"{RED_SIGNAL}: The {AUTOMATION} signal read from this Memory will not change";
				}

				public static class DENSELOGICTEAM_LOGICSEVENSEGMENT {
					public static LocString NAME = ONI_UI.FormatAsLink("Seven Segment Display",
						LogicSevenSegmentConfig.ID);
					public static LocString DESC = $"Giving the duplicants a way to show off their favorite numbers has never been easier.";
					public static LocString EFFECT = $"Displays the value of a {RIBBON_CABLE} as a number 0 through 9.";
					public static LocString LOGIC_PORT = "Numerical Input";
					public static LocString INPUT_PORT_ACTIVE = $"{GREEN_SIGNAL}: Displays a non-zero number.";
					public static LocString INPUT_PORT_INACTIVE = $"{RED_SIGNAL}: Displays zero.";
					public static LocString OUTPUT_LOGIC_PORT = "Overflow";
					public static LocString OUTPUT_PORT_ACTIVE = $"Outputs a {GREEN_SIGNAL} if the input is larger than 9.";
					public static LocString OUTPUT_PORT_INACTIVE = $"Outputs a {RED_SIGNAL} if the input is less than or equal to 9.";
				}

				public static class DENSELOGICTEAM_LOGICNOR {
					public static LocString NAME = ONI_UI.FormatAsLink("NOR Gate",
						LogicGateNorConfig.ID);
					public static LocString DESC = $"This gate outputs a {RED_SIGNAL} if receiving one or more {GREEN_SIGNAL}s.";
					public static LocString EFFECT = $"Outputs a {RED_SIGNAL} if at least one of Input A <b>OR</b> Input B is receiving a {GREEN}.\n\nOutputs a {GREEN_SIGNAL} when neither Input A or Input B are receiving {GREEN}.";
				}

				public static class DENSELOGICTEAM_LOGICNAND {
					public static LocString NAME = ONI_UI.FormatAsLink("NAND Gate",
						LogicGateNorConfig.ID);
					public static LocString DESC = $"This gate outputs a {RED_SIGNAL} if receiving two {GREEN_SIGNAL}s.";
					public static LocString EFFECT = $"Outputs a {RED_SIGNAL} if both Input A <b>AND</b> Input B are receiving {GREEN}.\n\nOutputs a {GREEN_SIGNAL} when even one Input is receiving {GREEN}.";
				}

				public static class DENSELOGICTEAM_LOGICXNOR {
					public static LocString NAME = ONI_UI.FormatAsLink("XNOR Gate",
						LogicGateNorConfig.ID);
					public static LocString DESC = $"This gate outputs a {RED_SIGNAL} if exactly one of its Inputs is receiving a {GREEN_SIGNAL}.";
					public static LocString EFFECT = $"Outputs a {RED_SIGNAL} if exactly one of its Inputs is receiving {GREEN}.\n\nOutputs a {GREEN_SIGNAL} if both or neither Inputs are receiving {GREEN}.";
				}

				public static class DENSELOGICTEAM_GENERALMULTIPLEXER {
					public static LocString INPUT_LOGIC_PORT = "1-Bit Input";
					public static LocString INPUT_LOGIC_PORT_RIBBON = "4-Bit Input";
					public static LocString CONTROL_LOGIC_PORT_ONE = "Control Input A";
					public static LocString CONTROL_LOGIC_PORT_TWO = "Control Input B";
					public static LocString OUTPUT_LOGIC_PORT = "1-Bit Selection";
					public static LocString OUTPUT_LOGIC_PORT_RIBBON = "4-Bit Selection";
					public static LocString INPUT_PORT_ACTIVE = $"{GREEN_SIGNAL}: Passes a {GREEN_SIGNAL} to the selected bit of the output.";
					public static LocString INPUT_PORT_INACTIVE = $"{RED_SIGNAL}: Passes a {RED_SIGNAL} to the selected bit of the output.";
					public static LocString INPUT_PORT_ACTIVE_RIBBON = $"{GREEN_SIGNAL}: Provides {GREEN} signals as selectable bits.";
					public static LocString INPUT_PORT_INACTIVE_RIBBON = $"{RED_SIGNAL}: Provides {RED} signals as selectable bits.";
					public static LocString OUTPUT_PORT_ACTIVE = $"{GREEN_SIGNAL}: The bit is selected and is {GREEN}.";
					public static LocString OUTPUT_PORT_INACTIVE = $"{RED_SIGNAL}: The bit is not selected bit or is {RED}.";
					public static LocString CONTROL_PORT_ACTIVE = $"{GREEN_SIGNAL}: Selects which bit to pass through/onto.";
					public static LocString CONTROL_PORT_INACTIVE = $"{RED_SIGNAL}: Selects which bit to pass through/onto.";
				}

				public static class DENSELOGICTEAM_DENSEMULTIPLEXER {
					public static LocString NAME = ONI_UI.FormatAsLink("Dense Multiplexer",
						DenseMultiplexerConfig.ID);
					public static LocString DESC = $"A 4-Bit multiplexer with {RIBBON_CABLE} as input.";
					public static LocString EFFECT = $"Controls which bit of the input {RIBBON_CABLE} is sent to the output using two control inputs.";
				}

				public static class DENSELOGICTEAM_DENSEDEMULTIPLEXER {
					public static LocString NAME = ONI_UI.FormatAsLink("Dense DeMultiplexer",
						DenseDeMultiplexerConfig.ID);
					public static LocString DESC = $"A 4-Bit multiplexer with {RIBBON_CABLE} as output.";
					public static LocString EFFECT = $"Controls which bit of the output {RIBBON_CABLE} the input is sent to using two control inputs.";
				}

				public static class DENSELOGICTEAM_DENSEINPUT {
					public static LocString NAME = ONI_UI.FormatAsLink("Signal Multiswitch",
						DenseInputConfig.ID);
					public static LocString DESC = $"A 4-Bit constant input with {RIBBON_CABLE} as output.";
					public static LocString EFFECT = $"Sends a configurable 4-Bit signal on an {AUTOMATION} grid.\n\n{RIBBON_CABLE} must be used as the output wire to avoid overloading.";
					public static LocString LOGIC_PORT_OUTPUT = $"Multisignal Toggle";
					public static LocString PORTOUT_ACTIVE = $"Writes {GREEN} signals to some bits of the {RIBBON_CABLE}.";
					public static LocString PORTOUT_INACTIVE = $"Writes {RED} signals to some bits of the {RIBBON_CABLE}.";
				}

				public static class DENSELOGICTEAM_SIGNALREMAPPER {
					public static LocString NAME = ONI_UI.FormatAsLink("Ribbon Rearranger",
						SignalRemapperConfig.ID);
					public static LocString DESC = $"Repeats the signals on its {RIBBON_CABLE} input to its {RIBBON_CABLE} output in a different order.";
					public static LocString EFFECT = $"Rearranges the order of signals from one {AUTOMATION} grid to another.\n\n{RIBBON_CABLE} must be used as both the input and output wire to avoid overloading.";
					public static LocString PORTIN_ACTIVE = $"Reads {GREEN} signals from {RIBBON_CABLE}s.";
					public static LocString PORTIN_INACTIVE = $"Reads {RED} signals from {RIBBON_CABLE}s.";
					public static LocString PORTOUT_ACTIVE = $"Writes {GREEN} signals to each bit of the output {RIBBON_CABLE} when the configured input {RIBBON_CABLE} bit is sending {GREEN}.";
					public static LocString PORTOUT_INACTIVE = $"Writes {RED} signals to each bit of the output {RIBBON_CABLE} when the configured input {RIBBON_CABLE} bit is sending {RED}.";
				}
			}
		}

		public static class UI {
			public static class TOOLTIPS {
				public static class FOURBITSELECT {
					public static LocString ENABLE_ALL = "Enables all bits";
					public static LocString DISABLE_ALL = "Disables all bits";
				}

				public static class INLINELOGIC {
					public static LocString INPUT_ONE = "Select input bit to use for Logic Input 1";
					public static LocString INPUT_TWO = "Select input bit to use for Logic Input 2";
					public static LocString OUTPUT = "Select bit to output logic result";
				}

				public static class SIGNALREMAPPER {
					public static LocString BIT_1 = "Select input bit to use on Output Bit 1";
					public static LocString BIT_2 = "Select input bit to use on Output Bit 2";
					public static LocString BIT_3 = "Select input bit to use on Output Bit 3";
					public static LocString BIT_4 = "Select input bit to use on Output Bit 4";
					public static LocString IDENTITY = "Sets all bits to pass through as normal";
					public static LocString CLEAR = "Stop all bits from passing through";
				}
			}

			public static class UISIDESCREENS {
				public static class FOURBITSELECT {
					public static LocString ENABLE_ALL = "Set";
					public static LocString DISABLE_ALL = "Clear";
					public static LocString TITLE = "Set Provided Signal";
				}

				public static class GATESELECT {
					public static LocString TITLE = "Select Logic Function";
				}

				public static class INLINELOGIC {
					public static LocString INPUT_ONE = "Input 1";
					public static LocString INPUT_TWO = "Input 2";
					public static LocString INVALID_BITS = "Output bit must be different!";
					public static LocString OUTPUT = "Output";
					public static LocString TITLE = "Configure Inline Logic Gate";
				}

				public static class SIGNALREMAPPER {
					public static LocString BIT_NONE = "None";
					public static LocString BIT_1 = "Bit 1";
					public static LocString BIT_2 = "Bit 2";
					public static LocString BIT_3 = "Bit 3";
					public static LocString BIT_4 = "Bit 4";
					public static LocString OUTPUT = "Output {0}";
					public static LocString TITLE = "Configure Signal Mapping";
					public static LocString IDENTITY = "Set Default";
					public static LocString CLEAR = "Clear Mapping";
				}
			}
		}
	}
}
