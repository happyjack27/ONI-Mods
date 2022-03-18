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
using UnityEngine.UI;
using STRINGS;

namespace ONI_DenseLogic {
	/// <summary>
	/// Controls the logic behind the side screen for selecting the bit values
	/// of a building with configurable bits.
	/// </summary>
	internal sealed class FourBitSelectSideScreen : SideScreenContent {
		private static readonly Color ACTIVE_COLOR = new Color(0.3411765f, 0.7254902f, 0.3686275f);
		private static readonly Color INACTIVE_COLOR = new Color(0.9529412f, 0.2901961f, 0.2784314f);

		/// <summary>
		/// The bit toggles in the UI.
		/// </summary>
		private readonly IList<BitSelectRow> toggles;

		/// <summary>
		/// The selected building with bits to modify/visualize.
		/// </summary>
		private IConfigurableFourBits target;

		internal FourBitSelectSideScreen() {
			toggles = new List<BitSelectRow>(DenseLogicGate.NUM_BITS);
			target = null;
		}

		public override void ClearTarget() {
			target = null;
		}

		private void DisableAll(GameObject _) {
			if (target != null) {
				for (int i = 0; i < DenseLogicGate.NUM_BITS; i++)
					target.SetBit(false, i);
				RefreshToggles();
			}
		}

		private void EnableAll(GameObject _) {
			if (target != null) {
				for (int i = 0; i < DenseLogicGate.NUM_BITS; i++)
					target.SetBit(true, i);
				RefreshToggles();
			}
		}

		private void FlipBit(int bit) {
			if (target != null) {
				target.SetBit(!target.GetBit(bit), bit);
				RefreshToggles();
			}
		}

		public override string GetTitle() {
			return DenseLogicStrings.UI.UISIDESCREENS.FOURBITSELECT.TITLE;
		}

		public override bool IsValidForTarget(GameObject target) {
			return target.GetComponent<IConfigurableFourBits>() != null;
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
			toggles.Clear();
			for (int i = 0; i < DenseLogicGate.NUM_BITS; i++) {
				// Capture the right value!
				int index = i;
				var row = new BitSelectRow(index);
				toggles.Add(row);
				row.Row.AddTo(gameObject);
				row.Toggle.onClick += () => FlipBit(index);
			}
			new PPanel("BottomRow") {
				Alignment = TextAnchor.MiddleCenter, Direction = PanelDirection.Horizontal,
				Spacing = 10, Margin = margin
			}.AddChild(new PButton() {
				Color = PUITuning.Colors.ButtonBlueStyle, Margin = new RectOffset(8, 8, 3, 3),
				TextStyle = PUITuning.Fonts.TextLightStyle, OnClick = EnableAll,
				ToolTip = DenseLogicStrings.UI.TOOLTIPS.FOURBITSELECT.ENABLE_ALL,
				Text = DenseLogicStrings.UI.UISIDESCREENS.FOURBITSELECT.ENABLE_ALL
			}).AddChild(new PButton() {
				Color = PUITuning.Colors.ButtonBlueStyle, Margin = new RectOffset(8, 8, 3, 3),
				TextStyle = PUITuning.Fonts.TextLightStyle, OnClick = DisableAll,
				ToolTip = DenseLogicStrings.UI.TOOLTIPS.FOURBITSELECT.DISABLE_ALL,
				Text = DenseLogicStrings.UI.UISIDESCREENS.FOURBITSELECT.DISABLE_ALL
			}).AddTo(gameObject);
			ContentContainer = gameObject;
			base.OnPrefabInit();
			RefreshToggles();
		}

		public override void SetTarget(GameObject target) {
			if (target == null)
				PUtil.LogError("Invalid gameObject received");
			else {
				this.target = target.GetComponent<IConfigurableFourBits>();
				if (this.target == null)
					PUtil.LogError("The gameObject received is not an IConfigurableFourBits");
				else
					RefreshToggles();
			}
		}

