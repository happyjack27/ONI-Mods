using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KBComputing.SideScreens {
	internal sealed class Rom4SideScreen : AbstractSideScreen<IROM>
	{
		byte[] bytes = new byte[16];

		protected IList<ListOption<int>> optionsBytes;

		private GameObject[] bytesField = new GameObject[16];
		string[] preset_naems = new string[]
		{
			"Custom",
			"Clear",
			"AND",
			"shift by 1",
		};
		byte[][] presets = new byte[][]
        {
			new byte[]{
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
			},
			new byte[]{
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
			},
			new byte[]{
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0000,
				0b0000,0b0000,0b0000,0b0001,
			},
			new byte[]{
				0b0000,0b0000,0b0001,0b0001,
				0b0010,0b0010,0b0011,0b0011,
				0b0100,0b0100,0b0101,0b0101,
				0b0110,0b0110,0b0111,0b0111,
			},
		};

		private void BytesOptionSelected(GameObject obj, ListOption<int> option)
		{
			int o = option.value;
			if( target == null )
            {
				return;
            }
			for( int i = 0; i < 16; i++)
            {
				if( obj == bytesField[i])
                {
					bytes[i] = (byte)option.value;
					target.setBytes(bytes);
					return;
                }
            }
		}


		public override string GetTitle() {
			return SideScreenStrings.UI.UISIDESCREENS.MEMORY_CONTENTS.TITLE;
		}

		protected override void OnPrefabInit() {
			optionsBytes = new List<ListOption<int>>(16);
			for (int i = 0; i < 16; i++)
			{
				string s = SideScreenStrings.BUILDINGS.FOURBITS.FORMATTED[i];
				optionsBytes.Add(new ListOption<int>(i, s));
			}

			var margin = new RectOffset(4, 4, 4, 4);
			// Update the parameters of the base BoxLayoutGroup
			var baseLayout = gameObject.GetComponent<BoxLayoutGroup>();
			if (baseLayout != null)
				baseLayout.Params = new BoxLayoutParams() {
					Margin = margin,
					Direction = PanelDirection.Vertical,
					Alignment = TextAnchor.UpperCenter,
					Spacing = 8
				};
			var rowBG = PUITuning.Images.GetSpriteByName("overview_highlight_outline_sharp");

			for( int i = 0; i < 16; i++) {
				string labeltext = SideScreenStrings.BUILDINGS.FOURBITS.FORMATTED[i];
				PPanel rowBytes = new PPanel("BytesRow")
				{
					FlexSize = Vector2.one,
					Alignment = TextAnchor.MiddleCenter,
					Spacing = 10,
					Direction = PanelDirection.Horizontal,
					Margin = new RectOffset(2, 2, 2, 2)
				}.AddChild(new PLabel("BytesLabel")
				{
					TextAlignment = TextAnchor.MiddleLeft,
					ToolTip = "",
					Text = labeltext,
					TextStyle = PUITuning.Fonts.TextDarkStyle,
					FlexSize = Vector2.left,
				});
				var cbBytes = new PComboBox<ListOption<int>>("BytesSelect")
				{
					Content = optionsBytes,
					InitialItem = optionsBytes[0],
					FlexSize = Vector2.right,
					ToolTip = "",
					TextStyle = PUITuning.Fonts.TextLightStyle,
					TextAlignment = TextAnchor.MiddleRight,
					OnOptionSelected = BytesOptionSelected
				}.AddOnRealize((obj) => bytesField[i]= obj);
				rowBytes.AddChild(cbBytes);
				rowBytes.AddTo(gameObject);
			}

			ContentContainer = gameObject;

			base.OnPrefabInit();
			Load(gameObject);
		}



		protected override void Clear(GameObject _)
        {
			byte[] bb = new byte[16];
			for( int i = 0; i < bb.Length; i++ )
            {
				bb[i] = (byte)i;
            }
			target.setBytes(bb);
			Load(gameObject);
        }

        protected override void Store(GameObject _)
        {
			if (target == null || bytesField == null)
				return;
			target.setBytes( bytes);
		}

		protected override void Load(GameObject _)
		{
			if (target == null || bytesField == null || optionsBytes == null)
				return;
			bytes = target.getBytes();
			for( int i = 0; i < 16; i++) {
				if( bytesField[i] == null )
                {
					continue;
                }
				GameObject obj = bytesField[i];
				PComboBox<ListOption<int>>.SetSelectedItem(obj, optionsBytes[bytes[i]]);
			}
		}
    }
}
