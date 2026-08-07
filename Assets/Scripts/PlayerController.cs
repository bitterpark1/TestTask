using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerInput input;

    [SerializeField]
    Rigidbody playerBody;

    [SerializeField]
    float playerSpeed = 1f;

    [SerializeField]
    Animator animator;

    const string MOVEMENT_INPUT_NAME = "Movement";

	private void FixedUpdate()
	{
        UpdateMovement();
    }

	void UpdateMovement()
	{
        var joystickInput = input.actions[MOVEMENT_INPUT_NAME].ReadValue<Vector2>();
        var movementDir = new Vector3(joystickInput.x, 0, joystickInput.y).normalized;
        var movement = movementDir * playerSpeed * Time.fixedDeltaTime;
        playerBody.MovePosition(transform.position + movement);

        //Convert movement input to local axes and update animator
        var localMovementDir = transform.InverseTransformDirection(movementDir);
        animator.SetFloat("LocMovementX", localMovementDir.x, .15f, Time.fixedDeltaTime);
        animator.SetFloat("LocMovementZ", localMovementDir.z, .15f, Time.fixedDeltaTime);
        
    }

}
