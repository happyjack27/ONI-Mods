using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KBComputing {
	internal sealed class MemoryContentsSideScreen : AbstractSideScreen<IMemoryContents>
	{
		int bank = 0;
		PTextArea contents;

		private GameObject contentField;

		internal MemoryContentsSideScreen() {
		}

		public override string GetTitle() {
			return SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.TITLE;
		}

		protected override void OnPrefabInit() {
			var margin = new RectOffset(4, 4, 4, 4);
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
			contents = new PTextArea("TAContents")
			{
				BackColor = PUITuning.Colors.BackgroundLight,
				FlexSize = new Vector2(8.0f, 8.0f),
						   //Vector2.one,
				LineCount = 30,
				MaxLength = 1024,
				MinWidth = 64,
				Text =
					target.getContents(bank,"HEX"),
					//"intial text",
				TextAlignment = TextAlignmentOptions.TopLeft,
				TextStyle = PUITuning.Fonts.TextDarkStyle,
			}.AddOnRealize((obj) => contentField = obj);
			/*
			PScrollPane contentsContainer = new PScrollPane("contentsContainer")
			{
				//AlwaysShowHorizontal = (AlwaysShowVertical = true),
				BackColor = PUITuning.Colors.Transparent,
				Child = contents,
				FlexSize = Vector2.zero,
				ScrollHorizontal = false,
				ScrollVertical = false,
				//TrackSize = PScrollPane.DEFAULT_TRACK_SIZE,
			};
			*/
			//contents.AddTo(contentsContainer);
			//contentsContainer.AddTo(new PPanel("Contents"));

			PPanel row = new PPanel("Panel") {
				BackImage = rowBG,
				BackColor = new Color(0.898f, 0.898f, 0.898f),
				ImageMode = Image.Type.Sliced,
				Alignment = TextAnchor.MiddleCenter,
				Direction = PanelDirection.Horizontal,
				Spacing = 10,
				Margin = margin,
				FlexSize = Vector2.right
			};
			/*
			row.AddChild(new PLabel("ContentsLabel") {
				TextAlignment = TextAnchor.MiddleRight,
				Text = SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.SAVE,
				TextStyle = PUITuning.Fonts.TextDarkStyle
			})
			*/;
			row.AddChild(contents);
			//row.AddChild(contentsContainer);
			//cb.OnRealize += (obj) => bitSelects[index] = obj;
			//row.AddChild(cb);
			row.AddTo(gameObject);

			// Save / Reload / Clear
			GameObject BottomRow = new PPanel("BottomRow") {
				Alignment = TextAnchor.MiddleCenter,
				Direction = PanelDirection.Horizontal,
				Spacing = 10,
				Margin = margin
			}.AddChild(new PButton() {
				Color = PUITuning.Colors.ButtonBlueStyle,
				Margin = new RectOffset(8, 8, 3, 3),
				TextStyle = PUITuning.Fonts.TextLightStyle,
				OnClick = Store,
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
			//Load(gameObject);
		}



		protected override void Clear(GameObject _)
        {
			target.ClearContents(bank);
			Load(gameObject);
        }

        protected override void Store(GameObject _)
        {
			if (target == null || contentField == null)
				return;
			string text = PTextField.GetText(contentField);
			target.setContents(bank, "HEX", text);// "00 01 02 03 ff ee dd cc");
		}

		protected override void Load(GameObject _)
		{
			if (target == null || contentField == null)
				return;
			//contents.setText(). = "test contents \n";
			try
			{
				string text = target.getContents(bank, "HEX");
				TMP_InputField input = contentField?.GetComponent<TMP_InputField>();
				input.textComponent.SetText(text);
			}
			catch { }
			
			//string cur = PTextField.Tex
			//cur.re
		}
    }
}
