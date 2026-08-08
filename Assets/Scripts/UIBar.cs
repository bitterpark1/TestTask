using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class UIBar : MonoBehaviour
	{
		[SerializeField]
		Transform barFill;

		float maxValue;

		public void SetMaxValue(float value)
		{
			if (value <=0)
			{
				throw new System.Exception($"Max value {value} is not greater than 0!");
			}
			maxValue = value;
		}

		public void SetCurrentValue(float value)
		{
			if (maxValue > 0)
			{
				var factor = value / maxValue;
				barFill.localScale = new Vector3(factor, barFill.localScale.y, barFill.localScale.z);
			}
		}

	}
}