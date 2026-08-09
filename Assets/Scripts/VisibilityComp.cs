using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class VisibilityComp : MonoBehaviour
	{
		public static event System.Action<GameObject> EOnBecameInvisible;
		public static event System.Action<GameObject> EOnBecameVisible;

		bool initialized = false;
		GameObject mainObj;
		public void Initialize(GameObject parentObj)
		{
			this.mainObj = parentObj;
			initialized = true;
		}

		private void OnBecameVisible()
		{
			if (initialized)
			{
				EOnBecameVisible?.Invoke(mainObj);
			}
		}

		private void OnBecameInvisible()
		{
			if (initialized)
			{
				EOnBecameInvisible?.Invoke(mainObj);
			}	
		}
	}
}