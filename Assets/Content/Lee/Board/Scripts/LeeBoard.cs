using UnityEngine;
using System.Collections;
using LeeMagic;
using VVOSS.D2d;

namespace LeeMagic
{

	public class LeeBoard : VVOSS.D2d.VOSMap2d<LeeBoardItem>,
		IVOSBuilder
	{

		protected void Awake()
		{
			Initialize();
		}


		protected override void _OnSeleceted(IVOSGrid2dPointer pointer)
		{
			base._OnSeleceted(pointer);

			Debug.Log("Selected: " + pointer.selectedRow + " " + pointer.selectedColumn);
		}


		[ContextMenu("Build")]
		public override void Build()
		{
			base.Build();
		}
	}
	
}
