using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	public interface ILeeBoardItem :
		IVOSTransformable
	{
		bool isElement { get; }
		bool isEmpty { get; }

		int edgesCount { get; }
		IEnumerable<ILeeBoardItem> edges { get; }

		void AddEdge(ILeeBoardItem edge);
		void RemoveEdge(ILeeBoardItem edge);
		bool ContainsEdge(ILeeBoardItem edge);

		ELeeBoardItemType type { get; set; }
		ELeeBoardItemState state { get; set; }
		ELeeBoardItemTrackType trackType { get; set; }
		int elementId { get; set; }
		int distance { get; set; }
		ILeeBoardItemInfo info { get; }

		int row { get; }
		int column { get; }
	}

	public class LeeBoardItem : MonoBehaviour,
		VVOSS.D2d.IVOSMap2dItem,
		ILeeBoardItem,
		IVOSBuilder
	{

		public bool isElement
		{
			get { return _type == ELeeBoardItemType.ELEMENT; }
		}
		public bool isEmpty
		{
			get { return _type == ELeeBoardItemType.EMPTY; }
		}

		public int							edgesCount
		{
			get { return _edges.Count; }
		}
		public IEnumerable<ILeeBoardItem>	edges
		{
			get { return _edges; }
		}

		

		public ELeeBoardItemType		type
		{
			get { return _type; }
			set { SetType(value); }
		}
		public ELeeBoardItemState		state
		{
			get { return _state; }
			set { SetState(value); }
		}
		public ELeeBoardItemTrackType	trackType
		{
			get { return _trackType; }
			set { SetTrackType(value); }
		}
		public ILeeBoardItemInfo		info
		{
			get; protected set;
		}
		public int						elementId
		{
			get { return _elementId; }
			set { SetElementId(value); }
		}
		public int						distance
		{
			get { return _distance; }
			set { SetDistance(_distance); }
		}

		public int row
		{
			get { return _row; }
			set { _row = value; }
		}
		public int column
		{
			get { return _column; }
			set { _column = value; }
		}

		public bool updateView
		{
			get { return _updateView; }
			set { _updateView = value; }
		}

		[SerializeField]
		protected int _row;
		[SerializeField]
		protected int _column;
		[SerializeField]
		protected ELeeBoardItemType _type;
		[SerializeField]
		protected ELeeBoardItemTrackType _trackType;
		[SerializeField]
		protected ELeeBoardItemState _state;
		[SerializeField]
		protected int _elementId = 0;
		[SerializeField]
		protected int _distance = 0;
		[SerializeField]
		protected LeeBoardItemView _view;

		[SerializeField]
		protected bool _updateView = false;

		protected HashSet<ILeeBoardItem> _edges = new HashSet<ILeeBoardItem>();

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
			if (__type != ELeeBoardItemType.ELEMENT)
			{
				_elementId = -1;
				_ClearEdges();				
			}

			_type = __type;
			_updateView = true;
		}

		public void SetTrackType(ELeeBoardItemTrackType __trackType)
		{
			_trackType = __trackType;

			SetType(ELeeBoardItemType.TRACK);
			_updateView = true;
		}

		public void SetElementId(int id)
		{
			_elementId = id;
			_type = ELeeBoardItemType.ELEMENT;

			_updateView = true;
		}

		public void SetDistance(int __distance)
		{
			_distance = __distance;
		}

		public void AddEdge(ILeeBoardItem edge)
		{
			_edges.Add(edge);
		}

		public void RemoveEdge(ILeeBoardItem edge)
		{
			_edges.Remove(edge);
		}

		public bool ContainsEdge(ILeeBoardItem edge)
		{
			return _edges.Contains(edge);
		}

		protected void Update()
		{
			if (_updateView)
			{
				_updateView = false;
				_view.View(this);
			}
		}

		//
		// < Clear >
		// 

		protected void _ClearEdges()
		{
			foreach (var e in _edges)
				e.RemoveEdge(this);
			_edges.Clear();
		}

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
