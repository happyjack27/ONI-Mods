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

using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System;
using UnityEngine;

namespace ONI_DenseLogic {
	/// <summary>
	/// Controls the logic behind the side screen content for selecting the function of a
	/// configurable logic gate.
	/// </summary>
	internal sealed class LogicGateSelectSideScreen : SideScreenContent {
		/// <summary>
		/// The buttons in the UI.
		/// </summary>
		private readonly GameObject[] buttons;

		/// <summary>
		/// The selected logic gate.
		/// </summary>
		private IConfigurableLogicGate target;

		internal LogicGateSelectSideScreen() {
			buttons = new GameObject[Enum.GetNames(typeof(LogicGateType)).Length];
			target = null;
		}

		public override void ClearTarget() {
			target = null;
		}

		/// <summary>
		/// Creates a deselected button for a particular gate type.
		/// </summary>
		/// <param name="type">The button type.</param>
		/// <returns>A blue button with the proper text.</returns>
		private PButton CreateGateButton(LogicGateType type) {
			string text = type.ToString().ToUpper();
			var button = new PButton(text) {
				Color = PUITuning.Colors.ButtonBlueStyle,
				Margin = new RectOffset(8, 8, 3, 3),
				TextStyle = PUITuning.Fonts.TextLightStyle,
				ToolTip = "Logic: " + text,
				Text = text,
				FlexSize = Vector2.right,
				OnClick = (obj) => SetGateType(type)
			};
			button.OnRealize += (obj) => buttons[(int)type] = obj;
			return button;
		}

		public override string GetTitle() {
			return DenseLogicStrings.UI.UISIDESCREENS.GATESELECT.TITLE;
		}

		public override bool IsValidForTarget(GameObject target) {
			return target.GetComponent<IConfigurableLogicGate>() != null;
		}

		protected override void OnPrefabInit() {
			var margin = new RectOffset(4, 4, 4, 4);
			var ss = new PGridPanel("GateLogicSelect") { Margin = margin }.
				AddRow(new GridRowSpec()).AddRow(new GridRowSpec()).AddRow(new GridRowSpec()).
				AddColumn(new GridColumnSpec(flex: 0.5f)).AddColumn(new GridColumnSpec()).
				AddColumn(new GridColumnSpec()).AddColumn(new GridColumnSpec(flex: 0.5f));
			var and = CreateGateButton(LogicGateType.And);
			and.Color = PUITuning.Colors.ButtonPinkStyle;
			ss.AddChild(and, new GridComponentSpec(0, 1) { Margin = margin });
			ss.AddChild(CreateGateButton(LogicGateType.Or), new GridComponentSpec(0, 2) {
				Margin = margin
			});
			ss.AddChild(CreateGateButton(LogicGateType.Xor), new GridComponentSpec(1, 1) {
				Margin = margin
			});
			ss.AddChild(CreateGateButton(LogicGateType.Nand), new GridComponentSpec(1, 2) {
				Margin = margin
			});
			ss.AddChild(CreateGateButton(LogicGateType.Nor), new GridComponentSpec(2, 1) {
				Margin = margin
			});
			ss.AddChild(CreateGateButton(LogicGateType.Xnor), new GridComponentSpec(2, 2) {
				Margin = margin
			});
			ContentContainer = ss.Build();
			base.OnPrefabInit();
			ContentContainer.SetParent(gameObject);
			UpdateGateType();
		}

		/// <summary>
		/// Called to set the gate type by pressing the buttons.
		/// </summary>
		/// <param name="type">The gate type to use.</param>
		private void SetGateType(LogicGateType type) {
			if (target != null)
				target.GateType = type;
			UpdateGateType();
		}

		public override void SetTarget(GameObject target) {
			if (target == null)
				PUtil.LogError("Invalid target specified");
			else {
				this.target = target.GetComponent<IConfigurableLogicGate>();
				UpdateGateType();
			}
		}

		/// <summary>
		/// Updates the button colors, making the selected button pink and the rest blue.
		/// </summary>
		private void UpdateGateType() {
			int n = buttons.Length;
			if (target != null) {
				var type = target.GateType;
				for (int i = 0; i < n; i++) {
					var color = ((LogicGateType)i == type) ? PUITuning.Colors.ButtonPinkStyle :
						PUITuning.Colors.ButtonBlueStyle;
					var image = buttons[i].GetComponentSafe<KImage>();
					if (image != null) {
						image.colorStyleSetting = color;
						image.ApplyColorStyleSetting();
					}
				}
			}
		}
	}
}
