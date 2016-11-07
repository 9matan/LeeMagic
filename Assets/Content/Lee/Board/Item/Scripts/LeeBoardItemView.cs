using UnityEngine;
using System.Collections;
using LeeMagic;

namespace LeeMagic
{

	public interface ILeeBoardItemView
	{

	}

	public class LeeBoardItemView : MonoBehaviour,
		ILeeBoardItemView,
		IVOSBuilder
	{

		public Sprite sprite
		{
			get { return _spriter.sprite; }
			set { _spriter.sprite = value; }
		}

		[SerializeField]
		protected SpriteRenderer _spriter;

		//
		// < View >
		//

		public void View(ILeeBoardItem item)
		{
			_ViewType(item);
			_ViewState(item);
		}

		protected void _ViewType(ILeeBoardItem item)
		{
			if (item.type == ELeeBoardItemType.TRACK)
				sprite = item.info.GetSpriteByTrackType(
					item.trackType);
			else
				sprite = item.info.GetSpriteByType(
					item.type);
		}

		protected void _ViewState(ILeeBoardItem item)
		{
			_spriter.color = item.info.GetColorByState(
				item.state);
		}

		//
		// </ View >
		//


		[ContextMenu("Build")]
		public void Build()
		{
			if (_spriter == null)
				_spriter = gameObject.CreateChild("Sprite").AddComponent<SpriteRenderer>();
		}		
	}
	
}
