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
    class Rom4 : baseClasses.BaseLogicOnChange
    {

        [Serialize]
        public byte[] Memory = new byte[16];

        protected override void OnCopySettings(object data)
        {
            Rom4 component = ((GameObject)data).GetComponent<Rom4>();
            if (component == null) return;
            this.Memory = (byte[])component.Memory.Clone();

            ReadValues();
            UpdateValues();
            UpdateVisuals();
        }

        protected override void ReadValues()
        {
        }

        protected override bool UpdateValues()
        {
            
            return true;
        }

        protected override void UpdateVisuals()
        {
            throw new NotImplementedException();
        }
    }
}