		/// <summary>
		/// Updates the state of the bit toggles, based on the state in the target.
		/// </summary>
		private void RefreshToggles() {
			if (target != null)
				for (int i = 0; i < toggles.Count; i++) {
					bool bitOn = target.GetBit(i);
					var row = toggles[i];
					row.StateIcon.color = bitOn ? ACTIVE_COLOR : INACTIVE_COLOR;
					row.StateText.SetText(bitOn ?
						UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE :
						UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
				}
		}

		private sealed class BitSelectRow {
			internal static readonly ColorStyleSetting BitSelectColorStyle;
			internal static readonly ColorStyleSetting BitSelectOutlineStyle;

			static BitSelectRow() {
				BitSelectColorStyle = ScriptableObject.CreateInstance<ColorStyleSetting>();
				BitSelectColorStyle.activeColor = new Color(0.96f, 0.89f, .93f);
				BitSelectColorStyle.inactiveColor = new Color(1.0f, 1.0f, 1.0f);
				BitSelectColorStyle.hoverColor = new Color(0.93f, 0.86f, .90f);
				BitSelectOutlineStyle = ScriptableObject.CreateInstance<ColorStyleSetting>();
				BitSelectOutlineStyle.activeColor = new Color(0.94f, 0.84f, 0.88f);
				BitSelectOutlineStyle.inactiveColor = new Color(0.898f, 0.898f, 0.898f);
				BitSelectOutlineStyle.hoverColor = new Color(0.91f, 0.81f, 0.85f);
			}

			public PPanel Row { get; }

			public KButton Toggle { get; private set; }

			public Image StateIcon { get; private set; }

			public LocText StateText { get; private set; }

			private KImage outlineImage;

			public BitSelectRow(int pos) {
				// TODO This could probably be done as a relative layout eventually, but
				// until PRelativePanel is added this is the best we can do
				Row = new PPanel("BitSelectRow_" + pos) {
					FlexSize = Vector2.right, Direction = PanelDirection.Horizontal
				};
				Row.OnRealize += gameObject => {
					var img = gameObject.AddOrGet<KImage>();
					// Since we need the color style setting this cannot be done with the
					// stock BackImage
					img.sprite = PUITuning.Images.GetSpriteByName("BitSelectorSideScreenRow");
					img.type = Image.Type.Sliced;
					img.colorStyleSetting = BitSelectColorStyle;
					Toggle = gameObject.AddOrGet<KButton>();
					Toggle.soundPlayer = new ButtonSoundPlayer() { Enabled = true };
					Toggle.bgImage = img;
					// this only works as long as the children are realized before the parents
					// there is probably a better way to do this but this works under that assumption
					Toggle.additionalKImages = new KImage[1] { outlineImage };
				};
				var RowInternal = new PGridPanel("BitSelectRowInternal") {
					Margin = new RectOffset(8, 8, 8, 8),
					FlexSize = Vector2.right,
				};
				RowInternal.OnRealize += gameObject => {
					var img = gameObject.AddOrGet<KImage>();
					// Again the color style is needed here
					img.sprite = PUITuning.Images.GetSpriteByName("overview_highlight_outline_sharp");
					img.type = Image.Type.Sliced;
					img.colorStyleSetting = BitSelectOutlineStyle;
					outlineImage = img;
				};
				Row.AddChild(RowInternal);
				RowInternal.AddRow(new GridRowSpec()).AddColumn(new GridColumnSpec(64.0f)).
					AddColumn(new GridColumnSpec(flex: 0.33f)).
					AddColumn(new GridColumnSpec(flex: 0.67f));
				// Red or green square
				var stateIcon = new PLabel("StateIcon") {
					Margin = new RectOffset(6, 6, 6, 6),
					Sprite = PUITuning.Images.GetSpriteByName("web_box_shadow"),
					SpriteSize = new Vector2(32, 32),
					SpriteMode = Image.Type.Sliced
				};
				stateIcon.OnRealize += gameObject =>
					StateIcon = gameObject.GetComponentInChildren<Image>();
				RowInternal.AddChild(stateIcon, new GridComponentSpec(0, 0) {
					Alignment = TextAnchor.MiddleLeft
				});
				RowInternal.AddChild(new PLabel("BitName") {
					Text = string.Format(UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.BIT, pos + 1),
					TextStyle = PUITuning.Fonts.TextDarkStyle
				}, new GridComponentSpec(0, 1) {
					Alignment = TextAnchor.MiddleLeft
				});
				var stateText = new PLabel("StateText") {
					Text = UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE,
					TextStyle = PUITuning.Fonts.TextDarkStyle
				};
				stateText.OnRealize += gameObject =>
					StateText = gameObject.GetComponentInChildren<LocText>();
				RowInternal.AddChild(stateText, new GridComponentSpec(0, 2) {
					Alignment = TextAnchor.MiddleLeft
				});
			}
		}
	}
}
