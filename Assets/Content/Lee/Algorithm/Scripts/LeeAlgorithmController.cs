using UnityEngine;
using System.Collections;
using LeeMagic;
using System.Collections.Generic;

namespace LeeMagic
{

	public interface ILeeBoardController
	{
		ILeeBoardItem currentFrom { get; }
		ILeeBoardItem currentTo { get; }
		ILeeBoard currentBoard { get; }

		void NextBoard();
		void ResetCurrentBoard();
		void SetCurrentBoard(int index);
		void SetWay(LeeAlgorithmWay way);
		ELeeAlgorithmNextEdgeResult NextEdge();
		bool IsUsed(ILeeBoardItem item);

		void OnStop();
	}

	public interface ILeeAlgorithm
	{
		bool stop { get; set; }
		bool isRunning { get; }

		void Tracing(ILeeBoardController controller);
	}

	public enum ELeeAlgorithmNextEdgeResult
	{
		STOP,
		EDGE,
		ITEM
	}

	public class LeeAlgorithmWay
	{
		public LeeAlgorithmWay(ILeeBoardItem __from, ILeeBoardItem __to, List<ILeeBoardItem> __way)
		{
			to = __to;
			from = __from;
			way = __way;
		}

		public ILeeBoardItem from;
		public ILeeBoardItem to;
		public ILeeBoard board;

		public List<ILeeBoardItem> way = new List<ILeeBoardItem>();

		public void AddToWay(ILeeBoardItem item)
		{
			way.Add(item);
		}

	}

