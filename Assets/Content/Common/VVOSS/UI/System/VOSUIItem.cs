using UnityEngine;
using System.Collections;

namespace VVOSS.UI
{

	public interface IVOSUIItem
	{
		IVOSUI ui { get; }
		int id { get; }
		string name { get; }
		EVOSUIItemState state { get; set; }
	}

	public enum EVOSUIItemState
	{
		CUSTOM,
		DEFAULT,
		SHOW,
		HIDE
	}

	public class VOSUIItem : MonoBehaviour,
		IVOSUIItem,
		IVOSBuilder
	{

		private static int _nextId = 0;

		public int id { get { return _id; } }
		public IVOSUI ui { get; protected set; }
		public EVOSUIItemState state
		{
			get { return _state; }
			set { SetState(value); }
		}

		[SerializeField]
		protected EVOSUIItemState _state;
		[SerializeField]
		private int _id = -1;


		public virtual void Initialize(IVOSUI __ui)
		{
			ui = __ui;

			_SetId();
		}

		private void _SetId()
		{
			_id = _nextId++;
		}

		//
		// < State >
		//

		public virtual void SetState(EVOSUIItemState __state)
		{
			switch (__state)
			{
				case EVOSUIItemState.SHOW:
					_Show();
					break;
				case EVOSUIItemState.HIDE:
					_Hide();
					break;
			}

			_state = __state;
		}

		protected void _Show()
		{
			gameObject.SetActive(true);
		}

		protected void _Hide()
		{
			gameObject.SetActive(false);
		}

		//
		// </ State >
		//

		public virtual void Clear()
		{
			_id = -1;

			ui = null;
		}

#if UNITY_EDITOR

		[ContextMenu("Build")]
		public virtual void Build()
		{
		}

#else

	public void Build(){}

#endif

	}

}