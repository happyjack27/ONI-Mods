// Decompiled with JetBrains decompiler
// Type: LogicGateBuffer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1FE956DE-2521-4C50-80CE-B4D7F814C847
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

namespace SomeLogicGates
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LogicGateDiode : LogicGate2
    {
        [Serialize]
        private MeterController meter;
        [MyCmpAdd]
        private CopyBuildingSettings copyBuildingSettings;
        private static readonly EventSystem.IntraObjectHandler<LogicGateDiode> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateDiode>((System.Action<LogicGateDiode, object>)((component, data) => component.OnCopySettings(data)));

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<LogicGateDiode>(-905833192, LogicGateDiode.OnCopySettingsDelegate);
        }

        private void OnCopySettings(object data)
        {
            LogicGateDiode component = ((GameObject)data).GetComponent<LogicGateDiode>();
            if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                return;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.meter = new MeterController((KAnimControllerBase)this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, (string[])null);
            this.meter.SetPositionPercent(1f);
        }

        private void Update() => this.meter.SetPositionPercent(0.0f);

        public override void LogicTick()
        {
            if (this.cleaningUp)
                return;
            this.meter.SetPositionPercent(1f);
            if (this.outputValueOne == 0 || !(Game.Instance.logicCircuitSystem.GetNetworkForCell(this.OutputCellOne) is LogicCircuitNetwork))
                return;
            this.outputValueOne = 0;
            this.RefreshAnimation();
        }



    }
}