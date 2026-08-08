using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class BackButtonScript : MonoBehaviour
	{
		void Awake()
		{
			Input.backButtonLeavesApp = true;
		}
	}
}