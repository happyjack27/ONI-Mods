// Decompiled with JetBrains decompiler
// Type: PeterHan.PLib.UI.SensibleTextArea
// Assembly: PLib, Version=4.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 642DAF9A-7503-46F0-9C39-02EBD9828B7C
// Assembly location: C:\Users\kbaas\AppData\Local\Temp\Hulamep\c84f4254ab\lib\net471\PLib.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PeterHan.PLib.UI
{
    /// <summary>
    /// A custom UI text area (multi-line text field) factory class. This class should
    /// probably be wrapped in a scroll pane.
    /// </summary>
    public sealed class SensibleTextArea : IUIComponent
    {
        /// <summary>The text area's background color.</summary>
        public Color BackColor { get; set; }

        /// <summary>The flexible size bounds of this component.</summary>
        public Vector2 FlexSize { get; set; }

        /// <summary>
        /// The preferred number of text lines to be displayed. If the component is made
        /// bigger, the number of text lines (and size) can increase.
        /// </summary>
        public int LineCount { get; set; }

        /// <summary>The maximum number of characters in this text area.</summary>
        public int MaxLength { get; set; }

        public string Name { get; }

        /// <summary>
        /// The minimum width in units (not characters!) of this text area.
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>The text alignment in the text area.</summary>
        public TextAlignmentOptions TextAlignment { get; set; }

        /// <summary>The initial text in the text field.</summary>
        public string Text { get; set; }

        /// <summary>
        /// The text field's text color, font, word wrap settings, and font size.
        /// </summary>
        public TextStyleSetting TextStyle { get; set; }

        /// <summary>The tool tip text.</summary>
        public string ToolTip { get; set; }

        public event PUIDelegates.OnRealize OnRealize;

        /// <summary>
        /// The action to trigger on text change. It is passed the realized source object.
        /// </summary>
        public PUIDelegates.OnTextChanged OnTextChanged { get; set; }

        /// <summary>The callback to invoke when validating input.</summary>
        public TMP_InputField.OnValidateInput OnValidate { get; set; }

        public TMP_InputField textEntry;

        public SensibleTextArea()
          : this((string)null)
        {
        }

        public SensibleTextArea(string name)
        {
            this.BackColor = PUITuning.Colors.BackgroundLight;
            this.FlexSize = Vector2.one;
            this.LineCount = 4;
            this.MaxLength = 1024;
            this.MinWidth = 64;
            this.Name = name ?? "TextArea";
            this.Text = (string)null;
            this.TextAlignment = TextAlignmentOptions.TopLeft;
            this.TextStyle = PUITuning.Fonts.TextDarkStyle;
            this.ToolTip = "";
        }

        /// <summary>Adds a handler when this text area is realized.</summary>
        /// <param name="onRealize">The handler to invoke on realization.</param>
        /// <returns>This text area for call chaining.</returns>
        public SensibleTextArea AddOnRealize(PUIDelegates.OnRealize onRealize)
        {
            this.OnRealize += onRealize;
            return this;
        }

        public GameObject Build()
        {
            GameObject ui1 = PUIElements.CreateUI((GameObject)null, this.Name);
            TextStyleSetting style = this.TextStyle ?? PUITuning.Fonts.TextLightStyle;
            Image image = ui1.AddComponent<Image>();
            image.sprite = PUITuning.Images.BoxBorderWhite;
            image.type = Image.Type.Sliced;
            image.color = style.textColor;
            GameObject ui2 = PUIElements.CreateUI(ui1, "Text Area", false);
            ui2.AddComponent<Image>().color = this.BackColor;
            RectMask2D rectMask2D = ui2.AddComponent<RectMask2D>();
            GameObject ui3 = PUIElements.CreateUI(ui2, "Text");
            TextMeshProUGUI textMeshProUgui = PTextField.ConfigureField(ui3.AddComponent<TextMeshProUGUI>(), style, this.TextAlignment);
            textMeshProUgui.enableWordWrapping = true;
            textMeshProUgui.raycastTarget = true;
            ui1.SetActive(false);
            TMP_InputField textEntry = ui1.AddComponent<TMP_InputField>();
            textEntry.textComponent = (TMP_Text)textMeshProUgui;
            textEntry.textViewport = ui2.rectTransform();
            textEntry.text = this.Text ?? "";
            textMeshProUgui.text = this.Text ?? "";
            this.ConfigureTextEntry(textEntry);
            PTextFieldEvents ptextFieldEvents = ui1.AddComponent<PTextFieldEvents>();
            ptextFieldEvents.OnTextChanged = this.OnTextChanged;
            ptextFieldEvents.OnValidate = this.OnValidate;
            ptextFieldEvents.TextObject = ui3;
            PUIElements.SetToolTip(ui1, this.ToolTip);
            rectMask2D.enabled = true;
            PUIElements.SetAnchorOffsets(ui3, new RectOffset());
            ui1.SetActive(true);
            LayoutElement layoutElement = PUIUtils.InsetChild(ui1, ui2, Vector2.one, new Vector2((float)this.MinWidth, (float)Math.Max(this.LineCount, 1) * PUIUtils.GetLineHeight(style))).AddOrGet<LayoutElement>();
            layoutElement.flexibleWidth = this.FlexSize.x;
            layoutElement.flexibleHeight = this.FlexSize.y;
            PUIDelegates.OnRealize onRealize = this.OnRealize;
            if (onRealize != null)
                onRealize(ui1);
            return ui1;
        }

        /// <summary>Sets up the text entry field.</summary>
        /// <param name="textEntry">The input field to configure.</param>
        private void ConfigureTextEntry(TMP_InputField textEntry)
        {
            textEntry.characterLimit = Math.Max(1, this.MaxLength);
            textEntry.enabled = true;
            textEntry.inputType = TMP_InputField.InputType.Standard;
            textEntry.interactable = true;
            textEntry.isRichTextEditingAllowed = false;
            textEntry.keyboardType = TouchScreenKeyboardType.Default;
            textEntry.lineType = TMP_InputField.LineType.MultiLineNewline;
            textEntry.navigation = Navigation.defaultNavigation;
            textEntry.richText = false;
            textEntry.selectionColor = PUITuning.Colors.SelectionBackground;
            textEntry.transition = Selectable.Transition.None;
            textEntry.restoreOriginalTextOnEscape = true;
        }

        /// <summary>
        /// Sets the default Klei pink style as this text area's color and text style.
        /// </summary>
        /// <returns>This button for call chaining.</returns>
        public SensibleTextArea SetKleiPinkStyle()
        {
            this.TextStyle = PUITuning.Fonts.UILightStyle;
            this.BackColor = PUITuning.Colors.ButtonPinkStyle.inactiveColor;
            return this;
        }

        /// <summary>
        /// Sets the default Klei blue style as this text area's color and text style.
        /// </summary>
        /// <returns>This button for call chaining.</returns>
        public SensibleTextArea SetKleiBlueStyle()
        {
            this.TextStyle = PUITuning.Fonts.UILightStyle;
            this.BackColor = PUITuning.Colors.ButtonBlueStyle.inactiveColor;
            return this;
        }

        /// <summary>
        /// Sets the minimum (and preferred) width of this text area in characters.
        /// 
        /// The width is computed using the currently selected text style.
        /// </summary>
        /// <param name="chars">The number of characters to be displayed.</param>
        /// <returns>This button for call chaining.</returns>
        public SensibleTextArea SetMinWidthInCharacters(int chars)
        {
            int num = Mathf.RoundToInt((float)chars * PUIUtils.GetEmWidth(this.TextStyle));
            if (num > 0)
                this.MinWidth = num;
            return this;
        }

        public override string ToString() => string.Format("SensibleTextArea[Name={0}]", (object)this.Name);
    }
}
