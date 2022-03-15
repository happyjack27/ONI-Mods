using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AutomationPlus
{
    class AluGateSideScreen : SideScreenContent
    {
        private Dictionary<AluGateOperators, string> _opCodeNames = new Dictionary<AluGateOperators, string> {
            {  AluGateOperators.none ,"None"},
            {   AluGateOperators.add,           "Add" },
            {    AluGateOperators.subtract ,     "Subtract" },
            {   AluGateOperators.modulus ,       "Modulus" },
            {     AluGateOperators.multiply ,    "Multiply" },
            {  AluGateOperators.divide ,         "Divide" },
            { AluGateOperators.exp ,             "Exponent" },
            { AluGateOperators.logicalBitLeft ,  "Bit Shift Left" },
            {  AluGateOperators.logicalBitRight ,"Bit Shift Right"},
            {  AluGateOperators.equal ,"Equal"},
            {  AluGateOperators.notEqual ,"Not Equal"},
            {  AluGateOperators.lessThan ,"Less than"},
            {  AluGateOperators.lessThanOrEqual ,"Less than or equal"},
            {  AluGateOperators.greaterThan ,"Greater than"},
            {  AluGateOperators.greaterThanOrEqual ,"Greater than or equal"},
            {  AluGateOperators.plusPlus ,"Increment by one"},
        };
        private AluGate targetAluGate;
        public AluGateSideScreen()
        {
            titleKey = "AutomationPlus.AluGateStrings.SIDESCREEN_TITLE";
        }
        private GameObject toggle;
        private GameObject combo;
        private GameObject invalidWarning;
        private GameObject lblInput1;
        private GameObject lblInput2;
        private GameObject lblOutput1;

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<AluGate>() != null;
        }

        private void BuildRow(string title, string tooltip, PUIDelegates.OnRealize onRealize)
        {
            var row = new PPanel("BitRow")
            {
                FlexSize = Vector2.right,
                Alignment = TextAnchor.MiddleCenter,
                Spacing = 10,
                Direction = PanelDirection.Horizontal,
                Margin = new RectOffset(2,2, 2, 2),
                
            }.AddChild(new PLabel("BitLabel")
            {
                TextAlignment = TextAnchor.MiddleRight,
                ToolTip = tooltip,
                Text = title,
                TextStyle = PUITuning.Fonts.TextDarkStyle
            });
            var cb = new PCheckBox
            {
                ToolTip = tooltip,
                TextStyle = PUITuning.Fonts.TextLightStyle,
                TextAlignment = TextAnchor.
                MiddleLeft,
                OnChecked = (s, e)=> OnToggleChanged(e),
            };
            cb.OnRealize += (t)=>
            {
                this.toggle = t;
                this.UpdateVisuals();
            };
            row.AddChild(cb).AddTo(gameObject);


            var row2 = new PPanel("BitRow")
            {
                FlexSize = Vector2.right,
                Alignment = TextAnchor.MiddleCenter,
                Spacing = 10,
                Direction = PanelDirection.Horizontal,
                Margin = new RectOffset(8, 8, 8, 8)
            }.AddChild(new PLabel("BitLabel")
            {
                TextAlignment = TextAnchor.MiddleRight,
                ToolTip = tooltip,
                Text = "Operator",
                TextStyle = PUITuning.Fonts.TextDarkStyle
            });
            var comboBox = new PComboBox<ListOption>
            {
                Content = _opCodeNames.Values.Select(p => new ListOption(p)),
                InitialItem = "Add",
                ToolTip = "Operator Code",
                TextStyle = PUITuning.Fonts.TextLightStyle,
                TextAlignment = TextAnchor.
                MiddleLeft,
                OnOptionSelected = (s, e) => SetOperator(e),
                MinWidth = 150,
            };
            comboBox.OnRealize += (t) =>
            {
                this.combo = t;
                this.UpdateVisuals();
            }; ;
            row2.AddChild(comboBox).AddTo(gameObject);
            var row3 = new PPanel("StatusRow").AddTo(gameObject);


             lblInput1 = new PLabel("lblInput1")
            {
                TextAlignment = TextAnchor.MiddleRight,
                ToolTip = tooltip,
                Text = " ",
                TextStyle = PUITuning.Fonts.TextDarkStyle
            }.AddTo(row3); 

             lblInput2 = new PLabel("lblInput1")
            {
                TextAlignment = TextAnchor.MiddleRight,
                ToolTip = tooltip,
                Text = " ",
                TextStyle = PUITuning.Fonts.TextDarkStyle
            }.AddTo(row3);

            lblOutput1 = new PLabel("lblInput1")
            {
                TextAlignment = TextAnchor.MiddleRight,
                ToolTip = tooltip,
                Text = " ",
                TextStyle = PUITuning.Fonts.TextDarkStyle
            }.AddTo(row3);

            var defaultStyle = PUITuning.Fonts.UIDarkStyle;
            var errorStyle = ScriptableObject.CreateInstance<TextStyleSetting>();
            errorStyle.enableWordWrapping = false;
            errorStyle.fontSize = defaultStyle.fontSize;
            errorStyle.sdfFont = defaultStyle.sdfFont;
            errorStyle.textColor = Color.red;
            invalidWarning = new PLabel("InvalidWarning")
            {
                Text = " ",
                TextAlignment = TextAnchor.MiddleCenter,
                Margin = new RectOffset(2,2,2,2),
                TextStyle = errorStyle
            }.AddTo(gameObject);
        }

        protected void SetOperator(ListOption e) 
        {
            foreach (var kvp in this._opCodeNames)
            {
                if(kvp.Value == e.GetProperName())
                {
                    this.targetAluGate.opCode = kvp.Key;
                    break;
                }
            }
            
            this.UpdateVisuals();
        }

        protected override void OnPrefabInit()
        {
            var baseLayout = gameObject.GetComponent<BoxLayoutGroup>();
            var margin = new RectOffset(4, 4, 4, 4);
            if (baseLayout != null)
                baseLayout.Params = new BoxLayoutParams()
                {
                    Margin = margin,
                    Direction = PanelDirection.Vertical,
                    Alignment =
                    TextAnchor.UpperCenter,
                    Spacing = 8
                };
            BuildRow("Signed Ints?", "Determines if the number system includes a sign bit", (t) =>
            {
                this.toggle = t;
                this.UpdateVisuals();
            });
            ContentContainer = gameObject;
            base.OnPrefabInit();
            UpdateVisuals();
        }

        private void OnToggleChanged(int newValue)
        {
            this.targetAluGate.isTwosComplement = newValue != PCheckBox.STATE_CHECKED;
            this.UpdateVisuals();
        }

        public override void SetTarget(GameObject target)
        {

            base.SetTarget(target);
            this.targetAluGate = target.GetComponent<AluGate>();
            this.UpdateVisuals();
        }

        private void checkOp()
        {
            if(invalidWarning != null && targetAluGate != null)
            {
                var valid = !this.targetAluGate.isOpCodeConnected();
                
                PUIElements.SetText(invalidWarning, valid ? " " : "Operator Cable Connected \n setting will be ignored");
            }

        }

        private void UpdateVisuals()
        {
            if (this.toggle != null && this.targetAluGate != null)
            {
                checkOp();
                PCheckBox.SetCheckState(this.toggle, targetAluGate.isTwosComplement ? PCheckBox.STATE_CHECKED: PCheckBox.STATE_UNCHECKED);
                var opCode = targetAluGate.GetOpCode();
                if(!this._opCodeNames.ContainsKey(opCode))
                {
                    opCode = AluGateOperators.none;
                }
                PComboBox<ListOption>.SetSelectedItem(this.combo, new ListOption(this._opCodeNames[opCode]));
                var values = targetAluGate.getValues();
                if(lblInput1)
                    PUIElements.SetText(lblInput1, $"Input 1: {values.inputValue1}");
                if (lblInput2)
                    PUIElements.SetText(lblInput2, $"Input 2: {values.inputValue2}");
                if (lblOutput1)
                    PUIElements.SetText(lblOutput1, $"Output: {values.outputValue}");
            }
        }
    }
}
