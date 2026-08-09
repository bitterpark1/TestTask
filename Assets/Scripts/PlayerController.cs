using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerInput input;

    [SerializeField]
    Rigidbody playerBody;

    [SerializeField]
    float playerSpeedBase = 6f;

    [SerializeField]
    Animator animator;

    [SerializeField]
    PlayerConfig config;

    const string MOVEMENT_INPUT_NAME = "Movement";

    readonly int moveAnimSpeedParam = Animator.StringToHash("Speed");

	private void Awake()
	{
        animator.SetFloat(moveAnimSpeedParam, config.moveSpeed);
	}

	private void FixedUpdate()
	{
        UpdateMovement();
    }

	void UpdateMovement()
	{
        var joystickInput = input.actions[MOVEMENT_INPUT_NAME].ReadValue<Vector2>();
        var movementDir = new Vector3(joystickInput.x, 0, joystickInput.y).normalized;
        var movement = movementDir * playerSpeedBase * config.moveSpeed * Time.fixedDeltaTime;
        playerBody.MovePosition(transform.position + movement);

        var localMovementDir = transform.InverseTransformDirection(movementDir);
        animator.SetFloat("LocMovementX", localMovementDir.x, .15f, Time.fixedDeltaTime);
        animator.SetFloat("LocMovementZ", localMovementDir.z, .15f, Time.fixedDeltaTime);
        
    }

}
