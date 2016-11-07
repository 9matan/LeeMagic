using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VVOSS.D2d
{

	public interface IVOSMap<TItem>
		where TItem : IVOSMapItem
	{
		TItem this[int row, int column] { get; set; }

		void SetItem(int row, int column, TItem item);
		bool SetItem(Vector2 worldPosition, TItem item);

		TItem GetItem(int row, int column);
		TItem GetItem(Vector2 worldPosition);
	}

	public class VOSMap<TItem> : MonoBehaviour,
		IVOSMap<TItem>
		where TItem :  class, IVOSMapItem 
	{

		public TItem this [int row, int column]
		{
			get { return GetItem(row, column); }
			set { SetItem(row, column, value); }
		}

		public int		rows
		{
			get { return _rows; }
		}
		public int		columns
		{
			get { return _columns; }
		}
		public Vector2	size
		{
			get { return _size; }
			set
			{
				_size = value;
				_grid.size = value;
			}
		}
		
		[SerializeField]
		protected int _rows;
		[SerializeField]
		protected int _columns;
		[SerializeField]
		protected Vector2 _size;

		[SerializeField]
		protected VOSGrid2d _grid;

		protected VOSField2D<TItem> _field;

		public void Initialize()
		{
			_field = new VOSField2D<TItem>(_rows, _columns);
		}

		public void SetItem(int row, int column, TItem item)
		{
			_grid.SetItem(row, column, item.transform);
			_field[row, column] = item;
		}

		public bool SetItem(Vector2 worldPosition, TItem item)
		{
			int row = _grid.GetRowByWorldPosition(worldPosition);
			int column = _grid.GetColumnByWorldPosition(worldPosition);

			if (row < 0 || column < 0)
				return false;

			SetItem(row, column, item);
			return true;
		}
		
		public TItem GetItem(int row, int column)
		{
			return _field[row, column];
		}

		public TItem GetItem(Vector2 worldPosition)
		{
			int row = _grid.GetRowByWorldPosition(worldPosition);
			int column = _grid.GetColumnByWorldPosition(worldPosition);

			if (row < 0 || column < 0)
				return null;

			return _field[row, column];
		}



		[ContextMenu("Build")]
		public void Build()
		{
			_grid = gameObject.Build(_grid, "Grid");
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()
		{
			_UpdateGrid();
		}

		protected void _UpdateGrid()
		{
			if (_grid == null)
				return;

			_grid.rows = _rows;
			_grid.columns = _columns;
			_grid.size = _size;
		}
#endif

	}

	public interface IVOSMapItem :
		IVOSTransformable
	{
	}

}
