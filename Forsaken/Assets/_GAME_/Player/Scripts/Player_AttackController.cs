using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackController : MonoBehaviour
{
    #region Enums
    private enum Directions { UP, DOWN, LEFT, RIGHT }
    private enum HeldItems { EMPTY = 0, SPEAR = 1, GUN = 2}
    #endregion

    #region Editor Data
    [Header("Dependencies")]
    [SerializeField] Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    #endregion

    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;
    private HeldItems _heldItem = HeldItems.EMPTY;

    private readonly int _animStandingShootingRight = Animator.StringToHash("Anim_Player_StaticGun_Right");
    private readonly int _animStandingShootingUp = Animator.StringToHash("Anim_Player_StaticGun_Up");
    private readonly int _animStandingShootingDown = Animator.StringToHash("Anim_Player_StaticGun_Down");
    #endregion

    #region Animator Data
    public bool isRightClickHeld = false;
    public bool isLeftClickHeld = false;
    #endregion

    #region Tick
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        CalculateFacingDirection();
        GatherInput();
        SaveLastUsedItem();
        UpdateAnimation();
    }
    #endregion

    #region Input Logic
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");

        isLeftClickHeld = Input.GetMouseButton(0);
        isRightClickHeld = Input.GetMouseButton(1);

        _animator.SetBool("isLeftClickHeld", isLeftClickHeld);
        _animator.SetBool("isRightClickHeld", isRightClickHeld);
    }

    private void SaveLastUsedItem()
    {
        if(isLeftClickHeld)
        {
            _heldItem = HeldItems.SPEAR;
            _animator.SetInteger("HeldItem", (int)_heldItem);
        }
        if(isRightClickHeld)
        {
            _heldItem = HeldItems.GUN;
            _animator.SetInteger("HeldItem", (int)_heldItem);
        }
    }
    #endregion


    #region AnimationLogic
    private void CalculateFacingDirection()
    {
        if (_moveDir.x != 0)
        {
            _facingDirection = _moveDir.x > 0 ? Directions.RIGHT : Directions.LEFT;
        }
        else if (_moveDir.y != 0)
        {
            _facingDirection = _moveDir.y > 0 ? Directions.UP : Directions.DOWN;

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

    }
    #endregion
}
