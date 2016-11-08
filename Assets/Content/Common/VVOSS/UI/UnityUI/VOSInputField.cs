using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace VVOSS.UI
{

	public interface IVOSInputField<TValue> : IVOSUIItem
	{
		string text { get; set; }
		InputField field { get; }
		TValue fieldValue { get; set; }
	}

	[RequireComponent(typeof(InputField))]
	public abstract class VOSInputField<TValue> : VOSUIItem,
		IVOSInputField<TValue>,
		IVOSInitializable
	{

		public			string		text
		{
			get { return _field.text; }
			set { _field.text = value; }
		}
		public virtual	TValue		fieldValue
		{
			get { return GetFieldValue(); }
			set { SetFieldValue(value); }
		}
		public			InputField	field
		{
			get
			{
				if(_field == null)
					_field = GetComponent<InputField>();
				return _field;
			}
		}

		protected InputField _field;
		[SerializeField]
		protected bool _initOnAwake = false;

		protected virtual void Awake()
		{
			if (_initOnAwake)
				Initialize();
		}

		public override void Initialize(IVOSUI __ui)
		{
			base.Initialize(__ui);
			Initialize();
		}

		public virtual void Initialize()
		{
			_field = GetComponent<InputField>();
		}

		public abstract void	SetFieldValue(TValue __fiedValue);
		public abstract TValue	GetFieldValue();
	}

}
