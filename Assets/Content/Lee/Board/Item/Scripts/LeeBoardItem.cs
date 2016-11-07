using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public interface ILeeBoardItem
	{

	}

	public class LeeBoardItem : MonoBehaviour,
		VVOSS.D2d.IVOSMap2dItem,
		ILeeBoardItem,
		IVOSBuilder
	{

		
		
		
		[ContextMenu("Build")]
		public void Build()
		{
		
		}		
	}
	
}