	public class LeeAlgorithmController : MonoBehaviour,
		ILeeBoardController,
		IVOSBuilder
	{

		public bool active
		{
			get { return _active; }
			set { SetActive(value); }
		}

		[SerializeField]
		protected LeeTwoWaveAlgorithm _towWaveAlgo;
		[SerializeField]
		protected GameObject _uiPanel;

		protected ILeeAlgorithm _currentAlgo;
		protected LeeBoard _board;
		protected bool _active;
		protected ItemInfo[,] _items;

		public void Initialize()
		{

		}

		public void SetBoard(LeeBoard board)
		{
			_board = board;
			_Copy();
		}

		public void StartTwoWayAlgo()
		{
			_Copy();
			_StartAlgo(_towWaveAlgo);
		}



		public void SetActive(bool __active)
		{
			if (!__active)
			{
				//	_Revert();
				ResetController();
				_uiPanel.Hide();
				_currentAlgo = null;
			}
			else
			{
				_uiPanel.Show();
			}

			_active = __active;
		}

		public void Stop()
		{
			_StopAlgo();
		}



		protected void _Copy()
		{
			_items = _CreateCopy(_board);
		}

		protected void _Revert()
		{
			_Revert(_items, _board);
		}



		protected ItemInfo[,] _CreateCopy(ILeeBoard board)
		{
			ItemInfo[,] items = new ItemInfo[board.rows, board.columns];

			for (int i = 0; i < board.rows; ++i)
			{
				for (int j = 0; j < board.columns; ++j)
				{
					items[i, j] = new ItemInfo(board[i, j]);
					items[i, j].elementId = board[i, j].elementId;
				}
			}

			return items;
		}

		protected void _Revert(ItemInfo[,] items, ILeeBoard board)
		{
			for (int i = 0; i < board.rows; ++i)
			{
				for (int j = 0; j < board.columns; ++j)
				{
					items[i, j].CopyTo(board[i, j]);
					board[i, j].state = ELeeBoardItemState.DEFAULT;
				}
			}
		}

		//
		// < Board controller >
		//			

		public class Ways : Dictionary<ILeeBoardItem, Dictionary<ILeeBoardItem, List<LeeAlgorithmWay>>>
		{
			public void AddWay(ILeeBoardItem a, ILeeBoardItem b, LeeAlgorithmWay way)
			{
				if (!ContainsKey(a))
					Add(a, new Dictionary<ILeeBoardItem, List<LeeAlgorithmWay>>());
				if (!ContainsKey(b))
					Add(b, new Dictionary<ILeeBoardItem, List<LeeAlgorithmWay>>());

				if (!this[a].ContainsKey(b))
					this[a].Add(b, new List<LeeAlgorithmWay>());
				if (!this[b].ContainsKey(a))
					this[b].Add(a, new List<LeeAlgorithmWay>());

				this[a][b].Add(way);
				this[b][a].Add(way);
			}
		}


		public ILeeBoardItem currentFrom
		{
			get
			{
				return currentBoard
				[
					_sortedItems[_currentItem].row,
					_sortedItems[_currentItem].column
				];
			}
		}

		public ILeeBoardItem currentTo
		{
			get
			{
				var to = _sortedItems[_currentItem].GetEdge(_currentEdge);

				return currentBoard
				[
					to.row,
					to.column
				];
			}
		}

		public ILeeBoard currentBoard
		{
			get { return _boards[_currentBoardIndx]; }
		}

		protected bool[,] _used;
		protected Ways _ways;

		protected List<LeeBoard> _boards = new List<LeeBoard>();
		protected int _currentBoardIndx = -1;

		protected List<ILeeBoardItem> _sortedItems;
		protected int _currentItem;
		protected int _currentEdge;

		protected void _StartAlgo(ILeeAlgorithm algo)
		{
			_currentAlgo = algo;
			_board.gameObject.Hide();

			_PreprocBoardItems();

			algo.Tracing(this);
		}

		protected void _PreprocBoardItems()
		{
			_AddNewBoard();
			ResetCurrentBoard();

			_ways = new Ways();
			_used = new bool[_board.rows, _board.columns];

			_sortedItems = _GetSortedElements(_board);
			_currentItem = 0;
			_currentEdge = -1;
		}

		protected void _AddNewBoard()
		{
			_boards.Add(
				_CreateNewBoard());
		}

		protected LeeBoard _CreateNewBoard()
		{
			var board = Instantiate(_board.gameObject).GetComponent<LeeBoard>();
			board.HideElements();
			board.Initialize();
			_Revert(_items, board);
			return board;
		}


		protected List<ILeeBoardItem> _GetSortedElements(ILeeBoard board)
		{
			var items = new List<ILeeBoardItem>();

			for (int i = 0; i < board.rows; ++i)
			{
				for (int j = 0; j < board.columns; ++j)
				{
					if (board[i, j].isElement)
						items.Add(board[i, j]);
				}
			}

			items.Sort((ILeeBoardItem a, ILeeBoardItem b) =>
			{
				if (a.edgesCount > b.edgesCount)
					return -1;
				return 1;
			});

			return items;
		}

		protected void _StopAlgo()
		{
			_currentAlgo.stop = true;
		}






		public void NextBoard()
		{
			if (_currentBoardIndx == _boards.Count - 1)
				_AddNewBoard();
			SetCurrentBoard(_currentBoardIndx + 1);
		}

		public void ResetCurrentBoard()
		{
			SetCurrentBoard(0);
		}

		public void SetCurrentBoard(int index)
		{
			if (_currentBoardIndx >= 0)
				_boards[_currentBoardIndx].gameObject.Hide();

			_boards[index].gameObject.Show();
			_currentBoardIndx = index;
		}

		public void SetWay(LeeAlgorithmWay way)
		{
			_ways.AddWay(currentFrom, currentTo, way);
		}

		public ELeeAlgorithmNextEdgeResult NextEdge()
		{
			var cur = _sortedItems[_currentItem];

			if (_currentEdge == cur.edgesCount - 1)
			{
				_used[cur.row, cur.column] = true;
				++_currentItem;
				_currentEdge = 0;

				if (_currentItem == _sortedItems.Count)
					return ELeeAlgorithmNextEdgeResult.STOP;
				else
					return ELeeAlgorithmNextEdgeResult.ITEM;
			}
			else
				++_currentEdge;

			return ELeeAlgorithmNextEdgeResult.EDGE;
		}

		public bool IsUsed(ILeeBoardItem item)
		{
			return _used[item.row, item.column];
		}

		public bool isRunning
		{
			get { return _currentAlgo.isRunning; }
		}

		public void OnStop()
		{

		}

		public void ResetController()
		{
			for (int i = 0; i < _boards.Count; ++i)
				Destroy(_boards[i].gameObject);

			_boards.Clear();
			_currentBoardIndx = -1;
			_currentItem = -1;
			_currentEdge = -1;
			_currentAlgo = null;
			_board.gameObject.Show();
		}

		//
		// </ Board controller >
		//

		//
		// < GUI >
		//

		protected void OnGUI()
		{
			float btnSize = 50.0f;

			float x = Screen.width - btnSize - 5.0f;
			float y = 5.0f;

			for (int i = 0; i < _boards.Count; ++i)
			{
				if (i == _currentBoardIndx)
					GUI.color = Color.red;
				else
					GUI.color = Color.white;

				if (GUI.Button(
					new Rect(
						x,
						y + i * (btnSize + 20.0f),
						btnSize,
						btnSize), (i + 1).ToString()))
				{
					SetCurrentBoard(i);
				}
			}
		}

		//
		// </ GUI >
		//

		[ContextMenu("Build")]
		public void Build()
		{

		}




		public struct ItemInfo
		{
			public ELeeBoardItemType type;
			public int elementId;

			public ItemInfo(ILeeBoardItem item)
			{
				elementId = -1;
				type = item.type;
			}

			public void CopyTo(ILeeBoardItem item)
			{
				if (elementId >= 0)
					item.elementId = elementId;
				item.type = type;
			}
		}

	}



}
