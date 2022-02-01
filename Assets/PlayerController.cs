using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 3f;
    public float distanceToGround = 0.1f;

    private Rigidbody _rigidbody;

    private PlayerInputActions _playerInputActions;
    private PlayerAnimationController _playerAnimationController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.Player.Jump.performed += Jump;

        _playerAnimationController 
            = GetComponent<PlayerAnimationController>();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGround()) _rigidbody.AddForce(Vector3.up
            * jumpForce, ForceMode.Impulse);
    }

    private bool IsGround()
    {
        return Physics.Raycast(transform.position, 
            Vector3.down, distanceToGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var position = transform.position;
        Gizmos.DrawLine(position, 
            position + (Vector3.down * distanceToGround));
    }

    private void Move()
    {
        float leftRight = _playerInputActions.Player.UpDown.ReadValue<float>();
        float upDown = _playerInputActions.Player.LeftRight.ReadValue<float>();
        
        _rigidbody.AddForce(new Vector3(upDown, 0, leftRight) 
                            * movementSpeed, ForceMode.Force);
        _playerAnimationController.SetAnimationMove(new Vector2(leftRight, upDown));
    }

    private void FixedUpdate()
    {
        Move();
    }
}
