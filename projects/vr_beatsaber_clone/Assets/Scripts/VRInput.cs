#define PC

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRInput
{
	private static Platform platform;
	private static Platform PlatformInstance
	{
		get
		{
			if( null == platform )
			{
#if PC
				platform = new PCPlatform();
#elif Oculus
				platform = new OculusPlatform();
#endif
			}

			return platform;
		}
	}

	public static Transform LHand
	{
		get
		{
			return PlatformInstance?.LHand;
		}
	}
	public static Transform RHand
	{
		get
		{
			return PlatformInstance?.RHand;
		}
	}
	public static Vector3	RHandPosition	{ get => platform?.RHandPosition ?? Vector3.zero; }
	public static Vector3	LHandPosition	{ get => platform?.LHandPosition ?? Vector3.zero; }
	public static Vector3	RHandDirection	{ get => platform?.RHandDirection ?? Vector3.zero; }
	public static Vector3	LHandDirection	{ get => platform?.LHandDirection ?? Vector3.zero; }

	public static bool GetButton( Button button, Controller controller )
	{
		return platform?.GetButton( button, controller ) ?? false;
	}
	public static bool GetButtonDown( Button button, Controller controller )
	{
		return platform?.GetButtonDown( button, controller ) ?? false;
	}
	public static bool GetButtonUp( Button button, Controller controller )
	{
		return platform?.GetButtonUp( button, controller ) ?? false;
	}
	public static float GetAxis( string axis, Controller controller )
	{
		return platform?.GetAxis( axis, controller ) ?? 0f;
	}

	public enum Button
	{
		One,
		Two,
		Thumbstick,
		IndexTrigger,
		HandTrigger,
	}
	public enum Controller
	{
		LTouch,
		RTouch,
	}

	private abstract class Platform
	{
		protected Transform				lHand;
		protected Transform				rHand;

		public abstract Transform		LHand			{ get; }
		public abstract Transform		RHand			{ get; }
		public abstract Vector3			RHandPosition	{ get; }
		public abstract Vector3			LHandPosition	{ get; }
		public abstract Vector3			RHandDirection	{ get; }
		public abstract Vector3			LHandDirection	{ get; }

		public abstract bool GetButton( Button button, Controller controller );
		public abstract bool GetButtonDown( Button button, Controller controller );
		public abstract bool GetButtonUp( Button button, Controller controller );
		public abstract float GetAxis( string axis, Controller controller );

		public abstract void PlayVibration( Controller hand );
		public abstract void PlayVibration( Controller hand, float duration, float frequency, float amplitude );

		public void Recenter( Transform target, Vector3 direction )
		{
			target.forward = target.rotation * direction;
		}
		public abstract void Recenter();
	}
	private class PCPlatform : Platform
	{
		public override Transform		RHand
		{
			get
			{
				if ( null == rHand )
				{
					GameObject handObj	= new GameObject("RHand");
					rHand				= handObj.transform;
					rHand.parent		= Camera.main.transform;
				}

				return rHand;
			}
		}
		public override Transform		LHand
		{
			get
			{
				return RHand;
			}
		}

		public override Vector3			RHandPosition
		{
			get
			{
				Vector3 pos		= Input.mousePosition;
				pos.z			= 0.7f;
				pos				= Camera.main.ScreenToWorldPoint( pos );
				RHand.position	= pos;

				return pos;
			}
		}
		public override Vector3			LHandPosition
		{
			get
			{
				return RHandPosition;
			}
		}
		public override Vector3			RHandDirection
		{
			get
			{
				Vector3 direction	= RHandPosition - Camera.main.transform.position;
				RHand.forward		= direction;

				return direction;
			}
		}
		public override Vector3			LHandDirection
		{
			get
			{
				return RHandDirection;
			}
		}

		public override bool GetButton( Button button, Controller controller )
		{
			return Input.GetButton( ( (ButtonTarget)button ).ToString() );
		}
		public override bool GetButtonDown( Button button, Controller controller )
		{
			return Input.GetButtonDown( ( (ButtonTarget)button ).ToString() );
		}
		public override bool GetButtonUp( Button button, Controller controller )
		{
			return Input.GetButtonUp( ( (ButtonTarget)button ).ToString() );
		}
		public override float GetAxis( string axis, Controller controller )
		{
			return Input.GetAxis( axis );
		}

		public override void PlayVibration( Controller hand )
		{
			Debug.Log( hand + " vibration" );
		}
		public override void PlayVibration( Controller hand, float duration, float frequency, float amplitude )
		{
			Debug.Log( hand + " vibration / duration : " + duration + " frequency : " + frequency + " amplitude : " + amplitude );
		}

		public override void Recenter()
		{
			throw new NotImplementedException();
		}

		public enum ButtonTarget
		{
			Fire1,
			Fire2,
			Fire3,
			Jump,
		}
	}
	private class OculusPlatform : Platform
	{
		public override Transform LHand => throw new NotImplementedException();

		public override Transform RHand => throw new NotImplementedException();

		public override Vector3 RHandPosition => throw new NotImplementedException();

		public override Vector3 LHandPosition => throw new NotImplementedException();

		public override Vector3 RHandDirection => throw new NotImplementedException();

		public override Vector3 LHandDirection => throw new NotImplementedException();

		public override float GetAxis( string axis, Controller controller )
		{
			throw new NotImplementedException();
		}

		public override bool GetButton( Button virtualMask, Controller controller )
		{
			throw new NotImplementedException();
		}

		public override bool GetButtonDown( Button virtualMask, Controller controller )
		{
			throw new NotImplementedException();
		}

		public override bool GetButtonUp( Button virtualMask, Controller controller )
		{
			throw new NotImplementedException();
		}

		public override void PlayVibration( Controller hand )
		{
			throw new NotImplementedException();
		}

		public override void PlayVibration( Controller hand, float duration, float frequency, float amplitude )
		{
			throw new NotImplementedException();
		}

		public override void Recenter()
		{
			throw new NotImplementedException();
		}
	}
}