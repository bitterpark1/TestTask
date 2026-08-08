using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyHealth : MonoBehaviour
	{
		[SerializeField]
		Animator animator;

		[SerializeField]
		EnemyBehaviour behaviour;

		readonly int HurtAnimationParam = Animator.StringToHash("Hurt");

		int startingHp = 10;

		int hp = 10;

		private void OnEnable()
		{
			hp = startingHp;
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
			if (hp > 0)
			{
				animator.SetTrigger(HurtAnimationParam);
				behaviour.takingDamage = true;
			} else
			{
				gameObject.SetActive(false);
			}
		}

		void OnHurtAnimEnded()
		{
			behaviour.takingDamage = false;
		}
	}
}