using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace VVOSS.UI
{

	public interface IVOSToggle : IVOSUIItem
	{
		Toggle toggle { get; }
		bool isOn { get; set; }
	}

	[RequireComponent(typeof(Toggle))]
	public class VOSToggle : VOSUIItem,
		IVOSToggle,
		VOSPlayerPrefs.IVOSPlayerPrefSaver,
		IVOSInitializable
	{

		public Toggle	toggle
		{
			get { return _toggle; }
		}
		public bool		isOn
		{
			get { return _toggle.isOn; }
			set { _toggle.isOn = value; }
		}
		public int		isOnInt
		{
			get { return ((_toggle.isOn) ? (1) : (0)); }
			set { _toggle.isOn = ((value == 1) ? (true) : (false)); }
		}

		[SerializeField]
		protected bool _initOnAwake = false;
		[SerializeField]
		protected VOSPlayerPrefs.IntItemInfo _pref;		

		protected Toggle _toggle;

		protected void Awake()
		{
			if(_initOnAwake)
				Initialize();
		}

		public override void Initialize(IVOSUI __ui)
		{
			base.Initialize(__ui);
			Initialize();
		}

		public void Initialize()
		{
			_toggle = GetComponent<Toggle>();

			_pref.Initialize(gameObject, isOnInt, true);

			if (_pref.usePlayerPref)
				isOnInt = _pref.prefValue;
		}




		public virtual void SavePlayerPref()
		{
			_pref.prefValue = isOnInt;
			_pref.SavePref();
		}

		public virtual void LoadPlayerPref()
		{
			isOnInt = _pref.LoadPref();
		}



	}

}