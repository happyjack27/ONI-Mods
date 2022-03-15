using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;

namespace EdgeDetectors
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public abstract class UnaryGateRibbon : KMonoBehaviour, ILogicEventSender, ILogicNetworkConnection, IRenderEveryTick
    {

        [MyCmpAdd]
        protected CopyBuildingSettings copyBuildingSettings;

        protected static readonly EventSystem.IntraObjectHandler<UnaryGateRibbon> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<UnaryGateRibbon>((component, data) => component.OnLogicValueChanged(data));
        protected LogicPorts ports;
        protected KBatchedAnimController kbac;

        [Serialize]
        protected int LastInput = 0;
        [Serialize]
        protected int CurrentInput = 0;
        [Serialize]
        protected int LastOutput = 0;
        [Serialize]
        protected int CurrentOutput = 0;

        protected bool connected;
        protected LogicPortVisualizer outputOne;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<UnaryGateRibbon>(-905833192, (IntraObjectHandler<UnaryGateRibbon>)((comp, data) => comp.OnCopySettings(data)));
        }

        protected void OnCopySettings(object data)
        {
            UnaryGateRibbon component = ((GameObject)data).GetComponent<UnaryGateRibbon>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
        }


        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Subscribe<UnaryGateRibbon>(-801688580, UnaryGateRibbon.OnLogicValueChangedDelegate);
            this.ports = this.GetComponent<LogicPorts>();
            this.kbac = this.GetComponent<KBatchedAnimController>();
            this.kbac.Play((HashedString)"off");
            Connect();
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

            int outputCellOne = this.ports.GetPortCell(UnaryGateRibbonConfig.OUTPUT_PORT_ID);
            logicCircuitSystem.AddToNetworks(outputCellOne, this, true);

            this.outputOne = new LogicPortVisualizer(outputCellOne, LogicPortSpriteType.RibbonOutput);
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
            int outputCellOne = this.ports.GetPortCell(UnaryGateRibbonConfig.OUTPUT_PORT_ID);
            logicCircuitSystem.RemoveFromNetworks(outputCellOne, (object)this, true);
            logicCircuitManager.RemoveVisElem((ILogicUIElement)this.outputOne);
            this.outputOne = (LogicPortVisualizer)null;
        }

        public int GetInputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetInputValue(UnaryGateRibbonConfig.INPUT_PORT_ID) ?? 0;
        }

        public int GetOutputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetOutputValue(UnaryGateRibbonConfig.OUTPUT_PORT_ID) ?? 0;
        }

        protected LogicCircuitNetwork GetInputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)null;
            if ((UnityEngine.Object)this.ports != (UnityEngine.Object)null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(UnaryGateRibbonConfig.INPUT_PORT_ID));
            return logicCircuitNetwork;
        }

        protected LogicCircuitNetwork GetOutputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = null;
            if (this.ports != null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(UnaryGateRibbonConfig.OUTPUT_PORT_ID));
            return logicCircuitNetwork;
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
        }

        public void RenderEveryTick(float delta)
        {
            //this.elapsedTime += delta;
            //while ((double)this.elapsedTime > (double)LogicCircuitManager.ClockTickInterval)
        }

        public abstract void LogicTick();

        public int GetLogicCell()
        {
            return this.ports.GetPortCell(UnaryGateRibbonConfig.OUTPUT_PORT_ID);
        }

        public int GetLogicValue()
        {
            return this.CurrentOutput;
        }

        public void OnLogicNetworkConnectionChanged(bool connected)
        {
        }

        protected void UpdateVisuals(int inputValue, int outputValue)
        {
            int num = (inputValue > 0 ? 1 : 0) + (outputValue > 0 ? 1 : 0) * 4;
            kbac.Play((HashedString)("on_" + num.ToString()));
        }

        protected override void OnCleanUp()
        {
            this.Disconnect();
            base.OnCleanUp();
        }
    }
}
