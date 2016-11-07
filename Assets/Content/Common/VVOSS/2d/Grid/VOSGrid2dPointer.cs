using UnityEngine;
using System.Collections;

namespace VVOSS.D2d
{

	using Events;

	namespace Events
	{
		public delegate void OnMap2dPointerAction(IVOSGrid2dPointer pointer);
	}

	public interface IVOSGrid2dPointer :
		IVOSGrid2dPointerEvents
	{
		int selectedRow { get; }
		int selectedColumn { get; }
	}

	public interface IVOSGrid2dPointerEvents
	{
		event OnMap2dPointerAction onSelected;
	}

	public class VOSGrid2dPointer : MonoBehaviour,
		IVOSBuilder,
		IVOSGrid2dPointer
	{

		public IVOSMap2d map
		{
			get; protected set;
		}
		public IVOSGrid2d grid
		{
			get { return map.grid; }
		}
		public IVOSManipulator manipulator
		{
			get { return _manipulator; }
		}

		public Vector3 position
		{
			get { return _manipulator.ToWorldPosition(_camera); }
		}
		public int selectedRow
		{
			get; protected set;
		}
		public int selectedColumn
		{
			get; protected set;
		}

		public new Camera camera
		{
			get { return _camera; }
			set { _camera = value; }
		}

		protected Camera _camera;

		[SerializeField]
		protected VOSManipulator _manipulator;

		//
		// < Initialize >
		//

		public void Initialize(IVOSMap2d __map)
		{
			map = __map;

			_SetComponents();
			_ListenManipulator(manipulator);
		}

		protected void _SetComponents()
		{
			if (_camera == null)
				_camera = Camera.main;
		}

		protected void _ListenManipulator(IVOSManipulatorEvents events)
		{
			events.onPressed += _OnPressed;
		}

		//
		// </ Initialize >
		//

		//
		// < Manipulator events >
		//

		protected void _OnPressed(IVOSManipulator m)
		{
			selectedRow = _SelectedRow(position);
			selectedColumn = _SelectedColumn(position);

			if (selectedRow >= 0 && selectedColumn >= 0)
				_OnSelected();
		}

		//
		// </ Manipulator events >
		//

		protected int _SelectedRow(Vector2 worldPosition)
		{
			return grid.GetRowByWorldPosition(worldPosition);
		}

		protected int _SelectedColumn(Vector2 worldPosition)
		{
			return grid.GetColumnByWorldPosition(worldPosition);
		}

		//
		// < Events >
		//

		public event OnMap2dPointerAction onSelected = delegate { };

		protected void _OnSelected()
		{
			onSelected(this);
		}

		//
		// </ Events >
		//

		//
		// < Clear >
		//

		protected void _ClearEvents()
		{
			onSelected = delegate { };
		}

		public void Clear()
		{
			_ClearEvents();

			map = null;
		}

		//
		// </ Clear >
		//

		[ContextMenu("Build")]
		public void Build()
		{
			_manipulator = gameObject.BuildAtObject<VOSManipulator>(_manipulator);
		}

	}

}
