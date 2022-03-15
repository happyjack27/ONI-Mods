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

using KSerialization;
using PeterHan.PLib.Core;
using System;
using UnityEngine;

namespace ONI_DenseLogic {
	[SerializationConfig(MemberSerialization.OptIn)]
	public sealed class InlineLogicGate : KMonoBehaviour, ISaveLoadable, IInlineSelectable,
			IConfigurableLogicGate {
		public static readonly HashedString PORTID = new HashedString("InlineGate_IO");
		public static readonly CellOffset OFFSET = CellOffset.none;

		private static readonly EventSystem.IntraObjectHandler<InlineLogicGate>
			OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<InlineLogicGate>(
			(component, data) => component.OnCopySettings(data));

		private static readonly Color COLOR_ON = new Color(0.3411765f, 0.7254902f, 0.3686275f);
		private static readonly Color COLOR_OFF = new Color(0.9529412f, 0.2901961f, 0.2784314f);

		private static readonly KAnimHashedString[] IN_A = { "in_a1", "in_a2", "in_a3", "in_a4" };
		private static readonly KAnimHashedString[] IN_B = { "in_b1", "in_b2", "in_b3", "in_b4" };
		private static readonly KAnimHashedString[] OUT = { "out_1", "out_2", "out_3", "out_4" };

		private static readonly KAnimHashedString GATE_OR = "or";
		private static readonly KAnimHashedString GATE_AND = "and";
		private static readonly KAnimHashedString GATE_XOR = "xor";

		private static readonly KAnimHashedString GATE_XNOR = "xnor";
		private static readonly KAnimHashedString GATE_NAND = "nand";
		private static readonly KAnimHashedString GATE_NOR = "nor";

		public LogicGateType GateType {
			get {
				return mode;
			}
			set {
				mode = value;
				UpdateGateType();
				UpdateLogicCircuit();
			}
		}

		public int InputBit1 {
			get {
				return inputBit1;
			}
			set {
				inputBit1 = value.InRange(0, DenseLogicGate.NUM_BITS - 1);
				UpdateLogicCircuit();
			}
		}

		public int InputBit2 {
			get {
				return inputBit2;
			}
			set {
				inputBit2 = value.InRange(0, DenseLogicGate.NUM_BITS - 1);
				UpdateLogicCircuit();
			}
		}

		public int OutputBit {
			get {
				return outputBit;
			}
			set {
				outputBit = value.InRange(0, DenseLogicGate.NUM_BITS - 1);
				UpdateLogicCircuit();
			}
		}

		internal ILogicEventReceiver InputHandler { get; private set; }

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
		[MyCmpReq]
		private KBatchedAnimController kbac;

		[MyCmpReq]
		private LogicPorts ports;

		[MyCmpGet]
		private Rotatable rotatable;
#pragma warning restore IDE0044, CS0649

		[Serialize]
		private int inVal;

		[SerializeField]
		[Serialize]
		private LogicGateType mode;

		[SerializeField]
		[Serialize]
		private int inputBit1, inputBit2, outputBit;

		internal InlineLogicGate() {
			InputHandler = null;
			mode = LogicGateType.And;
			inputBit1 = 0;
			inputBit2 = 1;
			outputBit = 2;
		}

		private int GetActualCell(CellOffset offset) {
			if (rotatable != null)
				offset = rotatable.GetRotatedCellOffset(offset);
			return Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), offset);
		}

		private void OnCopySettings(object data) {
			var gate = (data as GameObject)?.GetComponent<InlineLogicGate>();
			if (gate != null) {
				mode = gate.mode;
				inputBit1 = gate.inputBit1;
				inputBit2 = gate.inputBit2;
				outputBit = gate.outputBit;
				UpdateGateType();
			}
		}

		protected override void OnSpawn() {
			base.OnSpawn();
			gameObject.AddOrGet<CopyBuildingSettings>();
			Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
			InputHandler = new LogicIOHandler(this);
			UpdateGateType();
			UpdateVisuals();
		}

		internal void OnLogicValueChanged(int newValue) {
			if (newValue != inVal) {
				inVal = newValue;
				UpdateLogicCircuit();
			}
		}

		private void UpdateGateType() {
			kbac.SetSymbolVisiblity(GATE_OR, mode == LogicGateType.Or);
			kbac.SetSymbolVisiblity(GATE_AND, mode == LogicGateType.And);
			kbac.SetSymbolVisiblity(GATE_XOR, mode == LogicGateType.Xor);
			kbac.SetSymbolVisiblity(GATE_NOR, mode == LogicGateType.Nor);
			kbac.SetSymbolVisiblity(GATE_NAND, mode == LogicGateType.Nand);
			kbac.SetSymbolVisiblity(GATE_XNOR, mode == LogicGateType.Xnor);
		}

		private void UpdateLogicCircuit() {
			int curOut = 0;
			if (inputBit1 != outputBit && inputBit2 != outputBit) {
				int inVal1 = inVal >> inputBit1, inVal2 = inVal >> inputBit2;
				if (mode == LogicGateType.Or)
					curOut = inVal1 | inVal2;
				else if (mode == LogicGateType.And)
					curOut = inVal1 & inVal2;
				else if (mode == LogicGateType.Xor)
					curOut = inVal1 ^ inVal2;
				else if (mode == LogicGateType.Nor)
					curOut = ~(inVal1 | inVal2);
				else if (mode == LogicGateType.Nand)
					curOut = ~(inVal1 & inVal2);
				else if (mode == LogicGateType.Xnor)
					curOut = ~(inVal1 ^ inVal2);
				else
					// should never occur
					PUtil.LogWarning("Unknown InlineLogicGate operand " + mode);
				curOut = (curOut & 0x1) << outputBit;
			}
			ports.SendSignal(PORTID, curOut);
			UpdateVisuals();
		}

		private int GetSingleValue(int wire) {
			return wire & 0b1;
		}

		private void SetSymbolVisibility(int pos, int wire, int bit) {
			int color;
			int singleValue = GetSingleValue(wire >> bit);
			if (singleValue == 0) {
				color = 1;
			} else {
				color = 0;
			}
			for (int i = 0; i < 3; i++) {
				kbac.SetSymbolVisiblity($"light_bloom_{pos}_{i}", false);
			}
			kbac.SetSymbolVisiblity($"light_bloom_{pos}_{color}", true);
		}

		private void SetSymbolsOff() {
			for (int pos = 0; pos < DenseLogicGate.NUM_BITS; pos++) {
				for (int i = 0; i < 3; i++) {
					kbac.SetSymbolVisiblity($"light_bloom_{pos}_{i}", false);
				}
				kbac.SetSymbolVisiblity($"light_bloom_{pos}_2", true);
			}
		}

		public void UpdateVisuals() {
			// when there is not an output, we are supposed to play the off animation
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(GetActualCell(OFFSET)) is LogicCircuitNetwork)) {
				SetSymbolsOff();
				for (int bit = 0; bit < DenseLogicGate.NUM_BITS; bit++) {
					kbac.SetSymbolVisiblity(IN_A[bit], false);
					kbac.SetSymbolVisiblity(IN_B[bit], false);
					kbac.SetSymbolVisiblity(OUT[bit], false);
				}
			} else {
				// otherwise set the colors of the lamps and of the individual wires on the gate
				for (int i = 0; i < DenseLogicGate.NUM_BITS; i++)
					SetSymbolVisibility(i, inVal, i);
				for (int bit = 0; bit < DenseLogicGate.NUM_BITS; bit++) {
					int mask = 1 << bit;
					kbac.SetSymbolTint(IN_A[bit], (inVal & mask) != 0 ? COLOR_ON : COLOR_OFF);
					kbac.SetSymbolTint(IN_B[bit], (inVal & mask) != 0 ? COLOR_ON : COLOR_OFF);
					kbac.SetSymbolTint(OUT[bit], (inVal & mask) != 0 ? COLOR_ON : COLOR_OFF);
					kbac.SetSymbolVisiblity(IN_A[bit], inputBit1 == bit);
					kbac.SetSymbolVisiblity(IN_B[bit], inputBit2 == bit);
					kbac.SetSymbolVisiblity(OUT[bit], outputBit == bit);
				}
			}
		}

		// Handles "input" logic events on an "output" type port.
		internal sealed class LogicIOHandler : ILogicEventReceiver {
			private readonly int cell;

			private readonly InlineLogicGate gate;

			public LogicIOHandler(InlineLogicGate gate) {
				this.gate = gate ?? throw new ArgumentNullException("gate");
				cell = gate.GetActualCell(OFFSET);
			}

			public int GetLogicCell() {
				return cell;
			}

			public void OnLogicNetworkConnectionChanged(bool connected) {
				// The gate does not display a "missing connection"
			}

			public void ReceiveLogicEvent(int value) {
				if (gate != null)
					gate.OnLogicValueChanged(value);
			}
		}
	}
}
