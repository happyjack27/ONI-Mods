using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KBComputing.SideScreens {
	internal sealed class MemoryContentsSideScreen : AbstractSideScreen<IMemoryContents>
	{
		int bank = 0;
		string displayMode = "HEX";

		protected IList<ListOption<string>> optionsDisplayMode;
		protected IList<ListOption<int>> optionsBank;

		private GameObject contentField;
		private GameObject displayModeField;
		private GameObject bankField;

		private void BankOptionSelected(GameObject obj, ListOption<int> option)
		{
			bank = option.value;
			if (target != null && target.ContentDisplayBank != bank)
            {
				target.ContentDisplayBank = bank;
				Load(null);
			}
		}
		private void DisplayModeOptionSelected(GameObject obj, ListOption<string> option)
		{
			displayMode = option.value;
			if (target != null && !target.ContentDisplayMode.Equals(displayMode))
            {
				target.ContentDisplayMode = displayMode;
				Load(null);
			}
		}

		/*protected override void ClearTarget()
		{
			base.ClearTarget();
			bank = 0;
		}
		*/

		public override string GetTitle() {
			return SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.TITLE;
		}

		protected override void OnPrefabInit() {
			optionsDisplayMode = new List<ListOption<string>>(2);
			optionsBank = new List<ListOption<int>>(4);
			optionsDisplayMode.Add(new ListOption<string>("HEX", "HEX"));
			optionsDisplayMode.Add(new ListOption<string>("stack ops", "stack ops"));
			optionsBank.Add(new ListOption<int>(0, "0"));
			optionsBank.Add(new ListOption<int>(1, "1"));
			optionsBank.Add(new ListOption<int>(2, "2"));
			optionsBank.Add(new ListOption<int>(3, "3"));

			var margin = new RectOffset(4, 4, 4, 4);
			// Update the parameters of the base BoxLayoutGroup
			var baseLayout = gameObject.GetComponent<BoxLayoutGroup>();
			if (baseLayout != null)
				baseLayout.Params = new BoxLayoutParams() {
					Margin = margin,
					Direction = PanelDirection.Vertical,
					Alignment = TextAnchor.UpperCenter,
					Spacing = 8
				};
			var rowBG = PUITuning.Images.GetSpriteByName("overview_highlight_outline_sharp");

			PPanel rowDisplayMode = new PPanel("DisplayModeRow")
			{
				FlexSize = Vector2.right,
				Alignment = TextAnchor.MiddleLeft,
				Spacing = 10,
				Direction = PanelDirection.Horizontal,
				Margin = new RectOffset(8, 8, 8, 8)
			}.AddChild(new PLabel("DisplayModeLabel")
			{
				TextAlignment = TextAnchor.MiddleLeft,
				ToolTip = "",
				Text = "Display Mode",
				TextStyle = PUITuning.Fonts.TextDarkStyle,
				FlexSize = Vector2.left,
			});
			var cbDisplayMode = new PComboBox<ListOption<string>>("DisplayModeSelect")
			{
				Content = optionsDisplayMode,
				InitialItem = optionsDisplayMode[0],
				ToolTip = "",
				TextStyle = PUITuning.Fonts.TextLightStyle,
				TextAlignment = TextAnchor.MiddleRight,
				OnOptionSelected = DisplayModeOptionSelected,
				FlexSize = Vector2.right,
			}.AddOnRealize((obj) => displayModeField = obj);
			rowDisplayMode.AddChild(cbDisplayMode);
			rowDisplayMode.AddTo(gameObject);

			PPanel rowBank = new PPanel("BankRow")
			{
				FlexSize = Vector2.one,
				Alignment = TextAnchor.MiddleCenter,
				Spacing = 10,
				Direction = PanelDirection.Horizontal,
				Margin = new RectOffset(8, 8, 8, 8)
			}.AddChild(new PLabel("BankLabel")
			{
				TextAlignment = TextAnchor.MiddleLeft,
				ToolTip = "",
				Text = "Bank",
				TextStyle = PUITuning.Fonts.TextDarkStyle,
				FlexSize = Vector2.left,
			});
			var cbBank = new PComboBox<ListOption<int>>("BankSelect")
			{
				Content = optionsBank,
				InitialItem = optionsBank[0],
				FlexSize = Vector2.right,
				ToolTip = "",
				TextStyle = PUITuning.Fonts.TextLightStyle,
				TextAlignment = TextAnchor.MiddleRight,
				OnOptionSelected = BankOptionSelected
			}.AddOnRealize((obj) => bankField = obj);
			rowBank.AddChild(cbBank);
			rowBank.AddTo(gameObject);
			

			PPanel rowContent = new PPanel("Panel") {
				BackImage = rowBG,
				BackColor = new Color(0.898f, 0.898f, 0.898f),
				ImageMode = Image.Type.Sliced,
				Alignment = TextAnchor.UpperLeft,
				Direction = PanelDirection.Horizontal,
				Spacing = 10,
				Margin = margin,
				FlexSize = Vector2.right
			};

			PTextArea contents = new PTextArea("TAContents")
			{
				BackColor = PUITuning.Colors.BackgroundLight,
				FlexSize = new Vector2(8.0f, 8.0f),
				//Vector2.one,
				LineCount = 64,
				MaxLength = 8192,
				MinWidth = 64,
				Text = "placeholder text",
				TextAlignment = TextAlignmentOptions.TopLeft,
				TextStyle = PUITuning.Fonts.TextDarkStyle,
			}.AddOnRealize((obj) => contentField = obj);
			
			/*
			PScrollPane contentsContainer = new PScrollPane("contentsContainer")
			{
				//AlwaysShowHorizontal = (AlwaysShowVertical = true),
				BackColor = PUITuning.Colors.Transparent,
				Child = contents,
				FlexSize = new Vector2(8.0f, 8.0f),
				//FlexSize = Vector2.zero,
				ScrollHorizontal = false,
				ScrollVertical = true,
				//TrackSize = PScrollPane.DEFAULT_TRACK_SIZE,
			};
			*/


			//contents.AddTo(contentsContainer);
			rowContent.AddChild(contents);
			//rowContent.AddChild(contentsContainer);
			rowContent.AddTo(gameObject);

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
			Load(gameObject);
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
			byte[] bytes = new byte[256];
			switch(displayMode)
            {
				case "HEX":
					bytes = util.MemoryTranslation.HEXtoBytes(256, text);
					break;
				case "stack ops":
					bytes = util.MemoryTranslation.StackOpsToBytes(256, text);
					break;
				default:
					bytes = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, };
					break;
			}
			target.setBank(bank, bytes);
		}

		protected override void Load(GameObject _)
		{
			if (target == null || contentField == null)
				return;
			//contents.setText(). = "test contents \n";
			try
			{

				displayMode = target.ContentDisplayMode;
				for (int i = 0; i < optionsDisplayMode.Count; i++)
				{
					if (optionsDisplayMode[i].value.Equals(displayMode))
					{
						ListOption<string> option = optionsDisplayMode[i];
						if (displayModeField != null)
							PComboBox<ListOption<string>>.SetSelectedItem(displayModeField, option);
						break;
					}
				}

				bank = target.ContentDisplayBank;
				for (int i = 0; i < optionsBank.Count; i++)
				{
					if (optionsBank[i].value == bank)
					{
						ListOption<int> option = optionsBank[i];
						if (bankField != null)
							PComboBox<ListOption<int>>.SetSelectedItem(bankField, option);
						break;
					}
				}

				//displayModeField.set

				byte[] bytes = target.getBank(bank);
				string text = "";
				switch (displayMode)
				{
					case "HEX":
						text = util.MemoryTranslation.bytesToHEX(bytes);
						break;
					case "stack ops":
						text = util.MemoryTranslation.bytesToStackOps(bytes);
						break;
					default:
						text = "NO MATCHING TYPE FOUND!";
						break;
				}
				TMP_InputField input = contentField?.GetComponent<TMP_InputField>();
				input.textComponent.SetText(text);
				input.text = text;
			}
			catch { }
			
			//string cur = PTextField.Tex
			//cur.re
		}
    }
}
