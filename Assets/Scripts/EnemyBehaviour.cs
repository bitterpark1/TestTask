using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    public static event System.Action<int> EEnemyHitPlayer;

    [System.NonSerialized]
    public bool takingDamage;

    //[SerializeField]
    Animator animator;
    [SerializeField]
    Rigidbody myBody;

    [SerializeField]
    Transform playerPos;

    [SerializeField]
    float maxAttackDistance = 1f;
    [SerializeField]
    float minAttackDistance = .75f;

    [SerializeField]
    float baseSpeed = 3;

    readonly int AttackAnimParam = Animator.StringToHash("Attacking");
    readonly int MoveSpeedAnimParam = Animator.StringToHash("Speed");

    bool attacking = false;

    int attackDamage = 10;

    Vector3[] currentPath = new Vector3[32];
    RaycastHit[] raycastHits = new RaycastHit[1];
    float repathTimerCurrent;
    float repathTime = 1f;

    int pathNextWaypointIndex;


    public void Initialize(int damage, float speed, Animator animator, Transform playerTransform)
    {
        attackDamage = damage;
        baseSpeed = baseSpeed * speed;
        
        this.animator = animator;
        animator.SetFloat(MoveSpeedAnimParam, speed);
        playerPos = playerTransform;
    }


	private void OnDisable()
	{
        takingDamage = false;
        attacking = false;
        pathNextWaypointIndex = 0;
        repathTimerCurrent = 0;
	}

	private void FixedUpdate()
	{   
        if (takingDamage)
		{
            return;
		}
        
        var directionToPlayer = playerPos.position - transform.position;
        var distanceToPlayer = directionToPlayer.magnitude;

        Vector3 targetPos = playerPos.position;
        bool playerBehindObstacles = Physics.RaycastNonAlloc(new Ray(transform.position + new Vector3(0, 0.3f), directionToPlayer), raycastHits, distanceToPlayer, LayerMask.GetMask("Walls")) > 0;

        if (playerBehindObstacles)
		{
            if (repathTimerCurrent < 0)
			{
                NavMeshPath newPath = new NavMeshPath();
                if (NavMesh.CalculatePath(transform.position, playerPos.position, NavMesh.AllAreas, newPath))
                {
                    newPath.GetCornersNonAlloc(currentPath);
                    repathTimerCurrent = repathTime;
                    pathNextWaypointIndex = 0;
                }
            } else
			{
                repathTimerCurrent -= Time.fixedDeltaTime;
			}

            if (currentPath.Length > pathNextWaypointIndex)
			{
                targetPos = currentPath[pathNextWaypointIndex];
                if ((targetPos - transform.position).sqrMagnitude < 0.05f)
				{
                    pathNextWaypointIndex++;
				}
            }
		}    

        var directionToCurrentGoal = targetPos - transform.position;
        myBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToCurrentGoal), 5f * Time.fixedDeltaTime));


        if (attacking && distanceToPlayer > maxAttackDistance)
		{
            attacking = false;
		}

        if (!attacking)
		{
            if (distanceToPlayer > minAttackDistance)
			{
                myBody.MovePosition(transform.position + directionToCurrentGoal.normalized * baseSpeed * Time.fixedDeltaTime);
            } else
			{
                attacking = true;
			}
            
        }
        animator.SetBool(AttackAnimParam, attacking);

	}

    void OnAttackAnimationHitPlayer()
	{
        EEnemyHitPlayer?.Invoke(attackDamage);
    }
}
