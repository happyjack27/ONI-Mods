using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;
using static LogicPorts;

namespace AutomationPlus
{
    //enum AluGateOperators
    //{
    //    none = 0x0,
    //    add = 0x1,
    //    subtract = 0x2,
    //    multiply = 0x4,
    //    modulus = 0x5,
    //    exp = 0x6,
    //    divide = 0x8,
    //    logicalBitRight = 0xD,
    //    logicalBitLeft = 0xE, //(6) (==, !=, <=, >= , <, >)
    //}

    enum AluGateOperators
    {
        none = 0x0,
        add = 0x1,
        subtract = 0x2,
        multiply = 0x3,
        modulus = 0x4,
        exp = 0x5,
        divide = 0x6,
        logicalBitRight = 0x7,
        logicalBitLeft = 0x8,
        equal = 0x9,
        notEqual = 0xA,
        lessThan = 0xB,
        greaterThan = 0xC,
        lessThanOrEqual = 0xD,
        greaterThanOrEqual = 0xE,
        plusPlus = 0xF,
    }

    class BinaryFormatter : IFormatProvider, ICustomFormatter
    {
        private bool isTwosComplement;
        public BinaryFormatter(bool isTwosComplement)
        {
            this.isTwosComplement = isTwosComplement;
        }
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is int))
            {
                if (!string.IsNullOrEmpty(format))
                {
                    return string.Format(format, arg);
                }
                else
                {
                    return arg.ToString();
                }
            }
            // Check whether this is an appropriate callback
            if (!this.Equals(formatProvider))
                return null;
            var components = (format ?? "D").Split(':');
            var primary = components[0];
            var bits = 4;
            if (components.Length > 1)
            {
                bits = Convert.ToInt32(components[1]);
            }

            string numericString = arg.ToString();
            int value = (int)arg;
            var normalValue = value;
            if (isTwosComplement)
            {
                normalValue = BinaryUtils.GetValue(value, bits);
            }
            if (primary == "H")
            {
                return "0x" + Convert.ToString(value, 16);
            }
            if (primary == "D")
            {
                return normalValue.ToString();
            }
            else if (primary == "B")
            {
                var binaryValue = "";
                for (int i = 1; i <= bits; i++)
                {
                    var bit = (1 << (bits - i));
                    var hasBit = (value & bit) == bit;
                    if (hasBit)
                    {
                        binaryValue += "1";
                    }
                    else
                    {
                        binaryValue += "0";
                    }

                }
                return binaryValue;
            }
            else if (primary == "F")
            {
                return $"{this.Format("D", arg, formatProvider)} ({this.Format($"B:{bits}", arg, formatProvider)})";
            }
            return numericString;
        }
    }

    class BinaryUtils
    {
        private static Dictionary<int, int> _minValues = new Dictionary<int, int>()
        {
            {4, 1 << 3 },
            {8, 1 << 7 }
        };

        public static bool IsNegative(int value, int bits)
        {
            var minValue = _minValues[bits];
            var isNegative = (value & minValue) == minValue;
            return isNegative;
        }

        public static int GetTwosComplement(int value, int bits)
        {
            var minValue = _minValues[bits];
            if (IsNegative(value, bits))
            {
                return (value ^ minValue) + 1;
            }
            else
            {
                return value;
            }
        }

        public static int GetValue(int value, int bits)
        {
            var minValue = _minValues[bits];
            if (IsNegative(value, bits))
            {
                return -((value ^ minValue) + 1);
            }
            else
            {
                return value;
            }
        }
    }

    struct AluValues
    {
        public string inputValue1;
        public string inputValue2;
        public string outputValue;
    }

    class AluGate : KMonoBehaviour
    {
        [MyCmpAdd]
        private CopyBuildingSettings copyBuildingSettings;
        public static readonly HashedString INPUT_PORT_ID1 = new HashedString("AluGateInput1");
        public static readonly HashedString INPUT_PORT_ID2 = new HashedString("AluGateInput2");
        public static readonly HashedString OUTPUT_PORT_ID = new HashedString("AluGateOutput");
        public static readonly HashedString OP_PORT_ID = new HashedString("AluGateOpCode");

        public event EventHandler ValueChanged;
        private KBatchedAnimController kbac;

        [Serialize]
        protected int lhs;
        [Serialize]
        protected int rhs;
        [Serialize]
        private AluGateOperators _inputOpCode;

        [Serialize]
        protected int currentValue = 0;
        [Serialize]
        protected bool needsToggleRefresh = true;
        [Serialize]
        private AluGateOperators _opCode = AluGateOperators.none;
        public AluGateOperators opCode
        {
            get
            {
                return _opCode;
            }
            set
            {
                _opCode = value;
                this.RecalcValues();
                RefreshAnimations();
            }
        }

        private Color activeTintColor = new Color(137 / 255f, 252 / 255f, 76 / 255f);
        private Color inactiveTintColor = Color.red;
        private Color disabledColor = new Color(79 / 255f, 93 / 255f, 71 / 255f); //#4F5D47

        protected LogicPorts ports;

        public AluGate() : base()
        {

        }

        protected int maxValue = 0xf;
        protected int bits = 4;
        [Serialize]
        private bool _isTwosComplement;

        public bool isTwosComplement
        {
            get { return _isTwosComplement; }
            set
            {
                _isTwosComplement = value;
                this.RecalcValues();
                RefreshAnimations();
            }
        }




        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<AluGate>(-905833192, (IntraObjectHandler<AluGate>)((comp, data) => comp.OnCopySettings(data)));
        }

        private void OnCopySettings(object data)
        {
            AluGate component = ((GameObject)data).GetComponent<AluGate>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
            this.isTwosComplement = component.isTwosComplement;
            this.opCode = component.opCode;
        }

        public bool isOpCodeConnected()
        {
            return ports.IsPortConnected(AluGate.OP_PORT_ID);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Action<AluGate, object> p = (c, d) => c.OnLogicValueChanged(d);
            this.kbac = this.GetComponent<KBatchedAnimController>();
            this.kbac.Play((HashedString)"off");
            //this.kbac.Play("on_0");
            this.Subscribe<AluGate>(-801688580, p);
            this.ports = this.GetComponent<LogicPorts>();
            Port port;
            bool thing;
            this.ports.TryGetPortAtCell(this.ports.GetPortCell(AluGate.OP_PORT_ID), out port, out thing);
            port.requiresConnection = false;
            RefreshAnimations();
        }

        public virtual int GetInputValue1()
        {
            return this.ports?.GetInputValue(AluGate.INPUT_PORT_ID1) ?? 0;
        }

        public virtual int GetInputValue2()
        {
            return this.ports?.GetInputValue(AluGate.INPUT_PORT_ID2) ?? 0;
        }

        public AluValues getValues()
        {
            var formatString = $"{{0:F:{this.bits}}}";
            var formatter = new BinaryFormatter(isTwosComplement);
            return new AluValues
            {
                inputValue1 = String.Format(formatter, formatString, GetInputValue1()),
                inputValue2 = String.Format(formatter, formatString, GetInputValue2()),
                outputValue = String.Format(formatter, formatString, currentValue),
            };
        }


        private int getTwosComplement(int value)
        {
            return BinaryUtils.GetTwosComplement(value, bits);
        }

        public AluGateOperators GetOpCode()
        {

            var input = this.ports?.GetInputValue(AluGate.OP_PORT_ID);
            if (input == null || !this.isOpCodeConnected())
            {
                return this.opCode;
            }
            return (AluGateOperators)input;
        }

        public int checkOverflow(int value)
        {
            while (value > maxValue)
            {
                value -= (maxValue + 1);
            }
            while (value < 0)
            {
                value += maxValue;
            }
            return value;
        }

        protected virtual void UpdateValue()
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, EventArgs.Empty);
            }
            this.GetComponent<LogicPorts>().SendSignal(AluGate.OUTPUT_PORT_ID, currentValue);
        }

        protected virtual bool HasPort(HashedString portId)
        {
            return portId == AluGate.INPUT_PORT_ID1
                || portId == AluGate.INPUT_PORT_ID2
                || portId == AluGate.OP_PORT_ID;

        }
        protected virtual bool HasValueChanged()
        {
            var currentLhs = GetInputValue1();
            var currentRhs = GetInputValue2();
            var currentOp = GetOpCode();

            return currentLhs != lhs || currentRhs != rhs || currentOp != _inputOpCode;
        }

        protected virtual bool ShouldRecalcValue(LogicValueChanged logicValueChanged)
        {
            return !(logicValueChanged.portID == AluGate.OUTPUT_PORT_ID);
        }

        public void OnLogicValueChanged(object data)
        {
            LogicValueChanged logicValueChanged = (LogicValueChanged)data;

            if (!this.HasPort(logicValueChanged.portID))
            {
                return;
            }
            if (!HasValueChanged() && !this.needsToggleRefresh && IsOnAnimation())
            {
                return;
            }
            this.needsToggleRefresh = true;

            if (!ShouldRecalcValue(logicValueChanged))
            {
                RefreshAnimations();
                return;
            }
            RecalcValues();
            RefreshAnimations();

        }

        protected virtual bool IsOnAnimation()
        {
            var nw1 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.INPUT_PORT_ID1));
            var nw2 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.INPUT_PORT_ID2));
            var nwOut = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.OUTPUT_PORT_ID));
            return (nw1 != null && nw2 != null && nwOut != null);
        }


        private void RefreshAnimations()
        {
            var nw1 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.INPUT_PORT_ID1));
            var nw2 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.INPUT_PORT_ID2));
            var nwOp = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.OP_PORT_ID));
            var nwOut = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.OUTPUT_PORT_ID));

            if (false)
            {

                this.kbac.Play("on");


                this.TintSymbolConditionally(nwOp, () => nwOp.OutputValue > 0, this.kbac, "light5_bloom");

                this.TintSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(3, currentValue), this.kbac, "light1_bloom"); //12
                this.TintSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(2, currentValue), this.kbac, "light2_bloom");
                this.TintSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(1, currentValue), this.kbac, "light3_bloom");
                this.TintSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(0, currentValue), this.kbac, "light4_bloom"); // 9

                this.TintSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(3, rhs), this.kbac, "light6_bloom");  // 7
                this.TintSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(2, rhs), this.kbac, "light7_bloom");
                this.TintSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(1, rhs), this.kbac, "light8_bloom");
                this.TintSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(0, rhs), this.kbac, "light9_bloom");  // 4

                this.TintSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(3, lhs), this.kbac, "light10_bloom");  // 3
                this.TintSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(2, lhs), this.kbac, "light11_bloom");
                this.TintSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(1, lhs), this.kbac, "light12_bloom");
                this.TintSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(0, lhs), this.kbac, "light13_bloom");  // 0
            }
            else
            {
                if (this.IsOnAnimation())
                {
                    this.kbac.Play("on_0");

                    ToggleBlooms();

                    DisplayOperator();
                    this.needsToggleRefresh = false;

                }
                else
                {
                    this.kbac.Play("off");
                }
            }


        }

        protected virtual void ToggleBlooms()
        {
            var nw1 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.INPUT_PORT_ID1));
            var nw2 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.INPUT_PORT_ID2));
            var nwOp = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.OP_PORT_ID));
            var nwOut = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(AluGate.OUTPUT_PORT_ID));
            ShowSymbolConditionally(nwOp, () => nwOp.OutputValue > 0, $"light{8}_bloom_green", $"light{8}_bloom_red");

            ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(3, currentValue), $"light{12}_bloom_green", $"light{12}_bloom_red");
            ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(2, currentValue), $"light{11}_bloom_green", $"light{11}_bloom_red");
            ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(1, currentValue), $"light{10}_bloom_green", $"light{10}_bloom_red");
            ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(0, currentValue), $"light{9}_bloom_green", $"light{9}_bloom_red");

            ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(3, rhs), $"light{7}_bloom_green", $"light{7}_bloom_red");
            ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(2, rhs), $"light{6}_bloom_green", $"light{6}_bloom_red");
            ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(1, rhs), $"light{5}_bloom_green", $"light{5}_bloom_red");
            ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(0, rhs), $"light{4}_bloom_green", $"light{4}_bloom_red");

            ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(3, lhs), $"light{3}_bloom_green", $"light{3}_bloom_red");
            ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(2, lhs), $"light{2}_bloom_green", $"light{2}_bloom_red");
            ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(1, lhs), $"light{1}_bloom_green", $"light{1}_bloom_red");
            ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(0, lhs), $"light{0}_bloom_green", $"light{0}_bloom_red");
        }

        private void DisplayOperator()
        {
            ToggleOperator(_inputOpCode == AluGateOperators.add, "op_add");
            ToggleOperator(_inputOpCode == AluGateOperators.divide, "op_div");
            ToggleOperator(_inputOpCode == AluGateOperators.exp, "op_exp");
            ToggleOperator(_inputOpCode == AluGateOperators.logicalBitLeft, "op_bits_left");
            ToggleOperator(_inputOpCode == AluGateOperators.logicalBitRight, "op_bits_right");
            ToggleOperator(_inputOpCode == AluGateOperators.modulus, "op_mod");
            ToggleOperator(_inputOpCode == AluGateOperators.multiply, "op_mul");
            ToggleOperator(_inputOpCode == AluGateOperators.subtract, "op_minus");
            ToggleOperator(_inputOpCode == AluGateOperators.lessThan, "op_less");
            ToggleOperator(_inputOpCode == AluGateOperators.lessThanOrEqual, "op_lessThanOrEqual");
            ToggleOperator(_inputOpCode == AluGateOperators.greaterThan, "op_more");
            ToggleOperator(_inputOpCode == AluGateOperators.greaterThanOrEqual, "op_moreThanOrEqual");
            ToggleOperator(_inputOpCode == AluGateOperators.plusPlus, "op_addadd");
            ToggleOperator(_inputOpCode == AluGateOperators.equal, "op_equality");
            ToggleOperator(_inputOpCode == AluGateOperators.notEqual, "op_not_equality");

        }

        private void ToggleOperator(
          bool isOperator,
          KAnimHashedString anim)
        {
            kbac.SetSymbolVisiblity(anim, isOperator);
        }

        private void RecalcValues()
        {
            lhs = this.GetInputValue1();
            rhs = this.GetInputValue2();
            this._inputOpCode = this.GetOpCode();
            var oldValue = currentValue;
            currentValue = 0;
            switch (_inputOpCode)
            {
                case AluGateOperators.add:
                    currentValue = lhs + rhs;
                    break;
                case AluGateOperators.subtract:
                    if (isTwosComplement)
                    {
                        var twosComplement = getTwosComplement(rhs);
                        currentValue = lhs + twosComplement;
                    }
                    else
                    {
                        currentValue = lhs - rhs;
                    }
                    break;
                case AluGateOperators.multiply:
                    currentValue = lhs * rhs;
                    break;
                case AluGateOperators.modulus:
                    if (rhs != 0)
                    {
                        currentValue = lhs % rhs;
                    }
                    else
                    {
                        currentValue = 0;
                    }
                    break;
                case AluGateOperators.exp:
                    currentValue = (int)Math.Pow(lhs, rhs);
                    break;
                case AluGateOperators.divide:
                    if (rhs != 0)
                    {
                        currentValue = lhs / rhs;
                    }
                    else
                    {
                        currentValue = 0;
                    }
                    break;
                case AluGateOperators.logicalBitRight:
                    currentValue = lhs >> rhs;
                    break;
                case AluGateOperators.logicalBitLeft:
                    currentValue = lhs << rhs;
                    break;
                case AluGateOperators.equal:
                    currentValue = lhs == rhs ? 1 : 0;
                    break;
                case AluGateOperators.notEqual:
                    currentValue = lhs != rhs ? 1 : 0;
                    break;
                case AluGateOperators.lessThan:
                    currentValue = lhs < rhs ? 1 : 0;
                    break;
                case AluGateOperators.lessThanOrEqual:
                    currentValue = lhs <= rhs ? 1 : 0;
                    break;
                case AluGateOperators.greaterThan:
                    currentValue = lhs > rhs ? 1 : 0;
                    break;
                case AluGateOperators.greaterThanOrEqual:
                    currentValue = lhs >= rhs ? 1 : 0;
                    break;
                case AluGateOperators.plusPlus:
                    currentValue = lhs + 1;
                    break;
                case AluGateOperators.none:
                default:
                    break;
            }
            // reduce the values
            currentValue = checkOverflow(currentValue);
            if (oldValue != currentValue)
            {
                this.UpdateValue();
            }
        }



        protected void ShowSymbolConditionally(
            object nw,
          Func<bool> active,
          KAnimHashedString ifTrue,
          KAnimHashedString ifFalse)
        {
            var connected = nw != null;
            kbac.SetSymbolVisiblity(ifTrue, connected && active());
            kbac.SetSymbolVisiblity(ifFalse, connected && !active());
        }

        private void TintSymbolConditionally(
          object tintAnything,
          Func<bool> condition,
          KBatchedAnimController kbac,
          KAnimHashedString symbol
         )
        {
            if (tintAnything != null)
                kbac.SetSymbolTint(symbol, condition() ? activeTintColor : inactiveTintColor);
            else
                kbac.SetSymbolTint(symbol, disabledColor);
        }

        private void SetBloomSymbolShowing(
          bool showing,
          KBatchedAnimController kbac,
          KAnimHashedString symbol,
          KAnimHashedString bloomSymbol)
        {
            kbac.SetSymbolVisiblity(bloomSymbol, showing);
            kbac.SetSymbolVisiblity(symbol, !showing);
        }
    }


}
