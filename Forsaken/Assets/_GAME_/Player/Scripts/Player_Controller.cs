using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    #region Enums
    private enum Directions { UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3}
    #endregion
    #region Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion

    #region Animator Data
    public bool isRightClickHeld = false;
    public bool isLeftClickHeld = false;
    #endregion

    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;

    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_Walk_Right"); // Readonly to not edit later by accident
    private readonly int _animMoveUp = Animator.StringToHash("Anim_Player_Walk_Up");
    private readonly int _animMoveDown = Animator.StringToHash("Anim_Player_Walk_Down");
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Player_Idle_Right");
    private readonly int _animIdleUp = Animator.StringToHash("Anim_Player_Idle_Up");
    private readonly int _animIdleDown = Animator.StringToHash("Anim_Player_Idle_Down");
    #endregion

    #region Tick
    private void Update()
    {
        GatherInput();   
        CalculateFacingDirection();
        UpdateAnimation();
        SetAnimatorDirectionParameters();
    }


    private void FixedUpdate()
    {
        MovementUpdate();
    }
    #endregion

    #region Input Logic
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");

        isLeftClickHeld = Input.GetMouseButton(0);
        isRightClickHeld = Input.GetMouseButton(1);
    }
    #endregion

    #region Movement Logic
    private void MovementUpdate()
    {
        _rb.velocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;
    }
    #endregion

    #region Animation Logic
    private void CalculateFacingDirection()
    {
        if (_moveDir.x != 0)
        {
            _facingDirection = _moveDir.x > 0 ? Directions.RIGHT : Directions.LEFT;
        }
        else if (_moveDir.y != 0)
        {
            _facingDirection = _moveDir.y > 0 ? Directions.UP :  Directions.DOWN;

        }
        // Debug.Log(_facingDirection);
    }
    private void UpdateAnimation()
    {
        if (_facingDirection == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }

        if (_moveDir.SqrMagnitude() > 0) // We're moving
        {
            if(_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
            {
                _animator.CrossFade(_animMoveRight, 0);
            }
            else if (_facingDirection == Directions.UP)
            {
                _animator.CrossFade(_animMoveUp, 0);
            }
            else if (_facingDirection == Directions.DOWN)
            {
                _animator.CrossFade(_animMoveDown, 0);
            }
        }
        else
        {
            if (!isRightClickHeld) 
            { 
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animIdleRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animIdleUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animIdleDown, 0);
                }
            }
        }
    }
    private void SetAnimatorDirectionParameters()
    {
        _animator.SetInteger("FacingDirection", (int) _facingDirection);
    }
    #endregion
}
