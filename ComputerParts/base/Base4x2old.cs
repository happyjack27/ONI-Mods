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
    public abstract class Base4x2 : KMonoBehaviour, ILogicEventSender, ILogicEventReceiver, ILogicNetworkConnection, IRenderEveryTick
    {

        [MyCmpAdd]
        protected CopyBuildingSettings copyBuildingSettings;

        protected static readonly EventSystem.IntraObjectHandler<Base4x2> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Base4x2>((component, data) => component.OnLogicValueChanged(data));
        protected LogicPorts ports;
        protected KBatchedAnimController kbac;

        [Serialize]
        protected int[][] PortValue = new int[2][] { new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };

        protected bool connected = false;

        protected LogicPortVisualizer outputOne;
        protected LogicPortVisualizer outputTwo;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<Base4x2>(-905833192, (IntraObjectHandler<Base4x2>)((comp, data) => comp.OnCopySettings(data)));
        }

        protected void OnCopySettings(object data)
        {
            Base4x2 component = ((GameObject)data).GetComponent<Base4x2>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
        }


        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Subscribe<Base4x2>(-801688580, Base4x2.OnLogicValueChangedDelegate);
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

            int outputCellOne = this.ports.GetPortCell(Base4x2Config.PORT_ID[1][0]);
            logicCircuitSystem.AddToNetworks(outputCellOne, this, true);

            this.outputOne = new LogicPortVisualizer(outputCellOne, LogicPortSpriteType.RibbonOutput);
            logicCircuitManager.AddVisElem((ILogicUIElement)this.outputOne);

            int outputCellTwo = this.ports.GetPortCell(Base4x2Config.PORT_ID[1][1]);
            logicCircuitSystem.AddToNetworks(outputCellTwo, this, true);

            this.outputTwo = new LogicPortVisualizer(outputCellTwo, LogicPortSpriteType.RibbonOutput);
            logicCircuitManager.AddVisElem((ILogicUIElement)this.outputTwo);
            

            LogicCircuitManager.ToggleNoWireConnected(false, this.gameObject);
        }

        protected void Disconnect()
        {
            if (!this.connected)
                return;
            var logicCircuitSystem = Game.Instance.logicCircuitSystem;
            var logicCircuitManager = Game.Instance.logicCircuitManager;
            this.connected = false;

            int outputCellOne = this.ports.GetPortCell(Base4x2Config.PORT_ID[1][0]);
            logicCircuitSystem.RemoveFromNetworks(outputCellOne, (object)this, true);
            logicCircuitManager.RemoveVisElem((ILogicUIElement)this.outputOne);
            this.outputOne = (LogicPortVisualizer)null;

            
            int outputCellTwo = this.ports.GetPortCell(Base4x2Config.PORT_ID[1][1]);
            logicCircuitSystem.RemoveFromNetworks(outputCellTwo, (object)this, true);
            logicCircuitManager.RemoveVisElem((ILogicUIElement)this.outputTwo);
            this.outputTwo = (LogicPortVisualizer)null;
            
        }

        /*
        protected LogicCircuitNetwork GetInputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)null;
            if ((UnityEngine.Object)this.ports != (UnityEngine.Object)null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][0]));
            return logicCircuitNetwork;
        }

        protected LogicCircuitNetwork GetOutputNetwork()
        {
            LogicCircuitNetwork logicCircuitNetwork = null;
            if (this.ports != null)
                logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[1][0]));
            return logicCircuitNetwork;
        }
        */
        

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
            connected = (UnityEngine.Object)this.ports != (UnityEngine.Object)null;
            LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)null;
            for( int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if( i == 1 && j == 2)
                    {
                        continue;
                    }
                    logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[i][j]));
                    if (logicCircuitNetwork == null)
                    {
                        connected = false;
                    }

                }
            }
            
            LogicCircuitManager.ToggleNoWireConnected(!connected, this.gameObject);
        }

        public void RenderEveryTick(float delta)
        {
            //this.elapsedTime += delta;
            //while ((double)this.elapsedTime > (double)LogicCircuitManager.ClockTickInterval)
        }

        public abstract void LogicTick();

        public void UpdateValues()
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 1 && j == 2)
                        {
                            PortValue[i][j] = 0;
                            continue;
                        }
                        PortValue[i][j] = this.GetComponent<LogicPorts>()?.GetInputValue(Base4x2Config.PORT_ID[i][j]) ?? 0;
                    }
                }
            } catch
            {

            }
        }

        public void OnLogicNetworkConnectionChanged(bool connected)
        {
            if( connected)
                UpdateValues();
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            try
            {
                if( !connected)
                {
                    //this.kbac.Play((HashedString)"off");
                    //return;
                }
                var nw1 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][0]));
                var nw1b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][1]));
                var nw2 = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][2]));
                var nw2b = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[0][3]));

                var nwOut = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[1][0]));
                var nwOutb = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[1][1]));

                var nwOp = Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(Base4x2Config.PORT_ID[1][3]));

                ShowSymbolConditionally(nwOp, () => nwOp.OutputValue > 0, $"light{24}_bloom_green", $"light{24}_bloom_red");


                //16-19 outb
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(3, nwOutb.OutputValue), $"light{19}_bloom_green", $"light{19}_bloom_red");
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(2, nwOutb.OutputValue), $"light{18}_bloom_green", $"light{18}_bloom_red");
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(1, nwOutb.OutputValue), $"light{17}_bloom_green", $"light{17}_bloom_red");
                ShowSymbolConditionally(nwOutb, () => LogicCircuitNetwork.IsBitActive(0, nwOutb.OutputValue), $"light{16}_bloom_green", $"light{16}_bloom_red");
                // 20-23 out
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(3, nwOut.OutputValue), $"light{23}_bloom_green", $"light{23}_bloom_red");
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(2, nwOut.OutputValue), $"light{22}_bloom_green", $"light{22}_bloom_red");
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(1, nwOut.OutputValue), $"light{21}_bloom_green", $"light{21}_bloom_red");
                ShowSymbolConditionally(nwOut, () => LogicCircuitNetwork.IsBitActive(0, nwOut.OutputValue), $"light{20}_bloom_green", $"light{20}_bloom_red");

                //in1b 0-3
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(3, nw1b.OutputValue), $"light{3}_bloom_green", $"light{3}_bloom_red");
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(2, nw1b.OutputValue), $"light{2}_bloom_green", $"light{2}_bloom_red");
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(1, nw1b.OutputValue), $"light{1}_bloom_green", $"light{1}_bloom_red");
                ShowSymbolConditionally(nw1b, () => LogicCircuitNetwork.IsBitActive(0, nw1b.OutputValue), $"light{0}_bloom_green", $"light{0}_bloom_red");
                //in1 4-7
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(3, nw1.OutputValue), $"light{7}_bloom_green", $"light{7}_bloom_red");
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(2, nw1.OutputValue), $"light{6}_bloom_green", $"light{6}_bloom_red");
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(1, nw1.OutputValue), $"light{5}_bloom_green", $"light{5}_bloom_red");
                ShowSymbolConditionally(nw1, () => LogicCircuitNetwork.IsBitActive(0, nw1.OutputValue), $"light{4}_bloom_green", $"light{4}_bloom_red");

                //in2b 8-11
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(3, nw2b.OutputValue), $"light{11}_bloom_green", $"light{11}_bloom_red");
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(2, nw2b.OutputValue), $"light{10}_bloom_green", $"light{10}_bloom_red");
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(1, nw2b.OutputValue), $"light{9}_bloom_green", $"light{9}_bloom_red");
                ShowSymbolConditionally(nw2b, () => LogicCircuitNetwork.IsBitActive(0, nw2b.OutputValue), $"light{8}_bloom_green", $"light{8}_bloom_red");
                //in2 12-15
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(3, nw2.OutputValue), $"light{15}_bloom_green", $"light{15}_bloom_red");
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(2, nw2.OutputValue), $"light{14}_bloom_green", $"light{14}_bloom_red");
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(1, nw2.OutputValue), $"light{13}_bloom_green", $"light{13}_bloom_red");
                ShowSymbolConditionally(nw2, () => LogicCircuitNetwork.IsBitActive(0, nw2.OutputValue), $"light{12}_bloom_green", $"light{12}_bloom_red");

            }
            catch (Exception ex)
            {

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
        protected override void OnCleanUp()
        {
            this.Disconnect();
            base.OnCleanUp();
        }

        public int GetLogicCell()
        {
            return this.ports.GetPortCell(Base4x2Config.PORT_ID[1][0]);
        }

        public int GetLogicValue()
        {
            return this.PortValue[1][0];
        }

        public int GetInputValue()
        {
            return this.PortValue[0][0];
        }

        public int GetOutputValue()
        {
            return this.PortValue[1][0];
        }

        public void ReceiveLogicEvent(int value)
        {
            LogicTick();
        }
    }
}
