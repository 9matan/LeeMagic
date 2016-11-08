using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VVOSS.UI
{

	public interface IVOSUI
	{

	}

	public class VOSUI : MonoBehaviour,
		IVOSUI,
		IVOSBuilder
	{

		[SerializeField]
		protected List<VOSUIItem> _uiItems;

		//
		// < Initialize >
		//

		protected virtual void _InitializeUIItems()
		{
			for (int i = 0; i < _uiItems.Count; ++i)
				_uiItems[i].Initialize(this);
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

		//
		// </ Clear >
		//

#if UNITY_EDITOR

		[ContextMenu("Build")]
		public virtual void Build()
		{
			_BuildItems();
		}

		protected void _BuildItems()
		{
			for (int i = 0; i < _uiItems.Count; ++i)
				_uiItems[i].Build();
		}

#else
	public void Build(){}

#endif

	}

}