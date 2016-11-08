using UnityEngine;
using System.Collections;

namespace VVOSS.UI
{

	public interface IVOSFloatInputField : IVOSInputField<float>
	{

	}

	public class VOSFloatInputField : VOSInputField<float>,
		IVOSFloatInputField,
		VOSPlayerPrefs.IVOSPlayerPrefSaver
	{

		[SerializeField]
		protected bool _useValidator = false;
		[SerializeField]
		protected float _minValue = -100.0f;
		[SerializeField]
		protected float _maxValue = 100.0f;
		[SerializeField]
		protected float _defaultValue = 0.0f;
		[Range(0, 10)]
		[SerializeField]
		protected int	_precision = 2;
		[SerializeField]
		protected VOSPlayerPrefs.FloatItemInfo _pref;

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

		public override float GetFieldValue()
		{
			return float.Parse(text);
		}

		public override void SetFieldValue(float __fiedValue)
		{
			text = __fiedValue.ToString(
				string.Concat("F", _precision));
		}



		protected virtual void _OnEndEdit(string __text)
		{
			if (_useValidator)
				Validate();
		}



		public virtual void Validate()
		{
			if (_ValidateFormat())
				_ValidateValue();
			else
				_ResetValue();
		}

		protected void _ValidateValue()
		{
			var val = fieldValue;

			val = System.Math.Max(val, _minValue);
			val = System.Math.Min(val, _maxValue);

			fieldValue = val;
		}

		protected bool _ValidateFormat()
		{
			float res = 0.0f;

			if (!float.TryParse(text, out res))
				return false;

			text = res.ToString(
				string.Concat("F", _precision));

			return true;
		}

		protected void _ResetValue()
		{
			text = "0";
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