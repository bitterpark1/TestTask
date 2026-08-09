using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "EnemyType", menuName = "SO Configs/New Enemy Type")]
	public class EnemyDefinition : ScriptableObject
	{
		public int hp;
		public int damage;
		public float moveSpeed;
		public GameObject model;
	}
}