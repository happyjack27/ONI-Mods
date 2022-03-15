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

        protected LogicPorts ports;
        protected KBatchedAnimController kbac;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            gameObject.AddOrGet<CopyBuildingSettings>();
            this.ports = this.GetComponent<LogicPorts>();
            this.kbac = this.GetComponent<KBatchedAnimController>();
        }

        public void OnLogicNetworkConnectionChanged(bool connected)
        {
        }

        public void RenderEveryTick(float dt)
        {
            if( UpdateValues())
                UpdateVisuals();
        }

        public abstract bool UpdateValues();

        public abstract void UpdateVisuals();

    }
}
