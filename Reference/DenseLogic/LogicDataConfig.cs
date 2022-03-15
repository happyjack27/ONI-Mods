/*
 * Copyright 2020 Dense Logic Team
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ONI_DenseLogic {
	public class LogicDataConfig : IBuildingConfig {
		public const string ID = "DenseLogicTeam_LogicData";

		public override BuildingDef CreateBuildingDef() {
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 2, 2,
				"logic_DLATCH_kanim", TUNING.BUILDINGS.HITPOINTS.TIER0,
				TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
				TUNING.MATERIALS.REFINED_METALS, 1600.0f, BuildLocationRule.Anywhere,
				TUNING.BUILDINGS.DECOR.PENALTY.TIER0, TUNING.NOISE_POLLUTION.NONE);
			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.ObjectLayer = ObjectLayer.LogicGate;
			buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
			buildingDef.AlwaysOperational = true;
			buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
			{
				LogicPorts.Port.InputPort(
					LogicData.DATAID,
					LogicData.DATAOFFSET,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICDATA.DATA_PORT,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICDATA.DATA_PORT_ACTIVE,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICDATA.DATA_PORT_INACTIVE,
					true, 
					true
				),
				LogicPorts.Port.InputPort(
					LogicData.SETID,
					LogicData.SETOFFSET,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICDATA.SET_PORT,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICDATA.SET_PORT_ACTIVE,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICDATA.SET_PORT_INACTIVE,
					true, true
				)
			};
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
			{
				LogicPorts.Port.OutputPort(
					LogicData.READID,
					LogicData.READOFFSET,
					STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT,
					STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT_ACTIVE,
					STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT_INACTIVE,
					true, true
				)
			};
			SoundEventVolumeCache.instance.AddVolume("logic_memory_kanim", "PowerMemory_on", TUNING.NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("logic_memory_kanim", "PowerMemory_off", TUNING.NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go) {
			go.AddOrGet<LogicData>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
		}
	}
}
