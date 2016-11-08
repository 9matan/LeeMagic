using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public interface ILeeAlgorithm
	{
		bool stop { get; set; }

		void Tracing(ILeeBoard board);
	}

	public class LeeAlgorithmController : MonoBehaviour,
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
		protected ILeeBoard		_board;
		protected bool			_active;
		protected ItemInfo[,]	_items;
		
		public void Initialize()
		{

		}


		public void SetBoard(ILeeBoard board)
		{
			_board = board;
			_Copy();
		}

		public void StartTwoWayAlgo()
		{
			_currentAlgo = _towWaveAlgo;
			_currentAlgo.Tracing(_board);
		}

		public void SetActive(bool __active)
		{
			if (!__active)
			{
				_Revert();
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
			_currentAlgo.stop = true;
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


		[ContextMenu("Build")]
		public void Build()
		{
		
		}




		public struct ItemInfo
		{
			public ELeeBoardItemType type;

			public ItemInfo (ILeeBoardItem item)
			{
				type = item.type;
			}

			public void CopyTo(ILeeBoardItem item)
			{
				item.type = type;
			}
		}

	}
	


}
