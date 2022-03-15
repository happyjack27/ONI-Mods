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
	public sealed class DenseInput : KMonoBehaviour, ISaveLoadable, IConfigurableFourBits {
		public static readonly HashedString OUTPUTID = new HashedString("DenseInput_OUT");

		private static readonly EventSystem.IntraObjectHandler<DenseInput>
			OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DenseInput>(
			(component, data) => component.OnCopySettings(data));

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
		[MyCmpReq]
		private KBatchedAnimController kbac;

		[MyCmpReq]
		private LogicPorts ports;
#pragma warning restore IDE0044, CS0649

		[SerializeField]
		[Serialize]
		private int curOut;

		internal DenseInput() {
			curOut = 0;
		}

		protected override void OnSpawn() {
			base.OnSpawn();
			gameObject.AddOrGet<CopyBuildingSettings>();
			Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
			UpdateLogicCircuit();
		}

		private void OnCopySettings(object data) {
			DenseInput component = ((GameObject)data).GetComponent<DenseInput>();
			if (component == null) return;
			curOut = component.curOut;
			UpdateLogicCircuit();
		}

		private void UpdateLogicCircuit() {
			ports.SendSignal(OUTPUTID, curOut);
			UpdateVisuals();
		}

		public void UpdateVisuals() {
			int num0 = (curOut & (0x1 << 0)) > 0 ? 1 : 0;
			int num1 = (curOut & (0x1 << 1)) > 0 ? 1 : 0;
			int num2 = (curOut & (0x1 << 2)) > 0 ? 1 : 0;
			int num3 = (curOut & (0x1 << 3)) > 0 ? 1 : 0;
			int state = num3 + 2 * num2 + 4 * num1 + 8 * num0;
			kbac.Play("on_" + state, KAnim.PlayMode.Once, 1f, 0.0f);
		}

		public void SetBit(bool value, int pos) {
			curOut &= ~(1 << pos);
			if (value) {
				curOut |= 1 << pos;
			}
			UpdateLogicCircuit();
		}

		public bool GetBit(int pos) {
			return (curOut & (1 << pos)) > 0;
		}
	}
}
