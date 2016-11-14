using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public interface ILeeBoardManager
	{
		ILeeBoardEditor boardEditor { get; }
	}

	public class LeeBoardManager : MonoBehaviour,
		ILeeBoardManager,
		IVOSBuilder
	{

		public ILeeBoardEditor boardEditor
		{
			get { return _boardEditor; }
		}
		public LeeBoard board
		{
			get { return _board; }
		}

		[SerializeField]
		protected LeeBoard _board;
		[SerializeField]
		protected LeeBoardEditor _boardEditor;

		protected void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			_board.Initialize();
			_boardEditor.Initialize(_board);
			_boardEditor.active = true;
		}

		[ContextMenu("Build")]
		public void Build()
		{

		}
	}

}
