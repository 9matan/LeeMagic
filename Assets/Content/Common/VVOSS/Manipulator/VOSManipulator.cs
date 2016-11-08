using UnityEngine;
using System.Collections;

namespace VVOSS
{

	using Events;

	namespace Events
	{
		public delegate void OnManipulatorAction(IVOSManipulator control);
	}

	public interface IVOSManipulator :
		IVOSManipulatorEvents
	{
		Vector2 position { get; }
		bool isPressed { get; }		

		Vector3 ToWorldPosition(Camera camera);
	}

	public interface IVOSManipulatorEvents
	{
		event OnManipulatorAction onReleased;
		event OnManipulatorAction onPressed;
	}

	public class VOSManipulator : MonoBehaviour,
		IVOSManipulator,
		IVOSBuilder
	{
		

		public Vector2 position { get; protected set; }
		public bool isPressed { get; protected set; }

		public Vector3 ToWorldPosition(Camera camera)
		{
			return camera.ScreenToWorldPoint(position);
		}

		protected void Update()
		{
			var currentPress = _IsPressed();

		//	if (currentPress)
		//	{
				position = _GetPosition();
		//	}

			if (isPressed && !currentPress)
			{
				_OnReleased();
			}
			else if (!isPressed && currentPress)
			{
				_OnPressed();
			}

			isPressed = currentPress;
		}

		//
		// < Events >
		//

		public event OnManipulatorAction onReleased = delegate { };
		public event OnManipulatorAction onPressed = delegate { };

		protected void _OnReleased()
		{
			onReleased(this);
		}

		protected void _OnPressed()
		{
			onPressed(this);
		}

		//
		// </ Events >
		//

		protected Vector2 _GetPosition()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			return Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID
		return Input.GetTouch(0).position;
#endif
		}

		protected bool _IsPressed()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			return Input.GetMouseButton(0) || Input.GetMouseButton(1);
#elif UNITY_IOS || UNITY_ANDROID
		return Input.touchCount > 0;
#endif
		}



		public void Clear()
		{
			onPressed = delegate { };
			onReleased = delegate { };
		}



		[ContextMenu("Build")]
		public void Build()
		{

		}

#if UNITY_EDITOR

		protected Color _gizmosColor = Color.green;

		protected virtual void OnDrawGizmos()
		{
			_DrawPointer();
		}

		protected void _DrawPointer()
		{
			if (Camera.main == null) return;

			Color color = _gizmosColor;
			Gizmos.color = _gizmosColor;

			Gizmos.DrawSphere(
				ToWorldPosition(Camera.main), 0.1f);

			Gizmos.color = color;
		}

#endif

	}

}