using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    #region Enums
    private enum Directions { UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3}
    private enum HeldItems { EMPTY = 0, SPEAR = 1, GUN = 2 }
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
    public bool isLeftClicked = false;
    public bool isMoving = false;
    #endregion

    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;
    private HeldItems _heldItem = HeldItems.EMPTY;

    // Walking 
    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_Walk_Right"); // Readonly to not edit later by accident
    private readonly int _animMoveUp = Animator.StringToHash("Anim_Player_Walk_Up");
    private readonly int _animMoveDown = Animator.StringToHash("Anim_Player_Walk_Down");

    // Shooting while moving
    private readonly int _animRunningGunRight = Animator.StringToHash("Anim_Player_RunningGun_Right");
    private readonly int _animRunningGunUp = Animator.StringToHash("Anim_Player_RunningGun_Up");
    private readonly int _animRunningGunDown = Animator.StringToHash("Anim_Player_RunningGun_Down");

    // Walking with spear
    private readonly int _animWalkingSpearRight = Animator.StringToHash("Anim_Player_WalkSpear_Right");
    private readonly int _animWalkingSpearUp = Animator.StringToHash("Anim_Player_WalkSpear_Up");
    private readonly int _animWalkingSpearDown = Animator.StringToHash("Anim_Player_WalkSpear_Down");

    // Idling normal
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Player_Idle_Right");
    private readonly int _animIdleUp = Animator.StringToHash("Anim_Player_Idle_Up");
    private readonly int _animIdleDown = Animator.StringToHash("Anim_Player_Idle_Down");

    // Idling gun
    private readonly int _animIdleGunRight = Animator.StringToHash("Anim_Player_IdleGun_Right");
    private readonly int _animIdleGunUp = Animator.StringToHash("Anim_Player_IdleGun_Up");
    private readonly int _animIdleGunDown = Animator.StringToHash("Anim_Player_IdleGun_Down");

    // Idle with spear
    private readonly int _animIdleSpearRight = Animator.StringToHash("Anim_Player_IdleSpear_Right");
    private readonly int _animIdleSpearUp = Animator.StringToHash("Anim_Player_IdleSpear_Up");
    private readonly int _animIdleSpearDown = Animator.StringToHash("Anim_Player_IdleSpear_Down");

    // Shooting static
    private readonly int _animStaticGunRight = Animator.StringToHash("Anim_Player_StaticGun_Right");
    private readonly int _animStaticGunUp = Animator.StringToHash("Anim_Player_StaticGun_Up");
    private readonly int _animStaticGunDown = Animator.StringToHash("Anim_Player_StaticGun_Down");

    // Attacking with spear
    private readonly int _animSpearRight = Animator.StringToHash("Anim_Player_Spear_Right");
    private readonly int _animSpearUp = Animator.StringToHash("Anim_Player_Spear_Up");
    private readonly int _animSpearDown = Animator.StringToHash("Anim_Player_Spear_Down");
    #endregion

    #region Tick
    private void Update()
    {
        GatherInput();   
        CalculateFacingDirection();
        UpdateAnimation();
        SetAnimatorDirectionParameters();
        SaveLastUsedItem();
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

        isLeftClicked = Input.GetMouseButtonDown(0);
        isRightClickHeld = Input.GetMouseButton(1);
    }
    private void SaveLastUsedItem()
    {
        if (isLeftClicked)
        {
            _heldItem = HeldItems.SPEAR;
            _animator.SetInteger("HeldItem", (int)_heldItem);
        }
        if (isRightClickHeld)
        {
            _heldItem = HeldItems.GUN;
            _animator.SetInteger("HeldItem", (int)_heldItem);
        }
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
            if(!isRightClickHeld && _heldItem == HeldItems.EMPTY)
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
            if (!isRightClickHeld && _heldItem == HeldItems.SPEAR)
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animWalkingSpearRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animWalkingSpearUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animWalkingSpearDown, 0);
                }
            }
            if(isRightClickHeld)
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animRunningGunRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animRunningGunUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animRunningGunDown, 0);
                }
            }

        }
        else // We're Static
        {
            if (!isRightClickHeld && _heldItem == HeldItems.EMPTY) 
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
            else if (!isRightClickHeld && _heldItem == HeldItems.GUN)
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animIdleGunRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animIdleGunUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animIdleGunDown, 0);
                }
            }
            else if (!isRightClickHeld && _heldItem == HeldItems.SPEAR)
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animIdleSpearRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animIdleSpearUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animIdleSpearDown, 0);
                }
            }
            else if (isRightClickHeld) // Gun Shooting Animation
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animStaticGunRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animStaticGunUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animStaticGunDown, 0);
                }
            }
            else if (isLeftClicked)
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animSpearRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animSpearUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animSpearDown, 0);
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
