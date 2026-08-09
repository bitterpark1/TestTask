using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyHealth : MonoBehaviour
	{
		public static event System.Action<EnemyHealth> EDeathAnimationFinished;

		public bool isAlive { get => hp > 0; }

		[SerializeField]
		EnemyBehaviour behaviour;

		[SerializeField]
		UIBar hpBar;

		[SerializeField]
		Collider myCollider;
		[SerializeField]
		Rigidbody myBody;

		Animator animator;

		readonly int HurtAnimationParam = Animator.StringToHash("Hurt");
		readonly int DeathAnimationParam = Animator.StringToHash("Die");

		int startingHp;
		int hp = 10;

		public void Initialize(int startingHp, Animator animator)
		{
			this.animator = animator;
			this.startingHp = startingHp;
			hp = startingHp;
			hpBar.SetMaxValue(hp);
			hpBar.SetCurrentValue(hp);
		}

		private void OnEnable()
		{
			hp = startingHp;
			hpBar.SetCurrentValue(hp);
			hpBar.gameObject.SetActive(true);
			behaviour.enabled = true;
			myCollider.enabled = true;
			myBody.isKinematic = false;
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
				myBody.isKinematic = true;
				behaviour.takingDamage = true;
			}
			else
			{
				animator.SetTrigger(DeathAnimationParam);
				myCollider.enabled = false;
				behaviour.enabled = false;
				hpBar.gameObject.SetActive(false);
			}
		}

		void OnHurtAnimEnded()
		{
			behaviour.takingDamage = false;
			myBody.isKinematic = false;
		}

		void OnDeathAnimEnded()
		{
			EDeathAnimationFinished?.Invoke(this);
			//Destroy(gameObject);
		}
	}
}