using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public float animationSmoothTime = 5f;

    private Vector2 _currentMovementAnimationBlend = new Vector2();
    private Vector2 _currentBlendVelocity;

    private Animator _animator;
    
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Z = Animator.StringToHash("Z");

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetAnimationMove(Vector2 inputVector)
    {
        _currentMovementAnimationBlend = Vector2.SmoothDamp(
                _currentMovementAnimationBlend, inputVector,
                ref _currentBlendVelocity, animationSmoothTime * Time.deltaTime);
        _animator.SetFloat(X, _currentMovementAnimationBlend.x);
        _animator.SetFloat(Z, _currentMovementAnimationBlend.y);
    }
}
