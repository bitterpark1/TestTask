using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyInitializer : MonoBehaviour
	{

		[SerializeField]
		EnemyBehaviour behaviour;
		[SerializeField]
		EnemyHealth health;
		//[SerializeField]
		//EnemyDefinition enemy;

		[SerializeField]
		RuntimeAnimatorController contr;

		public void Initialize(EnemyDefinition enemy, Transform playerTransform)
		{

			//Создаем модель
			var visual = Instantiate(enemy.model, transform);
			//Считываем аватар и заменяем аниматор дочернего объекта, автоматически добавленный юнити, на родительский для удобства
			var generatedAnimator = visual.GetComponent<Animator>();
			var avatar = generatedAnimator.avatar;
			if (avatar == null)
			{
				throw new System.Exception($"Для модели врага {enemy.model.name} в настройках импорта рига нужно сгенерировать аватар! Иначе анимации не будут работать");
			}
			Destroy(generatedAnimator);

			var animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = contr;
			animator.avatar = avatar;

			var visComp = GetComponentInChildren<SkinnedMeshRenderer>().gameObject.AddComponent<VisibilityComp>();
			visComp.Initialize(gameObject);

			behaviour.Initialize(enemy.damage, enemy.moveSpeed, animator, playerTransform);
			health.Initialize(enemy.hp, animator);


		}

	}
}