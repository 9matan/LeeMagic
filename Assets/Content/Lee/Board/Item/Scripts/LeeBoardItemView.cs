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
		protected SpriteRenderer	_spriter;
		[SerializeField]
		protected TextMesh			_idText;
		[SerializeField]
		protected TextMesh			_distanceText;

		//
		// < View >
		//

		public void View(ILeeBoardItem item)
		{
			_ViewType(item);
			_ViewState(item);
			_ViewText(item);
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

		protected void _ViewText(ILeeBoardItem item)
		{
			if (item.type == ELeeBoardItemType.ELEMENT)
				_idText.text = item.elementId.ToString();
			else
				_idText.text = string.Empty;

			_distanceText.text = item.distance.ToString();
		}

		//
		// </ View >
		//


		[ContextMenu("Build")]
		public void Build()
		{
			if (_spriter == null)
				_spriter = gameObject.CreateChild("Sprite").AddComponent<SpriteRenderer>();
			if (_idText == null)
				_idText = gameObject.CreateChild("IdText").AddComponent<TextMesh>();
			if (_distanceText == null)
				_distanceText = gameObject.CreateChild("DistanceText").AddComponent<TextMesh>();
		}		
	}
	
}
