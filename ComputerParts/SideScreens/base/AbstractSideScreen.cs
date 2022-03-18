

using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KBComputing {
	abstract class AbstractSideScreen<T> : SideScreenContent {

		protected T target;

		public sealed override void ClearTarget() {
			target = default(T);
		}

		public override sealed bool IsValidForTarget(GameObject target) {
			try
			{
				return target.GetComponent<T>() != null;
			} catch { return false; }
		}
		protected abstract void Clear(GameObject _);

		protected abstract void Store(GameObject _);
		protected abstract void Load(GameObject _);

		public override sealed void SetTarget(GameObject target) {
			this.target = target.GetComponent<T>();
			Load(target);
		}
	}
}
