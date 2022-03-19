using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;

namespace KBComputing
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public abstract class WireToRibbon : KMonoBehaviour, ILogicEventSender, ILogicNetworkConnection, IRenderEveryTick
    {

        [MyCmpAdd]
        protected CopyBuildingSettings copyBuildingSettings;

        protected static readonly EventSystem.IntraObjectHandler<WireToRibbon> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<WireToRibbon>((component, data) => component.OnLogicValueChanged(data));
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

        Dictionary<int, HashedString> _animations = new Dictionary<int, HashedString>();


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<WireToRibbon>(-905833192, (IntraObjectHandler<WireToRibbon>)((comp, data) => comp.OnCopySettings(data)));
        }

        protected void OnCopySettings(object data)
        {
            WireToRibbon component = ((GameObject)data).GetComponent<WireToRibbon>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
        }


        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Subscribe<WireToRibbon>(-801688580, WireToRibbon.OnLogicValueChangedDelegate);
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

            int outputCellOne = this.ports.GetPortCell(WireToRibbonConfig.OUTPUT_PORT_ID);
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
            int outputCellOne = this.ports.GetPortCell(WireToRibbonConfig.OUTPUT_PORT_ID);
            logicCircuitSystem.RemoveFromNetworks(outputCellOne, (object)this, true);
            logicCircuitManager.RemoveVisElem((ILogicUIElement)this.outputOne);
            this.outputOne = (LogicPortVisualizer)null;
        }

        public int GetInputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetInputValue(WireToRibbonConfig.INPUT_PORT_ID) ?? 0;
        }

        public int GetOutputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetOutputValue(WireToRibbonConfig.OUTPUT_PORT_ID) ?? 0;
        }

        protected LogicCircuitNetwork GetInputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)null;
            if ((UnityEngine.Object)this.ports != (UnityEngine.Object)null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(WireToRibbonConfig.INPUT_PORT_ID));
            return logicCircuitNetwork;
        }

        protected LogicCircuitNetwork GetOutputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = null;
            if (this.ports != null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(WireToRibbonConfig.OUTPUT_PORT_ID));
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

        public virtual void LogicTick()
        {
            if (GetInputNetwork() == null || GetOutputNetwork() == null)
            {
                kbac.Play((HashedString)"off");
                return;
            }

            LastInput = CurrentInput;
            CurrentInput = GetInputValue();
            CurrentOutput = CurrentInput;
            if (CurrentOutput != LastOutput)
            {
                this.GetComponent<LogicPorts>().SendSignal(WireToRibbonConfig.OUTPUT_PORT_ID, CurrentOutput);
            }
            UpdateVisuals();
        }
        public int GetLogicCell()
        {
            return this.ports.GetPortCell(WireToRibbonConfig.OUTPUT_PORT_ID);
        }

        public int GetLogicValue()
        {
            return this.CurrentOutput;
        }

        public void OnLogicNetworkConnectionChanged(bool connected)
        {
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            LogicCircuitNetwork inputNetwork = this.GetInputNetwork();
            LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
            if (inputNetwork != null || outputNetwork != null)
            {
                var firstBit = GetInputValue() > 0 ? 1 : 0;
                var key = firstBit << 4;
                var value = GetOutputValue();
                key |= value;
                if (!_animations.ContainsKey(key))
                {
                    var bit1 = 0x1 & value;
                    var bit2 = 0x1 & (value >> 1);
                    var bit3 = 0x1 & (value >> 2);
                    var bit4 = 0x1 & (value >> 3);
                    var valString = $"{bit1}{bit2}{bit3}{bit4}";
                    _animations.Add(key, $"on_{firstBit}_{valString}");
                }
                this.kbac.Play((HashedString)_animations[key], KAnim.PlayMode.Paused);
            }
            else
            {
                this.kbac.Play("off");
            }

        }

        protected override void OnCleanUp()
        {
            this.Disconnect();
            base.OnCleanUp();
        }
    }
}
