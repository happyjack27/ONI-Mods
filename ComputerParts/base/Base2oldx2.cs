using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;
using static LogicPorts;

namespace KBComputing.baseClasses
{

    abstract class Base2x2 : KMonoBehaviour, ILogicEventSender, ILogicNetworkConnection, IRenderEveryTick
    {
        [MyCmpAdd]
        private CopyBuildingSettings copyBuildingSettings;
        public static readonly HashedString INPUT_PORT_ID1 = new HashedString("AluGateInput1");
        public static readonly HashedString OUTPUT_PORT_ID1 = new HashedString("AluGateOutput1");
        public static readonly HashedString CONTROL_PORT_ID_READ = new HashedString("AluGateRead");
        public static readonly HashedString CONTROL_PORT_ID_WRITE = new HashedString("AluGateWrite");

        protected static readonly EventSystem.IntraObjectHandler<Base2x2> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Base2x2>((component, data) => component.OnLogicValueChanged(data));
        protected LogicPorts ports;
        protected KBatchedAnimController kbac;

        [Serialize]
        protected int inputValue1 = 0;
        [Serialize]
        protected int storedValue1 = 0;
        [Serialize]
        protected int outputValue1 = 0;
        [Serialize]
        protected int controlPort1 = 0;
        [Serialize]
        protected int controlPort2 = 0;

        //private Color activeTintColor = new Color(137 / 255f, 252 / 255f, 76 / 255f);
        //private Color inactiveTintColor = Color.red;
        //private Color disabledColor = new Color(79 / 255f, 93 / 255f, 71 / 255f); //#4F5D47

        public Base2x2() : base()
        {

        }

        protected int maxValue = 0xf;
        protected int bits = 4;
        private bool connected;
        private LogicPortVisualizer outputOne;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<Base2x2>(-905833192, (IntraObjectHandler<Base2x2>)((comp, data) => comp.OnCopySettings(data)));
        }

        private void OnCopySettings(object data)
        {
            Base2x2 component = ((GameObject)data).GetComponent<Base2x2>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
        }


        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Subscribe<Base2x2>(-801688580, Base2x2.OnLogicValueChangedDelegate);
            this.ports = this.GetComponent<LogicPorts>();
            this.kbac = this.GetComponent<KBatchedAnimController>();
            this.kbac.Play((HashedString)"off");
            Connect();

            Port port;
            bool thing;
            this.ports.TryGetPortAtCell(this.ports.GetPortCell(Base2x2.CONTROL_PORT_ID_READ), out port, out thing);
            port.requiresConnection = false;
            this.ports.TryGetPortAtCell(this.ports.GetPortCell(Base2x2.CONTROL_PORT_ID_WRITE), out port, out thing);
            port.requiresConnection = false;

