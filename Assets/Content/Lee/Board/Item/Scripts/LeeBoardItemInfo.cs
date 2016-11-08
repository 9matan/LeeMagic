using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public interface ILeeBoardItemInfo
	{
		Sprite GetSpriteByType(ELeeBoardItemType type);
		Color GetColorByState(ELeeBoardItemState state);
		Sprite GetSpriteByTrackType(ELeeBoardItemTrackType type);
	}

	[CreateAssetMenu(menuName = "LeeMagic/Board/Item Info")]
	public class LeeBoardItemInfo : ScriptableObject,
		ILeeBoardItemInfo
	{

		[SerializeField]
		protected LeeBoardItemStateInfoContainer _stateColors = new LeeBoardItemStateInfoContainer();
		[SerializeField]
		protected LeeBoardItemTypeInfoContainer _typeSprites = new LeeBoardItemTypeInfoContainer();
		[SerializeField]
		protected LeeBoardItemTrackTypeInfoContainer _tracks = new LeeBoardItemTrackTypeInfoContainer();

		public Sprite GetSpriteByTrackType(ELeeBoardItemTrackType type)
		{
			if (!_tracks.ContainsKey(type))
				return null;
			return _tracks[type];
		}

		public Sprite GetSpriteByType(ELeeBoardItemType type)
		{
			if (!_typeSprites.ContainsKey(type))
				return null;
			return _typeSprites[type];
		}

		public Color GetColorByState(ELeeBoardItemState state)
		{
			if (!_stateColors.ContainsKey(state))
				return Color.white;
			return _stateColors[state];
		}
	}

	[System.Serializable]
	public class LeeBoardItemStateInfoContainer : VOSSerializableDictionary<ELeeBoardItemState, Color>
	{
	}

	[System.Serializable]
	public class LeeBoardItemTypeInfoContainer : VOSSerializableDictionary<ELeeBoardItemType, Sprite>
	{
	}

	[System.Serializable]
	public class LeeBoardItemTrackTypeInfoContainer : VOSSerializableDictionary<ELeeBoardItemTrackType, Sprite>
	{
	}

}
