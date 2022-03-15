/*
 * Copyright 2020 Dense Logic Team
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

namespace ONI_DenseLogic {
	[SerializationConfig(MemberSerialization.OptIn)]
	public class LogicGate : KMonoBehaviour {
		internal static int GetInputBitMask(int cell) {
			var wire = Grid.Objects[cell, (int)ObjectLayer.LogicWire].GetComponentSafe<LogicWire>();
			if (wire != null) {
				switch (wire.MaxBitDepth) {
					case LogicWire.BitDepth.OneBit: return 0b1;
					case LogicWire.BitDepth.FourBit: return 0b1111;
					default: return 0x7FFFFFFF;
				}
			}
			return 0;
		}

		internal static int MaskOutputValue(int cellOne, int cellTwo, int cellOut, int output) {
			return output & Math.Min(GetInputBitMask(cellOut), Math.Max(GetInputBitMask(cellOne), GetInputBitMask(cellTwo)));
		}

		public static readonly HashedString INPUTID1 = new HashedString("LogicGate_IN1");
		public static readonly HashedString INPUTID2 = new HashedString("LogicGate_IN2");
		public static readonly HashedString OUTPUTID = new HashedString("LogicGate_OUT");

		public static readonly CellOffset INPUTOFFSET1 = new CellOffset(0, 0);
		public static readonly CellOffset INPUTOFFSET2 = new CellOffset(0, 1);
		public static readonly CellOffset OUTPUTOFFSET = new CellOffset(1, 0);

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
		[MyCmpReq]
		private KBatchedAnimController kbac;

		[MyCmpReq]
		private LogicPorts ports;

		[MyCmpGet]
		private Rotatable rotatable;
#pragma warning restore IDE0044, CS0649

		[Serialize]
		private int inVal1, inVal2;
		private int curOut;

		[Serialize]
		// [SerializeField] is not required on public fields with supported types
		public LogicGateType gateType;

		private int GetActualCell(CellOffset offset) {
			if (rotatable != null)
				offset = rotatable.GetRotatedCellOffset(offset);
			return Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), offset);
		}

		private int GetSingleValue(int wire) {
			return wire & 0b1;
		}

		protected override void OnCleanUp() {
			Unsubscribe((int)GameHashes.LogicEvent, OnLogicValueChanged);
			base.OnCleanUp();
		}

		protected override void OnSpawn() {
			base.OnSpawn();
			Subscribe((int)GameHashes.LogicEvent, OnLogicValueChanged);
		}

		public void OnLogicValueChanged(object data) {
			var logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == INPUTID1)
				inVal1 = logicValueChanged.newValue;
			else if (logicValueChanged.portID == INPUTID2)
				inVal2 = logicValueChanged.newValue;
			else
				return;
			UpdateLogicCircuit();
		}

		private void UpdateLogicCircuit() {
			if (gateType == LogicGateType.Or)
				curOut = inVal1 | inVal2;
			else if (gateType == LogicGateType.And)
				curOut = inVal1 & inVal2;
			else if (gateType == LogicGateType.Xor)
				curOut = inVal1 ^ inVal2;
			else if (gateType == LogicGateType.Nor)
				curOut = ~(inVal1 | inVal2);
			else if (gateType == LogicGateType.Nand)
				curOut = ~(inVal1 & inVal2);
			else if (gateType == LogicGateType.Xnor)
				curOut = ~(inVal1 ^ inVal2);
			else {
				// should never occur
				PUtil.LogWarning("Unknown LogicGate operand " + gateType);
				curOut = 0;
			}
			ports.SendSignal(OUTPUTID, MaskOutputValue(GetActualCell(INPUTOFFSET1),
				GetActualCell(INPUTOFFSET2), GetActualCell(OUTPUTOFFSET), curOut));
			UpdateVisuals();
		}

		public void UpdateVisuals() {
			// when there is not an output, we are supposed to play the off animation
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(GetActualCell(OUTPUTOFFSET)) is LogicCircuitNetwork)) {
				kbac.Play("off", KAnim.PlayMode.Once, 1f, 0.0f);
				return;
			}
			int num0 = GetSingleValue(inVal1);
			int num1 = GetSingleValue(inVal2);
			int num2 = GetSingleValue(curOut);
			int state = num0 + 2 * num1 + 4 * num2;
			kbac.Play("on_" + state, KAnim.PlayMode.Once, 1f, 0.0f);
		}
	}
}
