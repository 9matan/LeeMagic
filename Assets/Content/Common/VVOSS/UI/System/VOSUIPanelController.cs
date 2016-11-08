using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VVOSS.UI
{

	public interface IVOSUIPanelController : IVOSUIItem
	{
		void SetActivePanel(string name);
		void SetActivePanelById(int id);
	}

	public class VOSUIPanelController : VOSUIItem,
		IVOSUIPanelController
	{

		[SerializeField]
		protected List<VOSUIPanel> _uiPanels;

		[SerializeField]
		protected EVOSUIItemState _initState = EVOSUIItemState.HIDE;

		//
		// < Initialize >
		//

		public override void Initialize(IVOSUI __ui)
		{
			base.Initialize(__ui);

			_InitializeUIPanels();
		}

		protected virtual void _InitializeUIPanels()
		{
			for (int i = 0; i < _uiPanels.Count; ++i)
			{
				_uiPanels[i].Initialize(ui);
				_uiPanels[i].state = _initState;
				_AddToHash(_uiPanels[i]);
			}

			_SetActivePanel(_uiPanels[0]);
		}

		//
		// </ Initialize >
		//

		protected IVOSUIPanel _currentPanel = null;

		protected Dictionary<string, IVOSUIPanel> _nameHash = new Dictionary<string, IVOSUIPanel>();
		protected Dictionary<int, IVOSUIPanel> _idHash = new Dictionary<int, IVOSUIPanel>();

		public virtual void SetActivePanel(string name)
		{
			_SetActivePanel(
				_nameHash[name]);
		}

		public virtual void SetActivePanelById(int id)
		{
			_SetActivePanel(
				_idHash[id]);
		}

		protected virtual void _SetActivePanel(IVOSUIPanel _panel)
		{
			_DeactivateCurrentPanel();

			_currentPanel = _panel;
			_currentPanel.state = EVOSUIItemState.SHOW;
		}

		protected void _DeactivateCurrentPanel()
		{
			if (_currentPanel != null)
				_currentPanel.state = EVOSUIItemState.HIDE;
		}

		protected void _AddToHash(IVOSUIPanel _panel)
		{
			_nameHash.Add(_panel.name, _panel);
			_idHash.Add(_panel.id, _panel);
		}

		//
		// < Clear >
		//

		protected void _ClearHash()
		{
			_nameHash.Clear();
			_idHash.Clear();
		}

		protected virtual void _ClearUIPanels()
		{
			_DeactivateCurrentPanel();

			for (int i = _uiPanels.Count - 1; i >= 0; --i)
				_uiPanels[i].Clear();

			_ClearHash();
		}

		public override void Clear()
		{
			_ClearUIPanels();

			base.Clear();
		}

		//
		// </ Clear >
		//

#if UNITY_EDITOR

		[ContextMenu("Build")]
		public override void Build()
		{
			_BuildPanels();
		}

		protected void _BuildPanels()
		{
			for (int i = 0; i < _uiPanels.Count; ++i)
				_uiPanels[i].Build();
		}

#endif

	}

}