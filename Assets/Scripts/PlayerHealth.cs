using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerHealth : MonoBehaviour
	{
		[SerializeField]
		UIBar hpBar;
		
		int hp = 100;

		private void Awake()
		{
			EnemyBehaviour.EEnemyHitPlayer += OnHit;
			hpBar.SetMaxValue(hp);
			hpBar.SetCurrentValue(hp);
		}

		void OnHit(int damage)
		{
			hp -= damage;
			hpBar.SetCurrentValue(hp);
			if (hp <= 0)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}

	}
}