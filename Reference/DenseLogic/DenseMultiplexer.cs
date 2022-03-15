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

namespace ONI_DenseLogic {
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DenseMultiplexer : KMonoBehaviour {
		public static readonly HashedString INPUTID = new HashedString("DenseGate_IN");
		public static readonly HashedString OUTPUTID = new HashedString("DenseGate_OUT");
		public static readonly HashedString CONTROLID1 = new HashedString("DenseMuxGate_CTRL1");
		public static readonly HashedString CONTROLID2 = new HashedString("DenseMuxGate_CTRL2");

		public static readonly CellOffset INPUTOFFSET = new CellOffset(0, 1);
		public static readonly CellOffset OUTPUTOFFSET = new CellOffset(1, 1);
		public static readonly CellOffset CONTROLOFFSET1 = new CellOffset(0, 0);
		public static readonly CellOffset CONTROLOFFSET2 = new CellOffset(1, 0);

		private static readonly EventSystem.IntraObjectHandler<DenseMultiplexer>
			OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<DenseMultiplexer>(
			(component, data) => component.OnLogicValueChanged(data));

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
		private int curOut;
		[Serialize]
		private int ctrlVal1, ctrlVal2;

		[Serialize]
		public MultiplexerType muxType;

		private int GetActualCell(CellOffset offset) {
			if (rotatable != null)
				offset = rotatable.GetRotatedCellOffset(offset);
			return Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), offset);
		}

		protected override void OnSpawn() {
			base.OnSpawn();
			Subscribe((int)GameHashes.LogicEvent, OnLogicValueChangedDelegate);
			UpdateVisuals();
		}

		public void OnLogicValueChanged(object data) {
			var logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == INPUTID)
				inVal = logicValueChanged.newValue;
			else if (logicValueChanged.portID == CONTROLID1)
				ctrlVal1 = logicValueChanged.newValue;
			else if (logicValueChanged.portID == CONTROLID2)
				ctrlVal2 = logicValueChanged.newValue;
			else
				return;
			UpdateLogicCircuit();
		}

		private int GetBitValue(int pos) {
			return inVal & 1 << pos;
		}

		private int SetBitValue(int pos, bool on) {
			return on ? 1 << pos : 0;
		}

		private void UpdateLogicCircuit() {
			if (muxType == MultiplexerType.MUX) {
				curOut = GetBitValue(ctrlVal1 + 2 * ctrlVal2) > 0 ? 1 : 0;
			} else if (muxType == MultiplexerType.DEMUX) {
				curOut = SetBitValue(ctrlVal1 + 2 * ctrlVal2, inVal > 0);
			} else {
				// should never occur
				PUtil.LogWarning("Unknown multiplexer type " + muxType);
				curOut = 0;
			}
			ports.SendSignal(OUTPUTID, curOut);
			UpdateVisuals();
		}

		private int GetRibbonValue(int wire) {
			if (wire == 0) {
				return 0;
			} else if (wire == 0b1111) {
				return 2;
			} else {
				return 1;
			}
		}

		private int GetSingleValue(int wire) {
			return wire & 0b1;
		}

		public void UpdateVisuals() {
			// when there is not an output, we are supposed to play the off animation
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(GetActualCell(OUTPUTOFFSET)) is LogicCircuitNetwork)) {
				kbac.Play("off", KAnim.PlayMode.Once, 1f, 0.0f);
			} else {
				int bit0 = 0, bit1 = 0, bit2 = 0, bit3 = 0;
				if (muxType == MultiplexerType.MUX) {
					bit0 = GetRibbonValue(inVal);
					bit1 = GetSingleValue(ctrlVal1);
					bit2 = GetSingleValue(ctrlVal2);
					bit3 = GetSingleValue(curOut);
				} else if (muxType == MultiplexerType.DEMUX) {
					bit0 = GetRibbonValue(curOut);
					bit1 = GetSingleValue(ctrlVal1);
					bit2 = GetSingleValue(ctrlVal2);
					bit3 = GetSingleValue(inVal);
				} else {
					// should never occur
					PUtil.LogWarning("Unknown multiplexer type " + muxType);
				}
				kbac.Play("on_" + (bit0 + 3 * bit1 + 6 * bit2 + 12 * bit3), KAnim.PlayMode.
					Once, 1f, 0.0f);
			}
		}

		public enum MultiplexerType {
			MUX, DEMUX
		}
	}
}
