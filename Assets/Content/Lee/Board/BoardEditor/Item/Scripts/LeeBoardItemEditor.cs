using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LeeMagic;
using VVOSS.UI;

namespace LeeMagic
{

	public interface ILeeBoardItemEditor
	{

	}

	public class LeeBoardItemEditor : MonoBehaviour,
		IVOSBuilder
	{

		public ILeeBoardItem item
		{
			get; protected set;
		}

		[SerializeField]
		protected VOSIntInputField	_elementId;
		[SerializeField]
		protected Button			_clearButton;
		[SerializeField]
		protected Button			_occupyButton;

		public int elementId
		{
			get { return _elementId.fieldValue; }
			set
			{
				_elementId.fieldValue = value;
			}
		}

		//
		// < Initialize >
		//

		public void Initialize()
		{
			_elementId.field.onEndEdit.AddListener(_OnIdChanged);
			_clearButton.onClick.AddListener(_OnClearClick);
			_occupyButton.onClick.AddListener(_OnOccupyClick);
		}

		protected void _UpdateUI()
		{
			if (item == null) return;

			elementId = item.elementId;
		}

		//
		// </ Initialize >
		//

		public void EditItem(ILeeBoardItem __item)
		{
			_ResetItem();

			item = __item;
			_elementId.transform.position = item.transform.position;
			_elementId.transform.localPosition = new Vector3(
				_elementId.transform.localPosition.x,
				_elementId.transform.localPosition.y,
				0.0f);

			_UpdateUI();
			_ActivateEdges();
		}		

		public void AddEdge(ILeeBoardItem __item)
		{
			if (item == null || !item.isElement || item == __item || !__item.isElement) return;

			if (!item.ContainsEdge(__item))
			{
				item.AddEdge(__item);
				__item.AddEdge(item);
				__item.state = ELeeBoardItemState.SELECTED;
			}
			else
			{
				item.RemoveEdge(__item);
				__item.RemoveEdge(item);
				__item.state = ELeeBoardItemState.DEFAULT;
			}
		}

		protected void _ActivateItem()
		{
			if (item == null) return;

			_ActivateEdges();
		}

		protected void _ActivateEdges()
		{
			foreach (var __item in item.edges)
			{
				__item.state = ELeeBoardItemState.SELECTED;
			}
		}

		protected void _ResetItem()
		{
			if (item == null) return;

			_ResetItemEdges();
		}

		protected void _ResetItemEdges()
		{
			item.state = ELeeBoardItemState.DEFAULT;
			foreach (var __item in item.edges)
			{
				__item.state = ELeeBoardItemState.DEFAULT;
			}
		}








		protected void _OnIdChanged(string text)
		{
			if (_elementId.useValidator)
				_elementId.Validate();

			item.elementId = elementId;
		}

		protected void _OnClearClick()
		{
			_ResetItemEdges();
			item.type = ELeeBoardItemType.EMPTY;
		}

		protected void _OnOccupyClick()
		{
			_ResetItemEdges();
			item.type = ELeeBoardItemType.OCCUPIED;
		}



		[ContextMenu("Build")]
		public void Build()
		{
		
		}		
	}
	
}
