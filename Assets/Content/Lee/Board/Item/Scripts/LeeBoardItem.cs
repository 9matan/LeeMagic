using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public enum ELeeBoardItemType
	{
		NONE,
		EMPTY,
		ELEMENT,
		TRACK,
		OCCUPIED
	}
	public enum ELeeBoardItemState
	{
		NONE,
		DEFAULT,
		SELECTED,
		VISITED,
		TRACKED
	}
	public enum ELeeBoardItemTrackType
	{
		NONE,
		VERTICAL,
		HORIZONTAL,
		LEFT_TOP,
		TOP_RIGHT,
		RIGHT_BOTTOM,
		BOTTOM_LEFT
	}

	public interface ILeeBoardItem
	{
		ELeeBoardItemType type { get; }
		ELeeBoardItemState state { get; }
		ELeeBoardItemTrackType trackType { get; }
		ILeeBoardItemInfo info { get; }
	}

	public class LeeBoardItem : MonoBehaviour,
		VVOSS.D2d.IVOSMap2dItem,
		ILeeBoardItem,
		IVOSBuilder
	{

		public ELeeBoardItemType		type
		{
			get { return _type; }
			set { _type = value; }
		}
		public ELeeBoardItemState		state
		{
			get { return _state; }
			set { SetState(value); }
		}
		public ELeeBoardItemTrackType	trackType
		{
			get { return _trackType; }
		}
		public ILeeBoardItemInfo		info
		{
			get; protected set;
		}

		[SerializeField]
		protected ELeeBoardItemType _type;
		[SerializeField]
		protected ELeeBoardItemTrackType _trackType;
		[SerializeField]
		protected ELeeBoardItemState _state;
		[SerializeField]
		protected LeeBoardItemView _view;

		[SerializeField]
		protected bool _updateView = false;

		//
		// < Initialize >
		//

		public void Initialize(ILeeBoardItemInfo __info)
		{
			info = __info;
		}

		//
		// </ Initialize >
		//

		public void SetState(ELeeBoardItemState __state)
		{
			_state = __state;
			_updateView = true;
		}

		public void SetType(ELeeBoardItemType __type)
		{
			_type = __type;
			_updateView = true;
		}

		public void SetTrackType(ELeeBoardItemTrackType __trackType)
		{
			_trackType = __trackType;

			SetType(ELeeBoardItemType.TRACK);
			_updateView = true;
		}



		protected void Update()
		{
			if (_updateView)
				_view.View(this);
		}

		//
		// < Clear >
		// 

		public void Clear()
		{

		}

		//
		// </ Clear >
		//

		[ContextMenu("Build")]
		public void Build()
		{
			_view = gameObject.Build(_view, "View");
		}		
	}
	
}
