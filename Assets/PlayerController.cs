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

    public GameObject cameraAnchor;
    private float _yaw;
    private float _pitch;
    public float rotationSmoothing = 20f;

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
       if (IsGround()) 
            _rigidbody.AddForce(Vector3.up
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
        float leftRight = _playerInputActions.Player.LeftRight.ReadValue<float>();
        float upDown = _playerInputActions.Player.UpDown.ReadValue<float>();
        
        _rigidbody.AddForce(new Vector3(upDown, 0, leftRight) 
                            * movementSpeed, ForceMode.Force);
        _playerAnimationController.SetAnimationMove(new Vector2(leftRight, upDown));
    }

    private void FixedUpdate()
    {
        Move();
        SetRotation();
    }

    private void SetRotation()
    {
        Vector2 inputVector = _playerInputActions.Player.Aim.ReadValue<Vector2>();

        _yaw += inputVector.x;
        _pitch -= inputVector.y;

        _pitch = Mathf.Clamp(_pitch, -60, 90);

        Quaternion smoothRotation = Quaternion.Euler(_pitch, _yaw, 0);

        cameraAnchor.transform.rotation = Quaternion.Slerp(cameraAnchor.transform.rotation, 
            smoothRotation, rotationSmoothing * Time.fixedDeltaTime);

        smoothRotation = Quaternion.Euler(0, _yaw, 0);
        
        transform.rotation = Quaternion.Slerp(transform.rotation,
            smoothRotation, rotationSmoothing * Time.fixedDeltaTime);
    }
}
