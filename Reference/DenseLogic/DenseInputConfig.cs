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
	public class DenseInputConfig : IBuildingConfig {
		public const string ID = "DenseLogicTeam_DenseInput";

		public override BuildingDef CreateBuildingDef() {
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 2, 1,
				"dense_INPUT_kanim", 
				TUNING.BUILDINGS.HITPOINTS.TIER1,
				TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				TUNING.MATERIALS.REFINED_METALS, 1600.0f, BuildLocationRule.Anywhere,
				TUNING.BUILDINGS.DECOR.PENALTY.TIER1, TUNING.NOISE_POLLUTION.NOISY.TIER1);
			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.PermittedRotations = PermittedRotations.R90;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			buildingDef.AlwaysOperational = true;
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
			{
				LogicPorts.Port.RibbonOutputPort(
					DenseInput.OUTPUTID,
					new CellOffset(1, 0),
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_DENSEINPUT.LOGIC_PORT_OUTPUT,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_DENSEINPUT.PORTOUT_ACTIVE,
					DenseLogicStrings.BUILDINGS.PREFABS.DENSELOGICTEAM_DENSEINPUT.PORTOUT_INACTIVE
				)
			};
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go) {
			go.AddOrGet<DenseInput>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
		}
	}
}

