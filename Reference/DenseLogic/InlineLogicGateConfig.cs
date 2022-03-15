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
	public class InlineLogicGateConfig : IBuildingConfig {
		public const string ID = "DenseLogicTeam_InlineGate";
		public const ObjectLayer LAYER = ObjectLayer.LogicGate;

		public override BuildingDef CreateBuildingDef() {
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 1,
				"dense_INLINE_kanim", 
				TUNING.BUILDINGS.HITPOINTS.TIER1, 
				TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1, 
				TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				TUNING.MATERIALS.REFINED_METALS, 1600.0f, BuildLocationRule.Anywhere,
				TUNING.BUILDINGS.DECOR.PENALTY.TIER1, TUNING.NOISE_POLLUTION.NOISY.TIER1);
			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.ObjectLayer = LAYER;
			buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
			buildingDef.AlwaysOperational = true;
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
				LogicPorts.Port.RibbonOutputPort(
					InlineLogicGate.PORTID,
					InlineLogicGate.OFFSET,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_INLINEGATE.LOGIC_PORT_IO,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_INLINEGATE.PORT_ACTIVE,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_INLINEGATE.PORT_INACTIVE
				)
			};
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go) {
			go.AddOrGet<InlineLogicGate>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
		}
	}
}
