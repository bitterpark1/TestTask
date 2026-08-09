using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class Spawner : MonoBehaviour
	{
		public static event System.Action<GameObject> EOnEnemySpawned;
		public static event System.Action<GameObject> EOnEnemyDespawned;

		[SerializeField]
		Transform[] spawnPositions;

		[SerializeField]
		EnemyInitializer baseEnemyPrefab;

		[SerializeField]
		EnemyConfig enemyDefs;
		[SerializeField]
		Transform enemiesGroup;
		[SerializeField]
		int maxEnemiesCount = 10;
		[SerializeField]
		int minEnemiesCount = 7;
		[SerializeField]
		Transform playerTransform;
		[SerializeField]
		Collider worldBounds;

		const float CHECK_RADIUS = 0.5f;

		List<GameObject> enemiesAwaitingDespawn = new List<GameObject>();
		List<GameObject> enemiesDespawned = new List<GameObject>();
		int onScreen;

		private void Start()
		{
			transform.position = playerTransform.position;
			VisibilityComp.EOnBecameInvisible += OnEnemyInvisible;
			VisibilityComp.EOnBecameVisible += OnEnemyVisible;
			EnemyHealth.EDeathAnimationFinished += OnEnemyDeathAnimFinished;
			CreateEnemiesPool();
			SpawnEnemies(maxEnemiesCount);
		}

		private void OnDestroy()
		{
			VisibilityComp.EOnBecameInvisible -= OnEnemyInvisible;
			VisibilityComp.EOnBecameVisible -= OnEnemyVisible;
			EnemyHealth.EDeathAnimationFinished -= OnEnemyDeathAnimFinished;
		}

		void CreateEnemiesPool()
		{
			var spawnOfEachType = System.Math.DivRem(maxEnemiesCount, enemyDefs.enemies.Length, out var mod);

			foreach (var enemyType in enemyDefs.enemies)
			{
				for (int i = 0; i < spawnOfEachType + mod; i++)
				{
					InstantiateEnemy(enemyType);
				}
				mod = 0;
			}
		}

		private void InstantiateEnemy(EnemyDefinition randomEnemyType)
		{
			var newEnemy = Instantiate(baseEnemyPrefab, /*new Vector3(pos.position.x, 0, pos.position.z)*/Vector3.zero, Quaternion.identity, enemiesGroup);
			newEnemy.Initialize(randomEnemyType, playerTransform);
			newEnemy.gameObject.SetActive(false);
			enemiesDespawned.Add(newEnemy.gameObject);
		}

		private void OnEnemyDeathAnimFinished(EnemyHealth obj)
		{
			if (!enemiesAwaitingDespawn.Contains(obj.gameObject))
			{
				onScreen--;
			}
			DespawnEnemy(obj.gameObject);
			CheckNewSpawnConditions();
		}

		private void OnEnemyInvisible(GameObject obj)
		{
			enemiesAwaitingDespawn.Add(obj);
			onScreen--;
			CheckNewSpawnConditions();
		}

		private void OnEnemyVisible(GameObject obj)
		{
			if (enemiesAwaitingDespawn.Contains(obj))
			{
				enemiesAwaitingDespawn.Remove(obj);
				onScreen++;
			}
		}

		private void CheckNewSpawnConditions()
		{
			if (onScreen < minEnemiesCount)
			{
				for (int i=0; i<minEnemiesCount - onScreen; i++)
				{
					if (enemiesDespawned.Count > 0)
					{
						StartCoroutine(SpawnEnemy());
					}
					else if (enemiesAwaitingDespawn.Count > 0)
					{

						var randomIndex = Random.Range(0, enemiesAwaitingDespawn.Count);
						var randomEnemy = enemiesAwaitingDespawn[randomIndex];
						DespawnEnemy(randomEnemy);
						StartCoroutine(SpawnEnemy());
					}
				} 	
			}
		}

		IEnumerator SpawnEnemy()
		{
			yield return null;
			SpawnEnemies(1);
		}

		void DespawnEnemy(GameObject obj)
		{
			EOnEnemyDespawned?.Invoke(obj);
			enemiesAwaitingDespawn.Remove(obj);
			enemiesDespawned.Add(obj);
			obj.SetActive(false);
		}
		

		private void Update()
		{
			transform.position = playerTransform.position;
		}

		void SpawnEnemies(int count)
		{
			int positionIndex = Random.Range(0, spawnPositions.Length);

			int positionsTried = 0;
			while (count > 0 && positionsTried < spawnPositions.Length)
			{
				var pos = spawnPositions[positionIndex];
				if (CheckSpawnPosition(pos))
				{
					SpawnEnemy(pos);
					count--;
				}

				positionIndex++;
				if (positionIndex > spawnPositions.Length - 1)
				{
					positionIndex = 0;
				}
				positionsTried++;
			}
		}

		void SpawnEnemy(Transform pos)
		{
			if (enemiesDespawned.Count > 0)
			{
				int randomIndex = Random.Range(0, enemiesDespawned.Count);
				var enemy = enemiesDespawned[randomIndex];

				enemy.transform.position = new Vector3(pos.position.x, 0, pos.position.z);
				enemy.SetActive(true);
				enemiesDespawned.RemoveAt(randomIndex);
				EOnEnemySpawned?.Invoke(enemy);
				onScreen++;
			}
		}


		bool CheckSpawnPosition(Transform spawn)
		{
			var checkPos = new Vector3(spawn.position.x, 0, spawn.position.z);
			bool positionValid =
				!Physics.CheckSphere(new Vector3(spawn.position.x, 0, spawn.position.z), CHECK_RADIUS, LayerMask.GetMask("Walls"))
				&& worldBounds.bounds.Contains(checkPos);
			return positionValid;
		}

	}
}