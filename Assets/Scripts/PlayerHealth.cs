using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerHealth : MonoBehaviour
	{
		[SerializeField]
		UIBar hpBar;

		[SerializeField]
		PlayerConfig config;

		int hp = 100;

		private void Awake()
		{
			EnemyBehaviour.EEnemyHitPlayer += OnHit;
			hp = config.health;
			hpBar.SetMaxValue(hp);
			hpBar.SetCurrentValue(hp);
		}
		private void OnDestroy()
		{
			EnemyBehaviour.EEnemyHitPlayer -= OnHit;
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