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
        , IRenderEveryTick
        {

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
        }

        public void RenderEveryTick(float dt)
        {
            ReadValues();
            if ( UpdateValues())
                UpdateVisuals();
        }
        protected abstract void ReadValues();

        protected abstract bool UpdateValues();

        protected abstract void UpdateVisuals();

    }
}
