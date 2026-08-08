using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyHealth : MonoBehaviour
	{
		public static event System.Action<EnemyHealth> EEnemyDead;

		[SerializeField]
		Animator animator;

		[SerializeField]
		EnemyBehaviour behaviour;

		[SerializeField]
		UIBar hpBar;

		[SerializeField]
		Collider myCollider;

		readonly int HurtAnimationParam = Animator.StringToHash("Hurt");
		readonly int DeathAnimationParam = Animator.StringToHash("Die");

		int startingHp = 10;

		int hp = 10;

		private void OnEnable()
		{
			hp = startingHp;
			hpBar.SetMaxValue(hp);
			hpBar.SetCurrentValue(hp);
			//Subscribe to player shoot event
			PlayerShooting.EPlayerFired += OnPlayerFired;
		}

		private void OnDisable()
		{
			PlayerShooting.EPlayerFired -= OnPlayerFired;
		}

		private void OnPlayerFired(Transform obj, int damage)
		{
			if (transform == obj)
			{
				OnShot(damage);
			}
		}

		void OnShot(int damageTaken)
		{
			hp -= damageTaken;
			hpBar.SetCurrentValue(hp);
			if (hp > 0)
			{
				animator.SetTrigger(HurtAnimationParam);
				behaviour.takingDamage = true;
			}
			else
			{
				animator.SetTrigger(DeathAnimationParam);
				myCollider.enabled = false;
				EEnemyDead?.Invoke(this);
				behaviour.enabled = false;
				hpBar.gameObject.SetActive(false);
			}
		}

		void OnHurtAnimEnded()
		{
			behaviour.takingDamage = false;
		}

		void OnDeathAnimEnded()
		{
			Destroy(gameObject);
		}
	}
}