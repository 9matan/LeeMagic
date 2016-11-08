using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VVOSS.D2d
{

	using Events;

	namespace Events
	{
		public delegate void OnMap2dAction<TItem>(IVOSMap2d<TItem> map) where TItem : IVOSMap2dItem;
		public delegate void OnMapItem2dAction<TItem>(TItem item) where TItem : IVOSMap2dItem;
	}

	public interface IVOSMap2d<TItem> :
		IVOSMap2d
		where TItem : IVOSMap2dItem
	{
		TItem this[int row, int column] { get; set; }
		TItem selecetedItem { get; }

		void SetItem(int row, int column, TItem item);
		bool SetItem(Vector2 worldPosition, TItem item);

		TItem GetItem(int row, int column);
		TItem GetItem(Vector2 worldPosition);
	}

	public interface IVOSMap2dEvents<TItem>
		 where TItem : IVOSMap2dItem
	{
		event OnMapItem2dAction<TItem> onSelected;
	}

	public class VOSMap2d<TItem> : VOSMap2d,
		IVOSMap2d<TItem>
		where TItem :  class, IVOSMap2dItem 
	{

		public virtual TItem this [int row, int column]
		{
			get { return GetItem(row, column); }
			set { SetItem(row, column, value); }
		}

		public TItem selecetedItem
		{
			get; protected set;
		}

		protected VOSField2D<TItem> _field;

		public override void Initialize()
		{
			base.Initialize();

			_field = new VOSField2D<TItem>(_rows, _columns);
		}

		public virtual void SetItem(int row, int column, TItem item)
		{
			_grid.SetItem(row, column, item.transform);
			_field[row, column] = item;
		}

		public virtual bool SetItem(Vector2 worldPosition, TItem item)
		{
			int row = _grid.GetRowByWorldPosition(worldPosition);
			int column = _grid.GetColumnByWorldPosition(worldPosition);

			if (row < 0 || column < 0)
				return false;

			SetItem(row, column, item);
			return true;
		}
		
		public virtual TItem GetItem(int row, int column)
		{
			return _field[row, column];
		}

		public virtual TItem GetItem(Vector2 worldPosition)
		{
			int row = _grid.GetRowByWorldPosition(worldPosition);
			int column = _grid.GetColumnByWorldPosition(worldPosition);

			if (row < 0 || column < 0)
				return null;

			return _field[row, column];
		}

		//
		// < Events >
		//

		public event OnMapItem2dAction<TItem> onSelected = delegate { };

		protected override void _OnSeleceted(IVOSGrid2dPointer pointer)
		{
			base._OnSeleceted(pointer);
			selecetedItem = GetItem(
				pointer.selectedRow, pointer.selectedColumn);
			onSelected(selecetedItem);
		}

		//
		// </ Events >
		//

		public void RelocateItems()
		{
			for(int i = 0; i < rows; ++i)
			{
				for (int j = 0; j < columns; ++j)
				{
					if(_field[i,j] != null)
						_grid.SetItem(i, j, _field[i, j].transform);
				}
			}
		}

		//
		// < Clear >
		//

		protected void _ClearEvents()
		{
			onSelected = delegate { };
		}

		public override void Clear()
		{
			_ClearEvents();

			base.Clear();
		}

		//
		// </ Clear >
		//

	}

	public interface IVOSMap2dItem :
		IVOSTransformable
	{
	}




	//
	// < Map2d >
	//

	public interface IVOSMap2d
	{
		int			rows { get; }
		int			columns { get; }
		Vector2		size { get; }
		IVOSGrid2d	grid { get; }

		bool IsValid(int row, int column);
	}

	public class VOSMap2d : MonoBehaviour,
		IVOSMap2d,
		IVOSBuilder
	{

		public int			rows
		{
			get { return _rows; }
		}
		public int			columns
		{
			get { return _columns; }
		}
		public Vector2		size
		{
			get { return _size; }
			set
			{
				_size = value;
				_grid.size = value;
			}
		}
		public IVOSGrid2d	grid
		{
			get { return _grid; }
		}

		[SerializeField]
		protected int _rows;
		[SerializeField]
		protected int _columns;
		[SerializeField]
		protected Vector2 _size;
		[SerializeField]
		protected VOSGrid2d _grid;

		[SerializeField]
		protected VOSGrid2dPointer _gridPointer;

		//
		// < Initialize >
		//

		public virtual void Initialize()
		{
			_InitializePointer();
		}

		protected void _InitializePointer()
		{
			if (_gridPointer != null)
			{
				_gridPointer.Initialize(this);
				_ListenPointer(_gridPointer);
			}
		}

		protected void _ListenPointer(IVOSGrid2dPointerEvents events)
		{
			events.onSelected += _OnSeleceted;
		}

		//
		// </ Initialize >
		//

		//
		// < Events >
		//

		protected virtual void _OnSeleceted(IVOSGrid2dPointer pointer)
		{
			
		}

		//
		// </ Events >
		//

		public bool IsValid(int row, int column)
		{
			return row >= 0 && row < rows && column >= 0 && column < columns;
		}

		//
		// < Clear >
		//

		public virtual void Clear()
		{
			_gridPointer.Clear();
		}

		//
		// </ Clear >
		//

		[ContextMenu("Build")]
		public virtual void Build()
		{
			_grid = gameObject.Build(_grid, "Grid");

			if (_gridPointer != null)
				_gridPointer.Build();
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

	//
	// </ Map2d >
	//

}
