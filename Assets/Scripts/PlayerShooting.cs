using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerShooting : MonoBehaviour
{

	public static event System.Action<Transform, int> EPlayerFired;

	[SerializeField]
	Animator animator;

	[SerializeField]
	VisualEffect muzzleFlash;

	[SerializeField]
	Rigidbody myBody;

	[SerializeField]
	PlayerConfig config;

	List<EnemyHealth> enemies;

	Transform currentTarget;

	bool shooting;

	readonly int shootParam = Animator.StringToHash("Shooting");
	readonly int attackSpeedParam = Animator.StringToHash("AttackSpeed");

	int damage = 1;

	private void Awake()
	{
		enemies = new List<EnemyHealth>(FindObjectsByType<EnemyHealth>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
		damage = config.damage;
		animator.SetFloat(attackSpeedParam, config.attackSpeed);

		Spawner.EOnEnemySpawned += OnEnemySpawned;
		Spawner.EOnEnemyDespawned += OnEnemyDespawned;
	}

	void OnEnemyDespawned(GameObject enemy)
	{
		enemies.Remove(enemy.GetComponent<EnemyHealth>());
	}

	void OnEnemySpawned(GameObject enemy)
	{
		enemies.Add(enemy.GetComponent<EnemyHealth>());
	}


	private void OnDestroy()
	{
		Spawner.EOnEnemySpawned -= OnEnemySpawned;
		Spawner.EOnEnemyDespawned -= OnEnemyDespawned;
	}

	private void FixedUpdate()
	{

		for (int i=0; i< enemies.Count; i++)
		{
			var enemy = enemies[i];
			if (!enemy.gameObject.activeInHierarchy || !enemy.isAlive)
			{
				continue;
			}

			if (currentTarget == null)
			{
				currentTarget = enemy.transform;
				shooting = true;
			}
			else if((transform.position - enemy.transform.position).sqrMagnitude < (transform.position - currentTarget.position).sqrMagnitude)
			{
				currentTarget = enemy.transform;
				shooting = true;
			}
		}

		if (currentTarget != null)
		{
			var direction = currentTarget.position - transform.position;
			myBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.fixedDeltaTime));
		}
		animator.SetBool(shootParam, shooting);
	}

	void OnMuzzleFlash()
	{
		muzzleFlash.Play();
		EPlayerFired?.Invoke(currentTarget, damage);
	}

	void OnShoot()
	{
		shooting = false;
		currentTarget = null;
	}

}
