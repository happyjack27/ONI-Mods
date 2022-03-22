using KBComputing.util;
using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KBComputing.SideScreens {
	internal sealed class Stack8SideScreen : AbstractSideScreen<IStack>
	{

		GameObject contentField = null;
		GameObject operationField = null;

		public override string GetTitle()
		{
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
					Alignment = TextAnchor.UpperCenter,
					Spacing = 8
				};
			var rowBG = PUITuning.Images.GetSpriteByName("overview_highlight_outline_sharp");
			/*
			body.AddRow(UI.MODIFYDIALOG.CAPTION, new PTextField("Title")
			{
				Text = editor.Title,
				MaxLength = 127,
				MinWidth = 512,
				BackColor =
				PUITuning.Colors.DialogDarkBackground,
				TextStyle = PUITuning.Fonts.
				TextLightStyle,
				TextAlignment = TMPro.TextAlignmentOptions.Left
			}.AddOnRealize((obj) => titleField = obj));
			*/

			PPanel rowOperation = new PPanel("Panel")
			{
				BackImage = rowBG,
				BackColor = new Color(0.898f, 0.898f, 0.898f),
				ImageMode = Image.Type.Sliced,
				Alignment = TextAnchor.UpperLeft,
				Direction = PanelDirection.Horizontal,
				Spacing = 10,
				Margin = margin,
				FlexSize = Vector2.right
			};
			PLabel operationLabel = new PLabel("Operation");
			PTextField operationTextField = new PTextField("")
			{
				Text = "",
				MaxLength = 32,
				MinWidth = 128,
				BackColor =
				PUITuning.Colors.DialogDarkBackground,
				TextStyle = PUITuning.Fonts.
				TextLightStyle,
				TextAlignment = TMPro.TextAlignmentOptions.Left
			}.AddOnRealize((obj) => operationField = obj);

			rowOperation.AddChild(operationLabel);
			rowOperation.AddChild(operationTextField);
			rowOperation.AddTo(gameObject);

			/*
			PPanel rowSize = new PPanel("Panel")
			{
				BackImage = rowBG,
				BackColor = new Color(0.898f, 0.898f, 0.898f),
				ImageMode = Image.Type.Sliced,
				Alignment = TextAnchor.UpperLeft,
				Direction = PanelDirection.Horizontal,
				Spacing = 10,
				Margin = margin,
				FlexSize = Vector2.right
			};
			PLabel sizeLabel = new PLabel("Stack Size");

			rowSize.AddChild(sizeLabel);
			rowSize.AddTo(gameObject);
			*/

			PPanel rowContent = new PPanel("Panel")
			{
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
				LineCount = 16,
				MaxLength = 1024,
				MinWidth = 128,
				Text = "placeholder text",
				TextAlignment = TextAlignmentOptions.TopLeft,
				TextStyle = PUITuning.Fonts.TextDarkStyle,
			}.AddOnRealize((obj) => contentField = obj);

			rowContent.AddChild(contents);
			rowContent.AddTo(gameObject);


			ContentContainer = gameObject;

			base.OnPrefabInit();
			Load(gameObject);
		}



		protected override void Clear(GameObject _)
        {
        }

        protected override void Store(GameObject _)
        {
		}

		protected override void Load(GameObject _)
		{

			if (target == null || contentField == null)
				return;

			
			Stack<byte> stack = target.getStack();
			byte flags = target.getFlags();
			byte operation = target.getOp();
			byte[] bb = stack.ToArray();

			StringBuilder sbop = new StringBuilder();
			sbop.Append(MemoryTranslation.HEX[operation >> 4]);
			sbop.Append(MemoryTranslation.HEX[operation & 0x0F]);
			sbop.Append(" - ");
			sbop.Append(StackOpCodeTranslate.NAMES[operation]);


			StringBuilder builder = new StringBuilder();
			/*
			builder.Append("Operation: ");
			builder.Append(MemoryTranslation.HEX[operation >> 4]);
			builder.Append(MemoryTranslation.HEX[operation & 0x0F]);
			builder.Append(" ");
			builder.Append(StackOpCodeTranslate.NAMES[operation]);
			builder.Append("\nSize: ");
			*/
			//builder.Append(bb.Length);
			//builder.Append("\n\n");
			string stacktest = MemoryTranslation.bytesToHEX(bb);
			builder.Append(stacktest);
			string text = builder.ToString();

			TMP_InputField input = contentField?.GetComponent<TMP_InputField>();
			input.textComponent.SetText(text);
			input.text = text;

			TMP_InputField input2 = operationField?.GetComponent<TMP_InputField>();
			input2.textComponent.SetText(sbop.ToString());
			input2.text = sbop.ToString();

		}
    }
}
