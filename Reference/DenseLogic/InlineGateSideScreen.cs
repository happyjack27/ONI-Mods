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
using System.Collections.Generic;
using UnityEngine;

namespace ONI_DenseLogic {
	internal sealed class InlineGateSideScreen : SideScreenContent {
		private readonly IList<BitOption> bitNames;

		/// <summary>
		/// Shown when an invalid bit pattern is selected.
		/// </summary>
		private GameObject invalidWarning;

		private GameObject inputOne, inputTwo, output;

		private IInlineSelectable target;

		internal InlineGateSideScreen() {
			// "None" is not an option here
			bitNames = new List<BitOption>(DenseLogicGate.NUM_BITS);
			target = null;
		}

		private void BuildRow(string title, string tooltip, PUIDelegates.OnRealize onRealize) {
			var row = new PPanel("BitRow") {
				FlexSize = Vector2.right, Alignment = TextAnchor.MiddleCenter, Spacing = 10,
				Direction = PanelDirection.Horizontal, Margin = new RectOffset(8, 8, 8, 8)
			}.AddChild(new PLabel("BitLabel") {
				TextAlignment = TextAnchor.MiddleRight, ToolTip = tooltip,
				Text = title, TextStyle = PUITuning.Fonts.TextDarkStyle
			});
			var cb = new PComboBox<BitOption>("BitSelect") {
				Content = bitNames, InitialItem = bitNames[0], ToolTip = tooltip,
				TextStyle = PUITuning.Fonts.TextLightStyle, TextAlignment = TextAnchor.
				MiddleLeft, OnOptionSelected = SetBit
			};
			cb.OnRealize += onRealize;
			row.AddChild(cb).AddTo(gameObject);
		}

		private void CheckSignals() {
			int outBit = target.OutputBit;
			// In1 == In2 is allowed
			bool invalid = outBit == target.InputBit1 || outBit == target.InputBit2;
			if (invalidWarning != null)
				PUIElements.SetText(invalidWarning, invalid ? (string)DenseLogicStrings.UI.
					UISIDESCREENS.INLINELOGIC.INVALID_BITS : " ");
		}

		public override void ClearTarget() {
			target = null;
		}

		public override string GetTitle() {
			return DenseLogicStrings.UI.UISIDESCREENS.INLINELOGIC.TITLE;
		}

		public override bool IsValidForTarget(GameObject target) {
			return target.GetComponent<IInlineSelectable>() != null;
		}

		private void LoadBitSettings() {
			if (target != null) {
				if (inputOne != null)
					PComboBox<BitOption>.SetSelectedItem(inputOne, bitNames[target.InputBit1]);
				if (inputTwo != null)
					PComboBox<BitOption>.SetSelectedItem(inputTwo, bitNames[target.InputBit2]);
				if (output != null)
					PComboBox<BitOption>.SetSelectedItem(output, bitNames[target.OutputBit]);
				CheckSignals();
			}
		}

		protected override void OnPrefabInit() {
			var margin = new RectOffset(8, 8, 8, 8);
			// Update the parameters of the base BoxLayoutGroup
			var baseLayout = gameObject.GetComponent<BoxLayoutGroup>();
			if (baseLayout != null)
				baseLayout.Params = new BoxLayoutParams() {
					Margin = margin, Direction = PanelDirection.Vertical, Alignment =
					TextAnchor.UpperCenter, Spacing = 8
				};
			bitNames.Add(DenseLogicStrings.UI.UISIDESCREENS.SIGNALREMAPPER.BIT_1);
			bitNames.Add(DenseLogicStrings.UI.UISIDESCREENS.SIGNALREMAPPER.BIT_2);
			bitNames.Add(DenseLogicStrings.UI.UISIDESCREENS.SIGNALREMAPPER.BIT_3);
			bitNames.Add(DenseLogicStrings.UI.UISIDESCREENS.SIGNALREMAPPER.BIT_4);
			// Bit select options
			BuildRow(DenseLogicStrings.UI.UISIDESCREENS.INLINELOGIC.INPUT_ONE,
				DenseLogicStrings.UI.TOOLTIPS.INLINELOGIC.INPUT_ONE,
				(obj) => inputOne = obj);
			BuildRow(DenseLogicStrings.UI.UISIDESCREENS.INLINELOGIC.INPUT_TWO,
				DenseLogicStrings.UI.TOOLTIPS.INLINELOGIC.INPUT_TWO,
				(obj) => inputTwo = obj);
			BuildRow(DenseLogicStrings.UI.UISIDESCREENS.INLINELOGIC.OUTPUT,
				DenseLogicStrings.UI.TOOLTIPS.INLINELOGIC.OUTPUT,
				(obj) => output = obj);
			// Setting color to red is actually effort
			var defaultStyle = PUITuning.Fonts.UIDarkStyle;
			var errorStyle = ScriptableObject.CreateInstance<TextStyleSetting>();
			errorStyle.enableWordWrapping = false;
			errorStyle.fontSize = defaultStyle.fontSize;
			errorStyle.sdfFont = defaultStyle.sdfFont;
			errorStyle.textColor = Color.red;
			invalidWarning = new PLabel("InvalidWarning") {
				Text = " ",
				TextAlignment = TextAnchor.MiddleCenter, Margin = margin,
				TextStyle = errorStyle
			}.AddTo(gameObject);
			ContentContainer = gameObject;
			base.OnPrefabInit();
			LoadBitSettings();
		}

		private void SetBit(GameObject obj, BitOption option) {
			int index = bitNames.IndexOf(option);
			if (target != null && index >= 0 && index < DenseLogicGate.NUM_BITS) {
				if (obj == inputOne)
					target.InputBit1 = index;
				else if (obj == inputTwo)
					target.InputBit2 = index;
				else if (obj == output)
					target.OutputBit = index;
				else if (obj != null)
					PUtil.LogWarning("Invalid source supplied to SetBit");
				CheckSignals();
			}
		}

		public override void SetTarget(GameObject target) {
			if (target == null)
				PUtil.LogError("Invalid gameObject received");
			else {
				this.target = target.GetComponent<IInlineSelectable>();
				if (this.target == null)
					PUtil.LogError("The gameObject received is not an IInlineSelectable");
				else
					LoadBitSettings();
			}
		}
	}
}
