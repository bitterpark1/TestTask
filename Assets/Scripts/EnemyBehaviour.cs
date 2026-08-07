using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField]
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
    float moveSpeed;

    [SerializeField]
    Transform showPoint;

    readonly int AttackAnimParam = Animator.StringToHash("Attacking");
    readonly int HurtAnimParam = Animator.StringToHash("Hurt");

    bool attacking = false;

    Vector3[] currentPath = new Vector3[32];
    RaycastHit[] raycastHits = new RaycastHit[1];
    float repathTimerCurrent;
    float repathTime = .5f;

    int pathNextWaypointIndex;

    private void FixedUpdate()
	{
        //Find direction to player
        var directionToPlayer = playerPos.position - transform.position;
        var distanceToPlayer = directionToPlayer.magnitude;

        //Raycast to see if player is obstructed
        Vector3 targetPos = playerPos.position;
        bool playerBehindObstacles = Physics.RaycastNonAlloc(new Ray(transform.position + new Vector3(0, 0.3f), directionToPlayer), raycastHits, distanceToPlayer, LayerMask.GetMask("Walls")) > 0;

        if (playerBehindObstacles)
		{
            //if no path exists or timer has elapsed - get navmesh path
            if (currentPath == null || currentPath.Length == 0 || repathTimerCurrent < 0)
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

            if (currentPath != null && currentPath.Length > pathNextWaypointIndex)
			{
                targetPos = currentPath[pathNextWaypointIndex];
                if ((targetPos - transform.position).sqrMagnitude < 0.05f)
				{
                    pathNextWaypointIndex++;
				}
            }
		}    

        //Find direction to player
        var directionToCurrentGoal = targetPos - transform.position;
        //Lerp rotate towards player
        myBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToCurrentGoal), 5f * Time.fixedDeltaTime));


        if (attacking && distanceToPlayer > maxAttackDistance)
		{
            attacking = false;
		}

        if (!attacking)
		{
            if (distanceToPlayer > minAttackDistance)
			{
                myBody.MovePosition(transform.position + directionToCurrentGoal.normalized * moveSpeed * Time.fixedDeltaTime);
            } else
			{
                attacking = true;
			}
            
        }
        animator.SetBool(AttackAnimParam, attacking);

	}

}
