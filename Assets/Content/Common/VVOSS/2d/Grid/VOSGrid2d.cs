using UnityEngine;
using System.Collections;

namespace VVOSS.D2d
{

	public class VOSGrid2d : MonoBehaviour,
		IVOSBuilder
	{

		public int		rows
		{
			get { return _rows; }
			set
			{
				_rows = value;
				_locator.rows = value;
			}
		}
		public int		columns
		{
			get { return _columns; }
			set
			{
				_columns = value;
				_locator.columns = value;
			}
		}
		public Vector2	size
		{
			get { return _size; }
			set
			{
				_size = value;
				_locator.size = value;
			}
		}


		[SerializeField]
		protected int _rows;
		[SerializeField]
		protected int _columns;
		[SerializeField]
		protected Vector2 _size;
	
		[SerializeField]
		protected VOSRectLocator2d _locator;

		public void SetItem(int row, int column, Transform tr)
		{
			tr.position = _locator.GetWorldPosition(row, column);
		}

		public int GetRowByWorldPosition(Vector2 worldPosition)
		{
			int row = _locator.GetRowByWorldPosition(worldPosition);
			if (row >= _rows)
				row = -1;

			return row;
		}

		public int GetColumnByWorldPosition(Vector2 worldPosition)
		{
			int column = _locator.GetColumnByWorldPosition(worldPosition);

			if (column >= _columns)
				column = -1;

			return column;
		}

		[ContextMenu("Build")]
		public void Build()
		{
			_locator = gameObject.Build(_locator, "Locator");
		}


#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()
		{
			_UpdateLocator();
		}

		protected void _UpdateLocator()
		{
			if (_locator == null)
				return;

			_locator.rows = _rows;
			_locator.size = _size;
			_locator.gizmosIndices = _rows * _columns;
		}
#endif

	}

}
