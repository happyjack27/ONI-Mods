using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KBComputing {
	internal sealed class MultiplexerSideScreen : AbstractSideScreen<IMemoryContents>
	{
		int bank = 0;
		PTextArea contents;
		internal MultiplexerSideScreen() {
		}

		public override string GetTitle() {
			return SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.TITLE;
		}

		protected override void OnPrefabInit() {
			var margin = new RectOffset(8, 8, 8, 8);
			// Update the parameters of the base BoxLayoutGroup
			var baseLayout = gameObject.GetComponent<BoxLayoutGroup>();
			if (baseLayout != null)
				baseLayout.Params = new BoxLayoutParams() {
					Margin = margin,
					Direction = PanelDirection.Vertical,
					Alignment =
					TextAnchor.UpperCenter,
					Spacing = 8
				};
			var rowBG = PUITuning.Images.GetSpriteByName("overview_highlight_outline_sharp");
			PScrollPane contentsContainer = new PScrollPane("contentsContainer")
			{
				//AlwaysShowHorizontal = (AlwaysShowVertical = true),
				BackColor = PUITuning.Colors.Transparent,
				Child = null,
				FlexSize = Vector2.zero,
				ScrollHorizontal = false,
				ScrollVertical = false,
				//TrackSize = PScrollPane.DEFAULT_TRACK_SIZE,
			};
			contents = new PTextArea("TAContents")
			{
				BackColor = PUITuning.Colors.BackgroundLight,
				FlexSize = Vector2.one,
				LineCount = 4,
				MaxLength = 1024,
				MinWidth = 64,
				Text = null,
				TextAlignment = TextAlignmentOptions.TopLeft,
				TextStyle = PUITuning.Fonts.TextDarkStyle,
			};
			//contents.AddTo(contentsContainer);
			//contentsContainer.AddTo(new PPanel("Contents"));

			PPanel row = new PPanel("Contents") {
				BackImage = rowBG,
				BackColor = new Color(0.898f, 0.898f, 0.898f),
				ImageMode = Image.Type.Sliced,
				Alignment = TextAnchor.MiddleCenter,
				Direction = PanelDirection.Horizontal,
				Spacing = 10,
				Margin = margin,
				FlexSize = Vector2.right
			}.AddChild(new PLabel("Contents") {
				TextAlignment = TextAnchor.MiddleRight,
				Text = "Contents",
				TextStyle = PUITuning.Fonts.TextDarkStyle
			});
			row.AddChild(contents);
				//cb.OnRealize += (obj) => bitSelects[index] = obj;
				//row.AddChild(cb);
			row.AddTo(gameObject);

			// Save / Reload / Clear
			new PPanel("BottomRow") {
				Alignment = TextAnchor.MiddleCenter, Direction = PanelDirection.Horizontal,
				Spacing = 10, Margin = margin
			}.AddChild(new PButton() {
				Color = PUITuning.Colors.ButtonBlueStyle, Margin = new RectOffset(8, 8, 3, 3),
				TextStyle = PUITuning.Fonts.TextLightStyle, OnClick = Store,
				Text = SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.SAVE
			}).AddChild(new PButton()
			{
				Color = PUITuning.Colors.ButtonBlueStyle,
				Margin = new RectOffset(8, 8, 3, 3),
				TextStyle = PUITuning.Fonts.TextLightStyle,
				OnClick = Load,
				Text = SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.LOAD
			}).AddChild(new PButton()
			{
				Color = PUITuning.Colors.ButtonBlueStyle,
				Margin = new RectOffset(8, 8, 3, 3),
				TextStyle = PUITuning.Fonts.TextLightStyle,
				OnClick = Clear,
				Text = SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.CLEAR
			}).AddTo(gameObject);
			ContentContainer = gameObject;
			base.OnPrefabInit();
			Load(gameObject);
		}



        protected override void Clear(GameObject _)
        {
			target.ClearContents(bank);
			Load(gameObject);
        }

        protected override void Store(GameObject _)
        {
			target.setContents(bank, contents.Text);
        }

        protected override void Load(GameObject _)
		{
			contents.Text = target.getContents(bank);
        }
    }
}
