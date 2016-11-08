using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public class LeeBoardManager : MonoBehaviour,
		IVOSBuilder
	{

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
