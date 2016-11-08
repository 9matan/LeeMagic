using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public interface ILeeBoardEditor
	{
		ILeeBoard board { get; }

		bool active { get; set; }
		void Reset();
	}

	public class LeeBoardEditor : MonoBehaviour,
		ILeeBoardEditor,
		IVOSBuilder
	{

		public ILeeBoard board
		{
			get; protected set;
		}

		public bool active
		{
			get { return _active; }
			set { SetActive(value); }
		}

		[SerializeField]
		protected LeeBoardItemEditor _itemEditor;

		[SerializeField]
		protected bool _active;

		//
		// < Initialize >
		//

		public void Initialize(ILeeBoard __board)
		{
			board = __board;

			_itemEditor.Initialize();
			_ListenBoard(board);
		}

		protected void _ListenBoard(ILeeBoardEvents events)
		{
			events.onSelected += _OnItemSelected;
		}

		//
		// </ Initialize >
		//

		public void SetActive(bool __active)
		{
			_active = __active;
			if (!_active)
				Reset();
		}

	//
	// < Board events >
	//

	protected void _OnItemSelected(LeeBoardItem item)
		{
			if (!_active) return;

			if (Input.GetMouseButtonDown(0))
				_SelectItem(item);
			else
				_AddEdge(item);
		}

		//
		// </ Board events >
		//

		protected void _SelectItem(LeeBoardItem item)
		{
			_itemEditor.gameObject.Show();
			_itemEditor.EditItem(item);
		}

		protected void _AddEdge(LeeBoardItem item)
		{
			_itemEditor.AddEdge(item);
		}

		public void Reset()
		{
			_itemEditor.EditItem(null);
			_itemEditor.gameObject.Hide();
		}

		[ContextMenu("Build")]
		public void Build()
		{
			_itemEditor = gameObject.Build(_itemEditor, "ItemEditor");
		}		
	}
	
}
