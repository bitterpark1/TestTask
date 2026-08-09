using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "PlayerConfig", menuName = "SO Configs/New Player Config")]
	public class PlayerConfig : ScriptableObject
	{
		public float attackSpeed = 1;
		public int damage = 10;
		public int health = 100;
		public float moveSpeed = 1;
	}
}