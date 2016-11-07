using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

namespace VVOSS.D2d
{

	public interface IVOSRectLocator2d :
		IEVOSRectLocator2d
	{
		Vector2 GetLocatorWorldSize();

		int GetRowsByIndex(int index);
		int GetColumnsByIndex(int index);

		Vector2 GetWorldPositionByIndex(int index);
		Vector2 GetWorldPosition(int row, int column);

		int GetIndexByRC(int row, int column);
		int GetIndexByWorldPosition(Vector2 worldPosition);

		int GetRowByIndex(int index);
		int GetRowByWorldPosition(Vector2 worldPosition);

		int GetColumnByIndex(int index);
		int GetColumnByWorldPosition(Vector2 worldPosition);
	}

	public interface IEVOSRectLocator2d
	{
#if UNITY_EDITOR
		int gizmosIndices { get; set; }

		void DrawGizmosItemsByIndices(int indices);
		void DrawGizmosItemByIndex(int index, Color dcolor);
#endif
	}

	// 0-base
	public class VOSRectLocator2d : MonoBehaviour,
		IVOSRectLocator2d,
		IVOSBuilder
	{

		public enum EOrientation
		{
			VERTICAL,
			HORIZONTAL
		}

		public EOrientation orientation
		{
			get { return _orientation; }
			set { _orientation = value; }
		}
		public Vector2 size
		{
			get { return _size; }
			set { _size = value; }
		}
		public Vector2 delta
		{
			get { return _delta; }
			set { _delta = value; }
		}
		public int rows
		{
			get { return _rows; }
			set
			{
				_orientation = EOrientation.HORIZONTAL;
				_rows = value;
			}
		}
		public int columns
		{
			get { return _columns; }
			set
			{
				_orientation = EOrientation.VERTICAL;
				_columns = value;
			}
		}

		public new Transform transform
		{
			get
			{
				if (Equals(null))
					return gameObject.AddComponent<Transform>();
				return base.transform;
			}
		}

		[SerializeField]
		protected EOrientation _orientation;
		[SerializeField]
		protected Vector2 _size;
		[SerializeField]
		protected Vector2 _delta;

		[SerializeField]
		protected int _rows;
		[SerializeField]
		protected int _columns;


		public Vector2 GetLocatorWorldSize()
		{
			Vector2 sz = Vector2.zero;

			if (_orientation == EOrientation.VERTICAL)
			{
				sz.x = columns * (_size.x + _delta.x) - _delta.x;
			}
			else
			{
				sz.y = rows * (_size.y + _delta.y) - _delta.y;
			}

			return transform.TransformVector(sz);
		}
	
		public int GetRowsByIndex(int index)
		{
			int currows = 0;

			if (_orientation == EOrientation.VERTICAL)
				currows = index / columns + 1;
			else
				currows = rows;

			return currows;
		}

		public int GetColumnsByIndex(int index)
		{
			int curcolumns = 0;

			if (_orientation == EOrientation.VERTICAL)
				curcolumns = columns;
			else
				curcolumns = rows / index + 1;

			return curcolumns;
		}

		//
		// < Position >
		//

		public Vector2 GetWorldPositionByIndex(int index)
		{
			return GetWorldPosition(
				GetRowByIndex(index),
				GetColumnByIndex(index)
				);
		}

		protected Vector2 _GetLocalPositionByIndex(int index)
		{
			return _GetLocalPosition(
				GetRowByIndex(index),
				GetColumnByIndex(index)
				);
		}

		protected float _GetLocalYByRow(int row)
		{
			return _size.y * 0.5f + (float)(row) * (_delta.y + _size.y);
		}

		protected float _GetLocalXByColumn(int column)
		{
			return _size.x * 0.5f + (float)(column) * (_delta.x + _size.x);
		}

		//		^
		//		|
		//		|
		//		|
		// (0;0)+----->
		public Vector2 GetWorldPosition(int row, int column)
		{
			return transform.TransformPoint(_GetLocalPosition(row, column));
		}

		protected Vector2 _GetLocalPosition(int row, int column)
		{
			var res = _size * 0.5f;

			res.x += (float)(column) * (_delta.x + _size.x);
			res.y += (float)(row) * (_delta.y + _size.y);

			return res;
		}

		//
		// </ Position >
		//
		
		//
		// < Index >
		//

		public int GetIndexByRC(int row, int column)
		{
			if (row < 0 || column < 0) return -1;
			int index = 0;

			if (_orientation == EOrientation.VERTICAL)
			{
				index = row * _columns + column;
			}
			else
			{
				index = column * _rows + row;
			}

			return index;
		}

		public int GetIndexByWorldPosition(Vector2 worldPosition)
		{
			return GetIndexByRC(
				GetRowByWorldPosition(worldPosition),
				GetColumnByWorldPosition(worldPosition));
		}

		//
		// </ Index >
		//

		//
		// < Row >
		//

		// 0-base
		public int GetRowByIndex(int index)
		{
			int row = 0;

			if (_orientation == EOrientation.VERTICAL)
				row = index / _columns;
			else
				row = index % _rows;

			return row;
		}

		public int GetRowByWorldPosition(Vector2 worldPosition)
		{
			int row = -1;

			var pos = transform.InverseTransformPoint(worldPosition);

			if (pos.y >= 0.0f)
			{
				row = (int)((pos.y) / (_size.y + _delta.y));
				if (!_IsRowHere(row, pos.y))
					row = -1;
			}

			if (_orientation == EOrientation.HORIZONTAL && row >= _rows)
				row = -1;

			return row;
		}

		//
		// </ Row >
		//

		//
		// < Column >
		//

		// 0-base
		public int GetColumnByIndex(int index)
		{
			int column = 0;

			if (_orientation == EOrientation.VERTICAL)
				column = index % _columns;
			else
				column = index / _rows;

			return column;
		}

		public int GetColumnByWorldPosition(Vector2 worldPosition)
		{
			int column = -1;

			var pos = transform.InverseTransformPoint(worldPosition);

			if (pos.x >= 0.0f)
			{
				column = (int)((pos.x) / (_size.x + _delta.x));
				if (!_IsColumnHere(column, pos.x))
					column = -1;
			}

			if (_orientation == EOrientation.VERTICAL && column >= _columns)
				column = -1;

			return column;
		}

		//
		// </ Column >
		//
		
		protected bool _IsRowHere(int row, float localY)
		{
			var pos = _GetLocalYByRow(row);
			return pos - _size.y * 0.5f <= localY && localY <= pos + _size.y * 0.5f;
		}

		protected bool _IsColumnHere(int column, float localX)
		{
			var pos = _GetLocalXByColumn(column);
			return pos - _size.x * 0.5f <= localX && localX <= pos + _size.x * 0.5f;
		}

		[ContextMenu("Build")]
		public void Build()
		{

		}

#if UNITY_EDITOR

		public int gizmosIndices
		{
			get { return _gizmosIndices; }
			set { _gizmosIndices = value; }
		}

		[SerializeField]
		protected int _gizmosIndices = 0;
		[SerializeField]
		protected Color _gizmosColor = Color.black;

		protected virtual void OnDrawGizmos()
		{
			DrawGizmosItemsByIndices(_gizmosIndices);
		}

		public void DrawGizmosItemsByIndices(int indices)
		{
			for (int i = 0; i < indices; ++i)
				DrawGizmosItemByIndex(i, _gizmosColor);
		}

		public void DrawGizmosItemByIndex(int index, Color dcolor)
		{
			var matrix = Gizmos.matrix;
			var color = Gizmos.color;

			Gizmos.color = dcolor;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(
				_GetLocalPositionByIndex(index), 
				_size);

			Gizmos.color = color;
			Gizmos.matrix = matrix;
		}

#endif

	}

}
