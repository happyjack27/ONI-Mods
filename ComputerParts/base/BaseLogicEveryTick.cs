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
    abstract class BaseLogicEveryTick : KMonoBehaviour
        , ILogicNetworkConnection
        , ISaveLoadable
        , ILogicEventSender
        , ILogicEventReceiver
        , IRenderEveryTick
    {

        private static readonly EventSystem.IntraObjectHandler<BaseLogicOnChange>
            OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<BaseLogicOnChange>(
            (component, data) => component.OnLogicValueChanged(data)
            );
        private static readonly EventSystem.IntraObjectHandler<BaseLogicEveryTick>
            OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BaseLogicEveryTick>(
            (component, data) => component.OnCopySettings(data));

        protected LogicPorts ports;
        protected KBatchedAnimController kbac;


        protected virtual void OnCopySettings(object data)
        {
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            gameObject.AddOrGet<CopyBuildingSettings>();
            Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
            this.ports = this.GetComponent<LogicPorts>();
            this.kbac = this.GetComponent<KBatchedAnimController>();
        }

        public void OnLogicNetworkConnectionChanged(bool connected)
        {
            OnLogicValueChanged(null);
        }

        public void OnLogicValueChanged(object data)
        {
            RenderEveryTick(0);
        }


        private readonly object syncLock = new object();
        public void RenderEveryTick(float dt)
        {
            lock (syncLock)
            {
                ReadValues();
                UpdateValues();
            }
            UpdateVisuals();
        }
        protected abstract void ReadValues();

        protected abstract void UpdateValues();

        protected abstract void UpdateVisuals();
        public int GetLogicCell()
        {
            return 0;
        }

        public int GetLogicValue()
        {
            return 0;
        }

        public void ReceiveLogicEvent(int value)
        {
            OnLogicValueChanged(null);
        }

        public void LogicTick()
        {
            OnLogicValueChanged(null);
        }
    }
}
