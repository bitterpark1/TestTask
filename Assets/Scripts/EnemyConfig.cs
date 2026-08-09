using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "EnemyType", menuName = "SO Configs/New Enemy Config")]
	public class EnemyConfig : ScriptableObject
	{
		public EnemyDefinition[] enemies;
	}
}