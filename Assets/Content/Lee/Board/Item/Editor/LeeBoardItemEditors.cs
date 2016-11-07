using UnityEngine;
using UnityEditor;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	[CustomPropertyDrawer(typeof(LeeBoardItemStateInfoContainer))]
	public class LeeBoardItemStateInfoContainerDrawer : VOSDictionaryDrawer<ELeeBoardItemState, Color>
	{

	}

	[CustomPropertyDrawer(typeof(LeeBoardItemTypeInfoContainer))]
	public class LeeBoardItemTypeInfoContainerDrawer : VOSDictionaryDrawer<ELeeBoardItemType, Sprite>
	{

	}

	[CustomPropertyDrawer(typeof(LeeBoardItemTrackTypeInfoContainer))]
	public class LeeBoardItemTrackTypeInfoContainerDrawer : VOSDictionaryDrawer<ELeeBoardItemTrackType, Sprite>
	{

	}

}
