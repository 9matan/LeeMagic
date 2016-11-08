using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LeeMagic;

namespace LeeMagic
{

	public class LeeTwoWaveAlgorithm : MonoBehaviour,
		IVOSBuilder
	{

		public class UsedItems : Dictionary<ILeeBoardItem, HashSet<ILeeBoardItem>>
		{
			public bool Contains(ILeeBoardItem a, ILeeBoardItem b)
			{
				return this[a].Contains(b) || this[b].Contains(a);
			}

			public void Add(ILeeBoardItem a, ILeeBoardItem b)
			{
				this[a].Add(b);
				this[b].Add(a);
			}
		}

		public class Ways : Dictionary<ILeeBoardItem, Dictionary<ILeeBoardItem, List<ILeeBoardItem>>>
		{
			public void AddWay(ILeeBoardItem a, ILeeBoardItem b, List<ILeeBoardItem> way)
			{
				this[a].Add(b, way);
				this[b].Add(a, way);
			}
		}

		public bool isRunning
		{
			get; protected set;
		}
		public bool stop
		{
			get; protected set;
		}
		public bool isValid
		{
			get; protected set;
		}

		[SerializeField]
		protected float _stepDelay = 0.1f;
		[SerializeField]
		protected float _pairDelay = 0.5f;

		protected ILeeBoard _board;
		protected UsedItems _usedPairs;
		protected Ways		_ways;

		public void Tracing(ILeeBoard __board)
		{
			isValid = true;
			isRunning = true;
			stop = false;

			_board = __board;
			_usedPairs = new UsedItems();
			_ways = new Ways();

			 StartCoroutine(_IEHandleElements());
		}		

		protected IEnumerator _IEHandleElements()
		{
			var items = _GetSortedElements(_board);

			foreach (var item in items)
			{
				foreach (var next in item.edges)
				{
					if (!_usedPairs.Contains(item, next))
					{
						yield return StartCoroutine(_IEHandlePair(
							item, next));
						_usedPairs.Add(item, next);
					}

					if (!isValid || stop)
						break;

					yield return new WaitForSeconds(_pairDelay);
				}

				if (!isValid || stop)
					break;

				yield return _pairDelay;
			}

			_Stop();
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


		//
		// < Pair >
		//

		protected List<Point2> _mask = new List<Point2>()
		{
			new Point2( -1, 0 ),
			new Point2( 1, 0 ),
			new Point2( 0, 1 ),
			new Point2( 0, -1 )
		};

		protected ILeeBoardItem _middlePoint;
		protected int[,]		_was;
		Queue<ILeeBoardItem>	_q;

		protected IEnumerator _IEHandlePair(ILeeBoardItem a, ILeeBoardItem b)
		{
			_ResetPairHandler();
			a.state = ELeeBoardItemState.SELECTED; b.state = ELeeBoardItemState.SELECTED;
			a.distance = 0; b.distance = 0;

			int cur = 0; bool exist = false;
			_q.Enqueue(a); _q.Enqueue(b);
			_was[a.row, a.column] = 1;
			_was[b.row, b.column] = -1;

			while (!exist && _q.Count != 0)
			{
				while (!exist && _q.Count != 0 && cur == _q.Peek().distance)
				{
					exist = ((_middlePoint = _HandleItem(_q.Dequeue())) != null);
				}

				yield return new WaitForSeconds(_stepDelay);
			}


			isValid = exist;

			if (exist)
			{
				_ways.AddWay(a, b, 
					_RestorePath(a, b, _middlePoint));
			}

			a.state = ELeeBoardItemState.DEFAULT; b.state = ELeeBoardItemState.DEFAULT;
		}

		protected ILeeBoardItem _HandleItem(ILeeBoardItem item)
		{
			int r = item.row;
			int c = item.column;

			for(int i = 0; i < _mask.Count; ++i)
			{
				if (_HandleNeighbor(item, r + _mask[i].row, c + _mask[i].column))
					return _board[r + _mask[i].row, c + _mask[i].column];
			}

			return null;
		}

		protected bool _HandleNeighbor(ILeeBoardItem parent, int row, int column)
		{
			if (_board.IsValid(row, column) || !_board[row, column].isEmpty) return false;

			var item = _board[row, column];

			if (_was[item.row, item.column] != 0)
				return true;

			_was[item.row, item.column] = _was[parent.row, parent.column];
			item.distance = Mathf.Min(item.distance, parent.distance + 1);
			item.state = ELeeBoardItemState.VISITED;
			_q.Enqueue(item);

			return false;
		}




		protected void _ResetPairHandler()
		{
			_ResetItems();

			_was = new int[_board.rows, _board.columns];
			_q = new Queue<ILeeBoardItem>();
		}

		protected void _ResetItems()
		{
			for (int i = 0; i < _board.rows; ++i)
			{
				for (int j = 0; j < _board.columns; ++j)
				{
					_board[i, j].distance = 100000;
					_board[i, j].state = ELeeBoardItemState.DEFAULT;
				}
			}
		}
		
		// < restore >

		protected List<ILeeBoardItem> _RestorePath(ILeeBoardItem a, ILeeBoardItem b, ILeeBoardItem middle)
		{
			var mToa = new List<ILeeBoardItem>();
			StartCoroutine(_RestorePath(middle, a, -1, mToa, 1));
			var mTob = new List<ILeeBoardItem>();
			StartCoroutine(_RestorePath(middle, b, -1, mTob, -1));

			mToa.RemoveAt(0);
			mToa.Add(a);
			mTob.Add(b);

			mToa.Reverse();
			mToa.AddRange(mTob);

			return mToa;
		}

		// from 0 - left, 1 - right, 2 - bottom, 3 - top
		protected IEnumerator _RestorePath(ILeeBoardItem current, ILeeBoardItem target, int from, List<ILeeBoardItem> path, int wasid)
		{
			if (current == target)
				yield return null;
			if(current.isEmpty)
				current.state = ELeeBoardItemState.TRACKED;
			yield return new WaitForSeconds(_stepDelay);

			path.Add(current);

			List<ILeeBoardItem> items = new List<ILeeBoardItem>();

			int[] dir = new int[4];
			for (int i = 0; i < dir.Length; ++i)
				dir[i] = -1;

			int mn = 100000;

			for (int i = 0; i < _mask.Count; ++i)
			{
				var r = current.row + _mask[i].row;
				var c = current.column + _mask[i].column;

				if (_board.IsValid(r, c) && _was[r, c] == wasid)
					mn = Mathf.Min(mn, _board[r, c].distance);
			}

			for (int i = 0; i < _mask.Count; ++i)
			{
				var r = current.row + _mask[i].row;
				var c = current.column + _mask[i].column;

				if (_board.IsValid(r, c) && mn == _board[r, c].distance && _was[r, c] == wasid)
				{
					dir[_MoveTo(current, _board[r, c])] = items.Count;
					items.Add(_board[r, c]);
				}
			}

			if (from < 0)
				yield return StartCoroutine(
					_RestorePath(items[0], target, _MoveTo(current, items[0]), path, wasid));
			else
			{
				if(dir[from] >= 0)
					yield return StartCoroutine(
						_RestorePath(items[dir[from]], target, from, path, wasid));
				else
					yield return StartCoroutine(
						_RestorePath(items[0], target, _MoveTo(current, items[0]), path, wasid));
			}
		}

		protected int _MoveTo(ILeeBoardItem current, ILeeBoardItem next)
		{
			if (current.column == next.column)
			{
				return ((current.row < next.row)?(2):(3));
			}
			else
				return ((current.column < next.column) ? (0) : (1));
		}

		// </ restore >

		//
		// </ Pair >
		//


		protected void _Stop()
		{


			isRunning = false;
		}


	/*	public void FindWay(ILeeBoardItem itemA, ILeeBoardItem itemB)
		{

		}
		*/
		
		
		[ContextMenu("Build")]
		public void Build()
		{
		
		}		


		public struct Point2
		{
			public int row;
			public int column;

			public Point2(int __row, int __column)
			{
				row = __row;
				column = __column;
			}
		}
	}
	
}
