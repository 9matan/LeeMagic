using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VVOSS.UI
{

	public interface IVOSUIPanel : IVOSUIItem
	{

	}

	public class VOSUIPanel : VOSUIItem,
		IVOSUIPanel,
		IVOSBuilder
	{

		[SerializeField]
		protected List<VOSUIItem> _uiItems;

		//
		// < Initialize >
		//

		public override void Initialize(IVOSUI __ui)
		{
			base.Initialize(__ui);

			_InitializeUIItems();
		}

		protected virtual void _InitializeUIItems()
		{
			for (int i = 0; i < _uiItems.Count; ++i)
				_uiItems[i].Initialize(ui);
		}

		//
		// </ Initialize >
		//

		//
		// < Clear >
		//

		protected virtual void _ClearUIItems()
		{
			for (int i = _uiItems.Count - 1; i >= 0; --i)
				_uiItems[i].Clear();
		}

		public override void Clear()
		{
			_ClearUIItems();

			base.Clear();
		}

		//
		// </ Clear >
		//

#if UNITY_EDITOR

		[ContextMenu("Build")]
		public override void Build()
		{
			_BuildItems();
		}

		protected void _BuildItems()
		{
			for (int i = 0; i < _uiItems.Count; ++i)
				_uiItems[i].Build();
		}

#endif

	}

}
