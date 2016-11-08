using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VVOSS.UI
{

	public interface IVOSDropdown
	{
		Dropdown	dropdown { get; }
		int			selectedItem { get; }

		void AddOption(string option);
	}

	[RequireComponent(typeof(Dropdown))]
	public class VOSDropdown : VOSUIItem,
		IVOSDropdown,
		VOSPlayerPrefs.IVOSPlayerPrefSaver,
		IVOSInitializable
	{

		public Dropdown dropdown
		{
			get
			{
				if (_dropdown == null)
					_dropdown = GetComponent<Dropdown>();
				return _dropdown;
			}
		}

		public int		selectedItem
		{
			get { return _dropdown.value; }
			set { _dropdown.value = value; }
		}

		[SerializeField]
		protected VOSPlayerPrefs.IntItemInfo _pref;
		[SerializeField]
		protected bool _initOnAwake = false;

		protected Dropdown _dropdown;

		protected virtual void Awake()
		{
			if(_initOnAwake)
				Initialize();
		}

		public override void Initialize(IVOSUI __ui)
		{
			base.Initialize(__ui);
			Initialize();
		}

		public virtual void Initialize()
		{
			_dropdown = GetComponent<Dropdown>();

			_InitializePref();
			_InitializeValue();
		}


		protected void _InitializePref()
		{
			_pref.Initialize(gameObject, 0, true);
		}

		protected void _InitializeValue()
		{
			if (_pref.usePlayerPref)
				selectedItem = _pref.prefValue;
		}



		public void AddOption(string option)
		{
			dropdown.options.Add(
				new Dropdown.OptionData(option));
		}



		public virtual void SavePlayerPref()
		{
			_pref.prefValue = selectedItem;
			_pref.SavePref();
		}

		public virtual void LoadPlayerPref()
		{
			selectedItem = _pref.LoadPref();
		}

	}

}
