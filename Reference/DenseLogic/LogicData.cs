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
using STRINGS;
using System;

namespace ONI_DenseLogic {
	[SerializationConfig(MemberSerialization.OptIn)]
	public class LogicData : KMonoBehaviour {
		public static readonly HashedString READID = new HashedString("LogicData_READ");
		public static readonly HashedString DATAID = new HashedString("LogicData_DATA");
		public static readonly HashedString SETID = new HashedString("LogicData_SET");

		public static readonly CellOffset READOFFSET = new CellOffset(0, 1);
		public static readonly CellOffset DATAOFFSET = new CellOffset(1, 0);
		public static readonly CellOffset SETOFFSET = new CellOffset(0, 0);

		private static readonly EventSystem.IntraObjectHandler<LogicData>
			OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicData>(
			(component, data) => component.OnLogicValueChanged(data));

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
		[MyCmpReq]
		private KBatchedAnimController kbac;
		[MyCmpGet]
		private LogicPorts ports;
#pragma warning restore IDE0044, CS0649

		[Serialize]
		private int value;
		private static StatusItem infoStatusItem;

		protected override void OnSpawn() {
			if (infoStatusItem == null) {
				infoStatusItem = new StatusItem("StoredValue", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
				infoStatusItem.resolveStringCallback = new Func<string, object, string>(ResolveInfoStatusItemString);
			}
			Subscribe(-801688580, OnLogicValueChangedDelegate);
		}

		public void OnLogicValueChanged(object data) {
			if (ports == null || gameObject == null || this == null || ((LogicValueChanged)data).portID == READID)
				return;
			int dataValue = ports.GetInputValue(DATAID);
			int setValue = ports.GetInputValue(SETID);
			int outValue = value;
			if (LogicCircuitNetwork.IsBitActive(0, setValue))
				outValue = dataValue;
			if (outValue != value) {
				value = outValue;
				ports.SendSignal(READID, value);
				kbac.Play(LogicCircuitNetwork.IsBitActive(0, value) ? "on" : "off", KAnim.PlayMode.Once, 1f, 0.0f);
			}
		}

		private static string ResolveInfoStatusItemString(string format_str, object data) {
			int outputValue = ((LogicData)data).ports.GetOutputValue(READID);
			return string.Format(BUILDINGS.PREFABS.LOGICMEMORY.STATUS_ITEM_VALUE, outputValue);
		}
	}
}
