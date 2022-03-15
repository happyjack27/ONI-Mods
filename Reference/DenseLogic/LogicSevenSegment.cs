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
using UnityEngine;

namespace ONI_DenseLogic {
	[SerializationConfig(MemberSerialization.OptIn)]
	public class LogicSevenSegment : KMonoBehaviour, IRender200ms {
		public static readonly HashedString INPUTID = new HashedString("LogicSevenSegment_IN");
		public static readonly HashedString OUTPUTID = new HashedString("LogicSevenSegment_OUT");

		public static readonly CellOffset INPUTOFFSET = new CellOffset(0, 0);
		public static readonly CellOffset OUTPUTOFFSET = new CellOffset(0, 2);

		private static readonly EventSystem.IntraObjectHandler<LogicSevenSegment>
		OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicSevenSegment>(
		(component, data) => component.OnLogicValueChanged(data));

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
		[MyCmpReq]
		private KBatchedAnimController kbac;

		[MyCmpReq]
		private LogicPorts ports;
#pragma warning restore IDE0044, CS0649

		[Serialize]
		private int inVal;
		private int curOut;
		private MeterController meter;

		protected override void OnSpawn() {
			base.OnSpawn();
			Subscribe((int)GameHashes.LogicEvent, OnLogicValueChangedDelegate);
			meter = new MeterController(kbac, "meter_target", kbac.FlipY ? "meter_dn" :
				"meter_up", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront,
				Vector3.zero, null);
			UpdateMeter();
		}

		public void UpdateMeter() {
			meter.SetPositionPercent((inVal % 10) / 10f);
		}

		public void OnLogicValueChanged(object data) {
			var logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == INPUTID)
				inVal = logicValueChanged.newValue;
			else
				return;
			UpdateMeter();
			UpdateLogicCircuit();
		}

		private void UpdateLogicCircuit() {
			curOut = inVal > 9 ? 1 : 0;
			ports.SendSignal(OUTPUTID, curOut);
			UpdateVisuals();
		}

		public void Render200ms(float dt) {
			UpdateMeter();
			UpdateVisuals();
		}

		public void UpdateVisuals() {
			int num0 = inVal > 0 ? 1 : 0;
			kbac.Play("on_" + (num0 + 4 * curOut), KAnim.PlayMode.Once, 1f, 0.0f);
		}
	}
}