            RefreshAnimations();
        }
        /*
         */
        protected virtual bool IsOnAnimation()
        {
            var nw1 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base2x2.INPUT_PORT_ID1));
            var nw2 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base2x2.OUTPUT_PORT_ID1));
            return (nw1 != null && nw2 != null);
        }


        private void RefreshAnimations()
        {
            if (this.IsOnAnimation())
            {
                this.kbac.Play("on_0");

                //ToggleBlooms();
            }
            else
            {
                this.kbac.Play("off");
            }
            ToggleBlooms();
        }



        private int GetRibbonValue(int wire)
        {
            if (wire == 0)
            {
                return 0;
            }
            else if (wire == 0b1111)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        private int GetSingleValue(int wire)
        {
            return wire & 0b1;
        }

        public void UpdateVisuals()
        {
            // when there is not an output, we are supposed to play the off animation
            /*
            if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(GetActualCell(OUTPUTOFFSET)) is LogicCircuitNetwork))
            {
                kbac.Play("off", KAnim.PlayMode.Once, 1f, 0.0f);
                return;
            }
            */
            int bit0 = 0, bit1 = 0, bit2 = 0, bit3 = 0;
            bit0 = inputValue1 == 0x0F ? 2 : inputValue1 == 0x00 ? 0 : 1;
            bit1 = controlPort1 == 0x0F ? 2 : controlPort1 == 0x00 ? 0 : 1;
            bit2 = controlPort2 == 0x0F ? 2 : controlPort2 == 0x00 ? 0 : 1;
            bit3 = outputValue1 == 0x0F ? 2 : outputValue1 == 0x00 ? 0 : 1;
            kbac.Play("on_" + (bit0 + 3 * bit1 + 6 * bit2 + 12 * bit3), KAnim.PlayMode.Once, 1f, 0.0f);
        }

        

        private void ToggleOperator(
          bool isOperator,
          KAnimHashedString anim)
        {
            kbac.SetSymbolVisiblity(anim, isOperator);
        }

        public void LogicTick()
        {
            inputValue1 = this.ports?.GetInputValue(Base2x2.INPUT_PORT_ID1) ?? 0;
            controlPort1 = this.ports?.GetInputValue(Base2x2.CONTROL_PORT_ID_WRITE) ?? 1;
            controlPort2 = this.ports?.GetInputValue(Base2x2.CONTROL_PORT_ID_READ) ?? 1;
            var lastout1 = outputValue1;

            storedValue1 = (controlPort1 == 1) ? inputValue1 : storedValue1;

            outputValue1 = (controlPort2 == 1) ? storedValue1 : 0;

            if (lastout1 != outputValue1)//} || lastout2 != outputValue2)
            {
                this.GetComponent<LogicPorts>().SendSignal(Base2x2.OUTPUT_PORT_ID1, outputValue1);
            }
            UpdateVisuals();
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
        public int GetLogicCell()
        {
            return this.ports.GetPortCell(OUTPUT_PORT_ID1);
        }

        public int GetLogicValue()
        {
            return this.outputValue1;
        }

        public int GetInputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetInputValue(INPUT_PORT_ID1) ?? 0;
        }

        public int GetOutputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetOutputValue(OUTPUT_PORT_ID1) ?? 0;
        }


        protected void Connect()
        {
            var logicCircuitSystem = Game.Instance.logicCircuitSystem;
            var logicCircuitManager = Game.Instance.logicCircuitManager;

            if (this.connected)
            {
                return;
            }
            this.connected = true;

            int outputCellOne = this.ports.GetPortCell(OUTPUT_PORT_ID1);
            logicCircuitSystem.AddToNetworks(outputCellOne, this, true);

            this.outputOne = new LogicPortVisualizer(outputCellOne, LogicPortSpriteType.Output);
            logicCircuitManager.AddVisElem((ILogicUIElement)this.outputOne);

            LogicCircuitManager.ToggleNoWireConnected(false, this.gameObject);
        }

        protected void Disconnect()
        {
            if (!this.connected)
                return;
            var logicCircuitSystem = Game.Instance.logicCircuitSystem;
            var logicCircuitManager = Game.Instance.logicCircuitManager;
            this.connected = false;
            int outputCellOne = this.ports.GetPortCell(OUTPUT_PORT_ID1);
            logicCircuitSystem.RemoveFromNetworks(outputCellOne, (object)this, true);
            logicCircuitManager.RemoveVisElem((ILogicUIElement)this.outputOne);
            this.outputOne = (LogicPortVisualizer)null;
        }

        protected LogicCircuitNetwork GetInputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)null;
            if ((UnityEngine.Object)this.ports != (UnityEngine.Object)null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base2x2.INPUT_PORT_ID1));
            return logicCircuitNetwork;
        }

        protected LogicCircuitNetwork GetOutputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = null;
            if (this.ports != null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base2x2.OUTPUT_PORT_ID1));
            return logicCircuitNetwork;
        }

        public void OnLogicNetworkConnectionChanged(bool connected)
        {
            ;// throw new NotImplementedException();
        }



        public void OnLogicValueChanged(object data)
        {
            /*
            LogicValueChanged logicValueChanged = (LogicValueChanged)data;
            if (logicValueChanged.portID == DelayGate.INPUT_PORT_ID)
            {
                LastInput = CurrentValue;
                var inputValue = this.GetInputValue();
            }
            */

            if (GetOutputNetwork() != null && GetInputNetwork() != null)
            {
                LogicCircuitManager.ToggleNoWireConnected(false, this.gameObject);
            }
            LogicValueChanged logicValueChanged = (LogicValueChanged)data;
            LogicTick();
        }

        public void RenderEveryTick(float delta)
        {
            //this.elapsedTime += delta;
            //while ((double)this.elapsedTime > (double)LogicCircuitManager.ClockTickInterval)
        }


        protected override void OnCleanUp()
        {
            this.Disconnect();
            base.OnCleanUp();
        }

    }
}
