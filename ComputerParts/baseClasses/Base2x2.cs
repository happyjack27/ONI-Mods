using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EventSystem;

namespace KBComputing.baseClasses
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public abstract class Base2x2 : KMonoBehaviour, ILogicEventSender, ILogicNetworkConnection, IRenderEveryTick
    {

        [MyCmpAdd]
        protected CopyBuildingSettings copyBuildingSettings;

        protected static readonly EventSystem.IntraObjectHandler<Base2x2> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Base2x2>((component, data) => component.OnLogicValueChanged(data));
        protected LogicPorts ports;
        protected KBatchedAnimController kbac;

        [Serialize]
        protected int InputValue = 0;
        [Serialize]
        protected int StoredValue = 0;
        [Serialize]
        protected int OutputValue = 0;
        [Serialize]
        protected int ControlPort1Value = 1;
        [Serialize]
        protected int ControlPort2Value = 1;

        protected bool connected;

        protected LogicPortVisualizer outputOne;

        Dictionary<int, HashedString> _animations = new Dictionary<int, HashedString>();


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<Base2x2>(-905833192, (IntraObjectHandler<Base2x2>)((comp, data) => comp.OnCopySettings(data)));
        }

        protected void OnCopySettings(object data)
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

            int outputCellOne = this.ports.GetPortCell(Base2x2Config.OUTPUT_PORT_ID);
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
            int outputCellOne = this.ports.GetPortCell(Base2x2Config.OUTPUT_PORT_ID);
            logicCircuitSystem.RemoveFromNetworks(outputCellOne, (object)this, true);
            logicCircuitManager.RemoveVisElem((ILogicUIElement)this.outputOne);
            this.outputOne = (LogicPortVisualizer)null;
        }

        public int GetInputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetInputValue(Base2x2Config.INPUT_PORT_ID) ?? 0;
        }

        public int GetOutputValue()
        {
            LogicPorts component = this.GetComponent<LogicPorts>();
            return component?.GetOutputValue(Base2x2Config.OUTPUT_PORT_ID) ?? 0;
        }

        protected LogicCircuitNetwork GetInputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)null;
            if ((UnityEngine.Object)this.ports != (UnityEngine.Object)null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base2x2Config.INPUT_PORT_ID));
            return logicCircuitNetwork;
        }

        protected LogicCircuitNetwork GetOutputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = null;
            if (this.ports != null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base2x2Config.OUTPUT_PORT_ID));
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
            UpdateVisuals();
        }
        public int GetLogicCell()
        {
            return this.ports.GetPortCell(Base2x2Config.OUTPUT_PORT_ID);
        }

        public int GetLogicValue()
        {
            return this.OutputValue;
        }

        public void OnLogicNetworkConnectionChanged(bool connected)
        {
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            int bit0 = 0, bit1 = 0, bit2 = 0, bit3 = 0;
            bit0 = StoredValue == 0x0F ? 2 : StoredValue == 0x00 ? 0 : 1;
            bit1 = ControlPort1Value > 0 ? 1 : 0;
            bit2 = ControlPort2Value > 0 ? 1 : 0;
            bit3 = 0;
            kbac.Play("on_" + (bit0 + 3 * bit1 + 6 * bit2 + 12 * bit3), KAnim.PlayMode.Once, 1f, 0.0f);
        }

        protected override void OnCleanUp()
        {
            this.Disconnect();
            base.OnCleanUp();
        }
    }
}
