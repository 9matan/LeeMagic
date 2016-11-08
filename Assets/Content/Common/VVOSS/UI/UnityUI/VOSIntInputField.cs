using UnityEngine;
using System.Collections;
using System;

namespace VVOSS.UI
{

	public interface IVOSIntInputField : IVOSInputField<int>
	{

	}

	public class VOSIntInputField : VOSInputField<int>,
		IVOSIntInputField,
		VOSPlayerPrefs.IVOSPlayerPrefSaver
	{

		public bool useValidator
		{
			get { return _useValidator; }
			set { _useValidator = value; }
		}

		[SerializeField]
		protected bool	_useValidator = false;
		[SerializeField]
		protected int	_minValue = -100;
		[SerializeField]
		protected int	_maxValue = 100;
		[SerializeField]
		protected int	_defaultValue = 0;
		[SerializeField]
		protected VOSPlayerPrefs.IntItemInfo _pref;

		//
		// < Initialize >
		//

		public override void Initialize()
		{
			base.Initialize();

			_InitializePref();
			_InitializeFieldValue();

			_field.onEndEdit.AddListener(_OnEndEdit);
		}

		protected void _InitializePref()
		{
			_pref.Initialize(gameObject, _defaultValue, true);
		}

		protected void _InitializeFieldValue()
		{
			if (_pref.usePlayerPref)
				fieldValue = _pref.prefValue;
			else
				fieldValue = _defaultValue;
		}

		//
		// </ Initialize >
		//

		public override int GetFieldValue()
		{
			return System.Convert.ToInt32(text);
		}

		public override void SetFieldValue(int __fiedValue)
		{
			text = __fiedValue.ToString();
		}



		protected virtual void _OnEndEdit(string __text)
		{
			if(_useValidator)
				Validate();
		}




		public virtual void Validate()
		{
			var val = fieldValue;

			val = Math.Max(val, _minValue);
			val = Math.Min(val, _maxValue);

			fieldValue = val;
		}

		public virtual void SavePlayerPref()
		{
			_pref.prefValue = fieldValue;
			_pref.SavePref();
		}
		
		public virtual void LoadPlayerPref()
		{
			fieldValue = _pref.LoadPref();
		}

	}

}
