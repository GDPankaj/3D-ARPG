using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    CharacterController _charCon;
    [SerializeField] float moveSpeed = 5f;
    Vector3 _movementVelocity;
    PlayerInput _playerInput;
    float _verticalVelocity;
    [SerializeField] float _gravityScale = -9.8f;
    Animator _charAnimator;
    Transform _playerTransform;
    bool _isInvincible;
    float _invincibilityDuration = 2f;
    [SerializeField] int _coins;
    [SerializeField] float _slideSpeed = 9f;


    float _attackAnimationDuration;

    //enemy
    [SerializeField] bool _isPlayer = true;
    NavMeshAgent _enemyNavMeshAgent;

    float _attackStartTime;
    [SerializeField] float _attackSlideDuration = .4f;
    [SerializeField] float _attackSlideSpeed = .06f;
    [SerializeField] GameObject _itemToDrop;

    Health _health;
    DamageCaster _damageCaster;
    Vector3 _impactOnCharacter;

    [SerializeField]float _spawnDuration = 2f;
    float _currentSpawnTime;

    //Material
    MaterialPropertyBlock _materialPropertyBlock;
    SkinnedMeshRenderer _skinnedMeshRenderer;

    public enum CharacterState
    {
        Normal, Attacking, Dead, BeingHit, Slide, Spawn
    }

    CharacterState _currentState;

    private void Awake()
    {
        _charCon = GetComponent<CharacterController>();
        _charAnimator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>();
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

        if(!_isPlayer)
        {
            _enemyNavMeshAgent = GetComponent<NavMeshAgent>();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _enemyNavMeshAgent.speed = moveSpeed;
            SwitchStateTo(CharacterState.Spawn);
        }
        else
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }

    public void CalculatePlayerMovement()
    {
        if(_playerInput.GetMouseButtonDown() && _charCon.isGrounded)
        {
            SwitchStateTo(CharacterState.Attacking);
            return;
        }
        else if(_playerInput.GetSpaceKeyDown() && _charCon.isGrounded)
        {
            SwitchStateTo(CharacterState.Slide);
            return;
        }
        _movementVelocity = new Vector3(_playerInput.GetHorizontalInput(), 0f, _playerInput.GetVerticalInput());
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0f, -45f, 0f) * _movementVelocity;
        _charAnimator.SetFloat("Speed", _movementVelocity.magnitude);
        _movementVelocity *= moveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }

        _charAnimator.SetBool("AirBorne", !_charCon.isGrounded);
    }

    public void CalculateEnemyMovement()
    {
        if(Vector3.Distance(_playerTransform.position, transform.position) > _enemyNavMeshAgent.stoppingDistance)
        {
            _enemyNavMeshAgent.SetDestination(_playerTransform.position);
            _charAnimator.SetFloat("Speed", 0.2f);
        }
        else
        {
            _enemyNavMeshAgent.SetDestination(transform.position);
            _charAnimator.SetFloat("Speed", 0f);
            SwitchStateTo(CharacterState.Attacking);
        }
    }

    private void FixedUpdate()
    {

        switch (_currentState)
        {
            case CharacterState.Normal:
                if (_isPlayer)
                    {
                        CalculatePlayerMovement();
                    }
                else
                    {
                        CalculateEnemyMovement();
                    }
                break;

            case CharacterState.Attacking:
                if (_isPlayer)
                {
                    _movementVelocity = Vector3.zero;

                    if(Time.time > _attackStartTime - _attackSlideDuration)
                    {
                        float timePassed = Time.time - _attackStartTime;
                        float lerpTime = timePassed / _attackSlideDuration;

                        _movementVelocity = Vector3.Lerp(transform.forward * _attackSlideSpeed, Vector3.zero, lerpTime);
                    }
                    if (_playerInput.GetMouseButtonDown() && _charCon.isGrounded)
                    {
                        string currentAnimationClipName = _charAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        _attackAnimationDuration = _charAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                        if(currentAnimationClipName != "LittleAdventurerAndie_ATTACK_03" && _attackAnimationDuration>0.5f && _attackAnimationDuration < 0.7f)
                        {
                            _playerInput.SetMouseButtonDown(false);
                            SwitchStateTo(CharacterState.Attacking);

                            CalculatePlayerMovement();
                        }
                    }
                }
                break;
            case CharacterState.Dead:
                
                return;
            case CharacterState.BeingHit:
                if (_impactOnCharacter.magnitude > .2f)
                {
                    _movementVelocity = _impactOnCharacter * Time.deltaTime;
                }
                _impactOnCharacter = Vector3.Lerp(_impactOnCharacter, Vector3.zero, Time.deltaTime * 5);
                break;
            case CharacterState.Slide:
                _movementVelocity = transform.forward * _slideSpeed * Time.deltaTime;
                break;
            case CharacterState.Spawn:
                _currentSpawnTime -= Time.deltaTime;
                if (_currentSpawnTime <= 0)
                {
                    SwitchStateTo(CharacterState.Normal);
                }
                break;
        }

        if (_impactOnCharacter.magnitude > .2f)
        {
            _movementVelocity = _impactOnCharacter * Time.deltaTime;
        }
        _impactOnCharacter = Vector3.Lerp(_impactOnCharacter, Vector3.zero, Time.deltaTime * 5);

        if (_isPlayer)
        {
            if (!_charCon.isGrounded)
            {
                _verticalVelocity = _gravityScale;
            }
            else
            {
                _verticalVelocity = _gravityScale * .3f;
            }

            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
        
            _charCon.Move(_movementVelocity);
            _movementVelocity = Vector3.zero;
        }
        else
        {
            if(_currentState != CharacterState.Normal)
            {
                _charCon.Move(_movementVelocity);
                _movementVelocity = Vector3.zero;
            }
        }
            
        
    }

    public void SwitchStateTo(CharacterState state)
    {
        if (_isPlayer)
        {
            _playerInput.ClearInputCache();
        }
       
        //exiting current state

        switch (_currentState)
        { 
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                DisableDamageCaster();

                if (_isPlayer)
                {
                    GetComponent<GameVFXManager>().StopBlade();
                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                break ;
            case CharacterState.Slide:
                break;
            case CharacterState.Spawn:
                _isInvincible = false;
                break;
        }


        //entering new state called state
        switch (state)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if(!_isPlayer)
                {
                    transform.rotation = Quaternion.LookRotation(_playerTransform.position - transform.position);
                }

                _charAnimator.SetTrigger("Attack");
                if(_isPlayer)
                {
                    _attackStartTime = Time.time;
                }
                
                break;
            case CharacterState.Dead:
                _charCon.enabled = false;
                _charAnimator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.BeingHit:
                _charAnimator.SetTrigger("BeingHit");
                if (_isPlayer)
                {
                    _isInvincible = true;
                    StartCoroutine(CancelInvincibility());
                }
                break;
            case CharacterState.Slide:
                _charAnimator.SetTrigger("Slide");
                break;
            case CharacterState.Spawn:
                _isInvincible = true;
                _currentSpawnTime = _spawnDuration;
                StartCoroutine(MaterialAppear());
                break;
        }

        _currentState = state;

        Debug.Log("Switched to " + _currentState);
    }

    public void SlideAnimationEnd()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void AttackAnimationEnd()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void BeingHitAnimationEnd()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if(_isInvincible)
        {
            return;
        }
        _health?.ApplyDamage(damage);

        if (!_isPlayer)
        {
            GetComponent<EnemyVFXManager>().BeingHit(attackerPos);
        }

        StartCoroutine(MaterialBlink());
        if (_isPlayer)
        {
            SwitchStateTo(CharacterState.BeingHit);
            AddImpact(attackerPos, 10f);
        }
        else
        {
            AddImpact(attackerPos, 2.5f);
        }
    }

    IEnumerator CancelInvincibility()
    {
        yield return new WaitForSeconds(_invincibilityDuration);
        _isInvincible = false;
    }

    private void AddImpact(Vector3 attackerPos, float force)
    {
        Vector3 impactDir = transform.position - attackerPos;
        impactDir.Normalize();
        impactDir.y = 0f;
        _impactOnCharacter = impactDir * force;
    }

    public void EnableDamageCaster()
    {
        _damageCaster?.EnableDamageCaster();
    }
    public void DisableDamageCaster()
    {
        _damageCaster?.DisableDamageCaster();
    }

    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", .4f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        yield return new WaitForSeconds(.2f);

        _materialPropertyBlock.SetFloat("_blink", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

    }

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(1f);

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0f;
        float initialDissolveValue = 20f;
        float targetDissolveValue = -10f;
        float currentDissolveHeight;

        _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        
        while (currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            currentDissolveHeight = Mathf.Lerp(initialDissolveValue, targetDissolveValue, currentDissolveTime/dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height", currentDissolveHeight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
           yield return null;
        }
        DropItem();

    }

    public void DropItem()
    {
        if(_itemToDrop != null) 
        {
            Instantiate(_itemToDrop, transform.position, Quaternion.identity);
        }
        
    }

    public void PickUpItem(PickUp item)
    {
        switch (item.GetPickUpType()) 
        { 
            case PickUp.PickUptype.Coin:
                AddCoin(item.GetValue());
                break;

            case PickUp.PickUptype.Heal:
                AddHeal(item.GetValue());
                break;
        } 
    }

    public void AddCoin(int value)
    {
        _coins += value;
    }
    
    public void AddHeal(int value)
    {
        _health.AddHealth(value);
        GetComponent<GameVFXManager>()?.HealVFX();
    }

    public bool IsPlayer()
    {
        return _isPlayer;
    }

    public void RotateToTarget()
    {
        if (_currentState != CharacterState.Dead)
        {
            transform.LookAt(_playerTransform, Vector3.up);
        }
    }

    IEnumerator MaterialAppear()
    {
        float dissolveTimeDuration = _spawnDuration;
        float currentDissolveDuration = 0;
        float dissolveHeight;
        float startDissolveHeight = -10f;
        float targetDissolveHeight = 20f;

        _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentDissolveDuration < dissolveTimeDuration)
        {
            currentDissolveDuration += Time.deltaTime;
            dissolveHeight = Mathf.Lerp(startDissolveHeight, targetDissolveHeight, currentDissolveDuration/dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHeight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }

        _materialPropertyBlock.SetFloat("_enableDissolve", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    public CharacterState GetCurrentState()
    {
        return _currentState;
    }

    public int GetCoins()
    {
        return _coins;
    }
}


