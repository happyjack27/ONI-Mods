// Decompiled with JetBrains decompiler
// Type: PeterHan.PLib.UI.SensibleTextField
// Assembly: PLib, Version=4.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 642DAF9A-7503-46F0-9C39-02EBD9828B7C
// Assembly location: C:\Users\kbaas\AppData\Local\Temp\Hulamep\c84f4254ab\lib\net471\PLib.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PeterHan.PLib.UI
{
    /// <summary>A custom UI text field factory class.</summary>
    public sealed class SensibleTextField : IUIComponent
    {
        /// <summary>Configures a Text Mesh Pro field.</summary>
        /// <param name="component">The text component to configure.</param>
        /// <param name="style">The desired text color, font, and style.</param>
        /// <param name="alignment">The text alignment.</param>
        /// <returns>The component, for call chaining.</returns>
        internal static TextMeshProUGUI ConfigureField(
          TextMeshProUGUI component,
          TextStyleSetting style,
          TextAlignmentOptions alignment)
        {
            component.alignment = alignment;
            component.autoSizeTextContainer = false;
            component.enabled = true;
            component.color = style.textColor;
            component.font = style.sdfFont;
            component.fontSize = (float)style.fontSize;
            component.fontStyle = style.style;
            component.overflowMode = TextOverflowModes.Overflow;
            return component;
        }

        /// <summary>Gets a text field's text.</summary>
        /// <param name="textField">The UI element to retrieve.</param>
        /// <returns>The current text in the field.</returns>
        public static string GetText(GameObject textField)
        {
            if ((UnityEngine.Object)textField == (UnityEngine.Object)null)
                throw new ArgumentNullException(nameof(textField));
            return textField.GetComponent<TMP_InputField>()?.text;
        }

        /// <summary>The text field's background color.</summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// Retrieves the built-in field type used for Text Mesh Pro.
        /// </summary>
        private TMP_InputField.ContentType ContentType
        {
            get
            {
                TMP_InputField.ContentType contentType;
                switch (this.Type)
                {
                    case SensibleTextField.FieldType.Integer:
                        contentType = TMP_InputField.ContentType.IntegerNumber;
                        break;
                    case SensibleTextField.FieldType.Float:
                        contentType = TMP_InputField.ContentType.DecimalNumber;
                        break;
                    default:
                        contentType = TMP_InputField.ContentType.Standard;
                        break;
                }
                return contentType;
            }
        }

        /// <summary>The flexible size bounds of this component.</summary>
        public Vector2 FlexSize { get; set; }

        /// <summary>The maximum number of characters in this text field.</summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// The minimum width in units (not characters!) of this text field.
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>
        /// The placeholder text style (including color, font, and word wrap settings) if the
        /// field is empty.
        /// </summary>
        public TextStyleSetting PlaceholderStyle { get; set; }

        /// <summary>The placeholder text if the field is empty.</summary>
        public string PlaceholderText { get; set; }

        public string Name { get; }

        /// <summary>The text alignment in the text field.</summary>
        public TextAlignmentOptions TextAlignment { get; set; }

        /// <summary>The initial text in the text field.</summary>
        public string Text { get; set; }

        /// <summary>
        /// The text field's text color, font, word wrap settings, and font size.
        /// </summary>
        public TextStyleSetting TextStyle { get; set; }

        /// <summary>The tool tip text.</summary>
        public string ToolTip { get; set; }

        /// <summary>The field type.</summary>
        public SensibleTextField.FieldType Type { get; set; }

        public event PUIDelegates.OnRealize OnRealize;

        /// <summary>
        /// The action to trigger on text change. It is passed the realized source object.
        /// </summary>
        public PUIDelegates.OnTextChanged OnTextChanged { get; set; }

        /// <summary>The callback to invoke when validating input.</summary>
        public TMP_InputField.OnValidateInput OnValidate { get; set; }

        public SensibleTextField()
          : this((string)null)
        {
        }

        public SensibleTextField(string name)
        {
            this.BackColor = PUITuning.Colors.BackgroundLight;
            this.FlexSize = Vector2.zero;
            this.MaxLength = 256;
            this.MinWidth = 32;
            this.Name = name ?? "TextField";
            this.PlaceholderText = (string)null;
            this.Text = (string)null;
            this.TextAlignment = TextAlignmentOptions.Center;
            this.TextStyle = PUITuning.Fonts.TextDarkStyle;
            this.PlaceholderStyle = this.TextStyle;
            this.ToolTip = "";
            this.Type = SensibleTextField.FieldType.Text;
        }

        /// <summary>Adds a handler when this text field is realized.</summary>
        /// <param name="onRealize">The handler to invoke on realization.</param>
        /// <returns>This text field for call chaining.</returns>
        public SensibleTextField AddOnRealize(PUIDelegates.OnRealize onRealize)
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
            TextMeshProUGUI textMeshProUgui1 = SensibleTextField.ConfigureField(ui3.AddComponent<TextMeshProUGUI>(), style, this.TextAlignment);
            textMeshProUgui1.enableWordWrapping = false;
            textMeshProUgui1.maxVisibleLines = 1;
            textMeshProUgui1.raycastTarget = true;
            ui1.SetActive(false);
            TMP_InputField textEntry = ui1.AddComponent<TMP_InputField>();
            textEntry.textComponent = (TMP_Text)textMeshProUgui1;
            textEntry.textViewport = ui2.rectTransform();
            textEntry.text = this.Text ?? "";
            textMeshProUgui1.text = this.Text ?? "";
            if (this.PlaceholderText != null)
            {
                TextMeshProUGUI textMeshProUgui2 = SensibleTextField.ConfigureField(PUIElements.CreateUI(ui2, "Placeholder Text").AddComponent<TextMeshProUGUI>(), this.PlaceholderStyle ?? style, this.TextAlignment);
                textMeshProUgui2.maxVisibleLines = 1;
                textMeshProUgui2.text = this.PlaceholderText;
                textEntry.placeholder = (Graphic)textMeshProUgui2;
            }
            this.ConfigureTextEntry(textEntry);
            SensibleTextFieldEvents SensibleTextFieldEvents = ui1.AddComponent<SensibleTextFieldEvents>();
            SensibleTextFieldEvents.OnTextChanged = this.OnTextChanged;
            SensibleTextFieldEvents.OnValidate = this.OnValidate;
            SensibleTextFieldEvents.TextObject = ui3;
            PUIElements.SetToolTip(ui1, this.ToolTip);
            rectMask2D.enabled = true;
            PUIElements.SetAnchorOffsets(ui3, new RectOffset());
            ui1.SetActive(true);
            RectTransform rectTransform = ui3.rectTransform();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            LayoutElement layoutElement = PUIUtils.InsetChild(ui1, ui2, Vector2.one, new Vector2((float)this.MinWidth, LayoutUtility.GetPreferredHeight(rectTransform))).AddOrGet<LayoutElement>();
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
            textEntry.contentType = this.ContentType;
            textEntry.enabled = true;
            textEntry.inputType = TMP_InputField.InputType.Standard;
            textEntry.interactable = true;
            textEntry.isRichTextEditingAllowed = false;
            textEntry.keyboardType = TouchScreenKeyboardType.Default;
            textEntry.lineType = TMP_InputField.LineType.SingleLine;
            textEntry.navigation = Navigation.defaultNavigation;
            textEntry.richText = false;
            textEntry.selectionColor = PUITuning.Colors.SelectionBackground;
            textEntry.transition = Selectable.Transition.None;
            textEntry.restoreOriginalTextOnEscape = true;
        }

        /// <summary>
        /// Sets the default Klei pink style as this text field's color and text style.
        /// </summary>
        /// <returns>This text field for call chaining.</returns>
        public SensibleTextField SetKleiPinkStyle()
        {
            this.TextStyle = PUITuning.Fonts.UILightStyle;
            this.BackColor = PUITuning.Colors.ButtonPinkStyle.inactiveColor;
            return this;
        }

        /// <summary>
        /// Sets the default Klei blue style as this text field's color and text style.
        /// </summary>
        /// <returns>This text field for call chaining.</returns>
        public SensibleTextField SetKleiBlueStyle()
        {
            this.TextStyle = PUITuning.Fonts.UILightStyle;
            this.BackColor = PUITuning.Colors.ButtonBlueStyle.inactiveColor;
            return this;
        }

        /// <summary>
        /// Sets the minimum (and preferred) width of this text field in characters.
        /// 
        /// The width is computed using the currently selected text style.
        /// </summary>
        /// <param name="chars">The number of characters to be displayed.</param>
        /// <returns>This text field for call chaining.</returns>
        public SensibleTextField SetMinWidthInCharacters(int chars)
        {
            int num = Mathf.RoundToInt((float)chars * PUIUtils.GetEmWidth(this.TextStyle));
            if (num > 0)
                this.MinWidth = num;
            return this;
        }

        public override string ToString() => string.Format("SensibleTextField[Name={0},Type={1}]", (object)this.Name, (object)this.Type);

        /// <summary>The valid text field types supported by this class.</summary>
        public enum FieldType
        {
            Text,
            Integer,
            Float,
        }
    }
}
