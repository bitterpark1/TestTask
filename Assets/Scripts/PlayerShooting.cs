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

	EnemyBehaviour[] enemies;

	Transform currentTarget;

	bool shooting;

	readonly int shootParam = Animator.StringToHash("Shooting");

	int damage = 5;

	private void Awake()
	{
		enemies = Resources.FindObjectsOfTypeAll<EnemyBehaviour>();
		//set shoot animation speed factor
	}

	private void FixedUpdate()
	{
		if (currentTarget == null)
		{
			//Go through enemies and find nearest
			for (int i=0; i< enemies.Length; i++)
			{
				var enemy = enemies[i];
				if (!enemy.gameObject.activeInHierarchy)
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

			
		} else
		{
			var direction = currentTarget.position - transform.position;
			myBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.fixedDeltaTime));
		}
		animator.SetBool(shootParam, shooting);
	}

	void OnShoot()
	{
		shooting = false;
		muzzleFlash.Play();
		//Invoke event to damage enemy
		EPlayerFired?.Invoke(currentTarget, damage);
		currentTarget = null;
	}

}
