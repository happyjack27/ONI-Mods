/*
 * Copyright 2020 Dense Logic Team
 * 
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
	public class LogicSevenSegmentConfig : IBuildingConfig {
		public static string ID = "DenseLogicTeam_LogicSevenSegment";

		public override BuildingDef CreateBuildingDef() {
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 3,
				"logic_SEVEN_kanim",
				TUNING.BUILDINGS.HITPOINTS.TIER1,
				TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				TUNING.MATERIALS.REFINED_METALS, 1600.0f, BuildLocationRule.Anywhere,
				TUNING.BUILDINGS.DECOR.PENALTY.TIER1, TUNING.NOISE_POLLUTION.NOISY.TIER1);
			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.PermittedRotations = PermittedRotations.FlipV;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.ObjectLayer = ObjectLayer.LogicGate;
			buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
			buildingDef.AlwaysOperational = true;
			buildingDef.LogicInputPorts = new List<LogicPorts.Port>() {
			  LogicPorts.Port.RibbonInputPort(
				  LogicSevenSegment.INPUTID, 
				  LogicSevenSegment.INPUTOFFSET, 
				  DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICSEVENSEGMENT.LOGIC_PORT,
				  DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICSEVENSEGMENT.INPUT_PORT_ACTIVE,
				  DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICSEVENSEGMENT.INPUT_PORT_INACTIVE,
				  true, false),
			};
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
				LogicPorts.Port.OutputPort(
					LogicSevenSegment.OUTPUTID,
					LogicSevenSegment.OUTPUTOFFSET,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICSEVENSEGMENT.OUTPUT_LOGIC_PORT,
				  DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICSEVENSEGMENT.OUTPUT_PORT_ACTIVE,
				  DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_LOGICSEVENSEGMENT.OUTPUT_PORT_INACTIVE,
				  false, false),
			};
			SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go) {
			go.AddOrGet<LogicSevenSegment>();
		}
	}
}
