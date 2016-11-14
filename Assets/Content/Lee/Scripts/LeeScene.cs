using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public class LeeScene : MonoBehaviour,
		IVOSBuilder
	{

		public bool isEditor
		{
			get { return _boardManager.boardEditor.active; }
			set { _boardManager.boardEditor.active = value; }
		}
		public bool isAlgo
		{
			get { return _algo.active; }
			set { _algo.active = value; }
		}

		[SerializeField]
		protected LeeBoardManager _boardManager;
		[SerializeField]
		protected LeeAlgorithmController _algo;

		public void Initialize()
		{
			_boardManager.Initialize();
			_algo.Initialize();
		}


		public void SwitchOnAlgo()
		{
			Debug.Log("Algo on");

			_algo.SetBoard(_boardManager.board);
			isEditor = false;
			isAlgo = true;
		}

		public void SwitchOnEditor()
		{
			Debug.Log("Editor on");

			isEditor = true;
			isAlgo = false;
		}

		[ContextMenu("Build")]
		public void Build()
		{

		}
	}

}
