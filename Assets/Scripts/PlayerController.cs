using System;
using System.Collections.Generic;
using UnityEngine;
using Relics;
using Random = UnityEngine.Random;

public enum PlayerColour
{
    White,
    Red,
    Green,
    Blue,
}

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public SpriteRenderer playerSpriteRenderer;
    public Sprite[] sprites;
    public GameObject aim;
    public GameObject projectile;
    public TrailRenderer dashTrail;
    public Transform swordBlade;
    public ParticleSystem swordTrail;
    public Collider2D damageCollider;
    
    [Header("Colours")]
    public Color red;
    public Color green;
    public Color blue;
    public Color white;
    public Color phaseColour;
    public PlayerColour playerColour;

    [Header("Movement Properties")]
    public float baseMoveSpeed = 4.0f;
    public float baseDashSpeed = 10.0f;
    public float baseDashDelay = 1.0f;
    public float baseDashDistance = 2.0f;
    
    [Header("Melee Attack Properties")]
    public float baseMeleeDamage = 1.0f;
    public float baseMeleeSpeed = 1.5f;
    public float baseMeleeRange = 1.0f;
    
    [Header("Ranged Attack Properties")]
    public float baseRangeDamage = 0.6f;
    public float baseRangeSpeed = 2.0f;
    public int baseMaxAmmo = 5;
    public float baseAmmoRechargeDelay = 1.2f;
    public float baseAmmoRechargeRate = 5.0f;

    [Header("Other Properties")] 
    public float baseMaxHealth = 10.0f;
    public float baseDefence;

    [Header("Uncommon Relic Effects")] 
    public float adrenalineRefill = 1.0f;
    public float burstSecondShotDelay = 0.5f;
    public int comboHits = 2;
    public float comboDelay = 1.0f;
    public float comboDamageMultiplier = 1.5f;
    public float postComboDelayMultiplier = 1.2f;
    public float homingSmoothing = 0.1f;
    public float lifeStealHealth = 0.5f;
    public float momentumDistanceMultiplier = 1.05f;
    public float phaseExtraDistance = 0.5f;
    public float resourcefulChance = 0.2f;
    public float shotgunAngle = 30.0f;
    public float stunChance = 0.1f;
    public float stunDuration = 0.5f;
    public float volatileRadius = 0.6f;
    public float volatileDamage = 1.0f;
    
    // Player properties
    public float AmmoPercentage => _ammo / baseMaxAmmo;
    
    // Movement trackers
    private Vector2 _movement;
    private float _dashDelayTimer;
    private float _dashTimer;
    private Vector2 _dashDirection;
    private bool _dashing;
    
    // Attack trackers
    private float _meleeAttackTimer;
    private float _burstShotTimer;
    private int _currentCombo;
    private float _comboTimer;
    

    // Ammo trackers
    private float _ammo;
    private float _rangeAttackTimer;
    private float _ammoRechargeTimer;
    
    // Other trackers
    private float _health;
    
    // Common relic-affected properties
    public float MoveSpeed { get; set; }
    public float DashSpeed { get; set; }
    public float DashDistance { get; set; }
    public float DashDelay { get; set; }
    public float MeleeDamage { get; set; }
    public float MeleeSpeed { get; set; }
    public float MeleeRange { get; set; }
    public float RangeDamage { get; set; }
    public float RangeSpeed { get; set; }
    public float MaxAmmo { get; set; }
    public float AmmoRechargeDelay { get; set; }
    public float AmmoRechargeRate { get; set; }
    public float MaxHealth { get; set; }
    public float Defence { get; set; }
    
    // Uncommon relic powers
    public bool Combo { get; set; }
    public bool Stun { get; set; }
    public bool LifeSteal { get; set; }
    public bool Shotgun { get; set; }
    public bool Burst { get; set; }
    public bool Volatile { get; set; }
    public bool Homing { get; set; }
    public bool Momentum { get; set; }
    public bool Adrenaline { get; set; }
    public bool Resourceful { get; set; }
    public bool Phase { get; set; }
    public bool Toxic { get; set; }
    
    // Player components
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    /// Resets properties affected by relics to their base values.
    private void ResetRelicProperties()
    {
        // Resets all stats to their base values
        MoveSpeed = baseMoveSpeed;
        DashSpeed = baseDashSpeed;
        DashDistance = baseDashDistance;
        DashDelay = baseDashDelay;
        MeleeDamage = baseMeleeDamage;
        MeleeSpeed = baseMeleeSpeed;
        MeleeRange = baseMeleeRange;
        RangeDamage = baseRangeDamage;
        RangeSpeed = baseRangeSpeed;
        MaxAmmo = baseMaxAmmo;
        AmmoRechargeDelay = baseAmmoRechargeDelay;
        AmmoRechargeRate = baseAmmoRechargeRate;
        MaxHealth = baseMaxHealth;
        Defence = baseDefence;
        
        // Resets all uncommon abilities
        Combo = false;
        Stun = false;
        LifeSteal = false;
        Shotgun = false;
        Burst = false;
        Volatile = false;
        Homing = false;
        Momentum = false;
        Adrenaline = false;
        Resourceful = false;
        Phase = false;
        Toxic = false;
    }

    /// Applies updated values of properties affected by relics to necessary components
    private void ApplyRelicPropertiesEffects()
    {
        swordBlade.localScale = new Vector3(1, MeleeRange, 1);

        ParticleSystem.MainModule swordTrailSettings = swordTrail.main;
        swordTrailSettings.startSizeYMultiplier *= MeleeRange;
    }
    
    /// Applies a list of relics to the player's stats.
    public void ApplyRelics(IEnumerable<Relic> relics)
    {
        ResetRelicProperties();
        
        foreach (Relic relic in relics)
        {
            // Checks that the relic applies to the player's current colour
            if (relic.Colour == playerColour || relic.Colour == PlayerColour.White)
            {
                relic.ApplyEffect(this);
            }
        }

        ApplyRelicPropertiesEffects();
    }

    private Color GetColour(PlayerColour colour) => colour switch
        {
            PlayerColour.White => white,
            PlayerColour.Red => red,
            PlayerColour.Green => green,
            PlayerColour.Blue => blue,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
    

    /// Changes the player's colour, adjusting the sprite colour of all necessary bits
    public void ChangeColour(PlayerColour colour)
    {
        playerColour = colour;
        playerSpriteRenderer.color = GetColour(colour);
    }

    private void ShootProjectile(Vector2 direction)
    {
        ShootSingleProjectile(direction);

        if (!Shotgun) return;

        // Handles shooting shotgun projectiles
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            
        float leftRadians = (angle - shotgunAngle) * Mathf.Deg2Rad;
        Vector2 leftDirection = new (Mathf.Sin(leftRadians), Mathf.Cos(leftRadians));
        float rightRadians = (angle + shotgunAngle) * Mathf.Deg2Rad;
        Vector2 rightDirection = new (Mathf.Sin(rightRadians), Mathf.Cos(rightRadians));

        ShootSingleProjectile(leftDirection);
        ShootSingleProjectile(rightDirection);
    }

    private void ShootSingleProjectile(Vector2 direction)
    {
        // Creates projectile
        ProjectileController p = Instantiate(projectile, transform.position, Quaternion.identity)
            .GetComponent<ProjectileController>();
            
        // Sets projectile properties
        p.SetDirection(direction);
        p.SetDamage(RangeDamage);
        p.SetColour(GetColour(playerColour));
        p.SetMomentum(Momentum ? momentumDistanceMultiplier : 1.0f);
        p.SetVolatile(Volatile, volatileRadius, volatileDamage);
        p.SetHoming(Homing, homingSmoothing);
    }

    public float GetMeleeDamage()
    {
        if (!Combo) return MeleeDamage;

        if (_comboTimer > 0)
        {
            _currentCombo++;
            if (_currentCombo == comboHits)
            {
                Debug.Log("Combo Attack");
                _comboTimer = 0;
                _currentCombo = 0;
                _meleeAttackTimer = postComboDelayMultiplier / MeleeSpeed;
                return MeleeDamage * comboDamageMultiplier;
            }
        }
        
        _comboTimer = comboDelay;
        return MeleeDamage;
    }

    public void MeleeKill()
    {
        if (!LifeSteal) return;
        _health = Mathf.Min(_health + lifeStealHealth, MaxHealth);
    }

    public void StunEnemy(EnemyController enemy)
    {
        float r = Random.Range(0.0f, 1.0f);
        if (r < stunChance)
        {
            enemy.Stun(stunDuration);
        }
    }

    private void Awake()
    {
        ResetRelicProperties();
        ApplyRelicPropertiesEffects();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Gets necessary components
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        // Initialises with full ammo
        _ammo = MaxAmmo;
        _health = MaxHealth;
        
        
        // Player setup
        ChangeColour(playerColour);
        dashTrail.emitting = false;
    }

    // Update is called once per frame
    private void Update()
    {   
        // DEBUG RELIC RESET KEY
        if (Input.GetKeyDown(KeyCode.R))
        {
            ApplyRelics(new Relic[0]);
        }
        
        
        // Handles player movement input
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        // Calculates angle towards the mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMousePos = mousePos - _rigidbody.position;
        float angle = Mathf.Atan2(toMousePos.x, toMousePos.y) * Mathf.Rad2Deg;
        
        // Rotates aim to face towards the mouse
        aim.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        
        // Sets player rotational sprite
        int spriteIndex = Mathf.RoundToInt(angle / 45) % 8;
        if (spriteIndex < 0)
        {   
            spriteIndex += 8;
        }
        playerSpriteRenderer.sprite = sprites[spriteIndex];

        // Melee attacks
        if (_meleeAttackTimer > 0)
        {
            _meleeAttackTimer -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("MeleeAttack");
            _meleeAttackTimer = 1 / MeleeSpeed;
        }

        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
        }
        
        
        // Ranged attack recharge delay
        if (_ammoRechargeTimer > 0)
        {
            _ammoRechargeTimer -= Time.deltaTime;
        } 
        else if (_ammo < MaxAmmo)
        {
            _ammo += Time.deltaTime * AmmoRechargeRate;
            
            if (_ammo > MaxAmmo)
            {
                _ammo = MaxAmmo;
            }
        }

        if (_burstShotTimer > 0)
        {
            _burstShotTimer -= Time.deltaTime;
            if (_burstShotTimer <= 0)
            {
                ShootProjectile(toMousePos.normalized);
            }
        }
        
        // Ranged attack shooting
        if (_rangeAttackTimer > 0)
        {
            _rangeAttackTimer -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire2") && _ammo >= 1)
        {
            if (Resourceful)
            {
                float r = Random.Range(0.0f, 1.0f);
                if (r > resourcefulChance)
                {
                    _ammo--;
                }
            }
            else
            {
                _ammo--;
            }
            
            _rangeAttackTimer = 1 / RangeSpeed;
            
            ShootProjectile(toMousePos.normalized);
            
            if (Burst)
            {
                _burstShotTimer = burstSecondShotDelay;
            }
            _ammoRechargeTimer = AmmoRechargeDelay * (Mathf.Max(Mathf.Log(MaxAmmo - _ammo, MaxAmmo), 0) + 1);
        }
        
        // Dashing
        if (_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
        }
        else if (_dashing)
        {
            _dashDelayTimer = DashDelay;
            _dashing = false;
            dashTrail.emitting = false;
            
            // Stops phase dash
            if (Phase)
            {
                damageCollider.enabled = true;
                ChangeColour(playerColour);
            }
        }

        if (_dashDelayTimer > 0)
        {
            _dashDelayTimer -= Time.deltaTime;
        } 
        else if (Input.GetButtonDown("Jump") && !_dashing && _movement.sqrMagnitude > 0.0)
        {
            _dashing = true;
            _dashTimer = DashDistance / DashSpeed;
            _dashDirection = _movement;
            dashTrail.emitting = true;
            
            if (!Phase) return;
            
            // Starts phase dash
            damageCollider.enabled = false;
            playerSpriteRenderer.color = phaseColour;
        }
    }

    private void FixedUpdate()
    {
        if (_dashing)
        {
            // Adjusts player velocity according to their dash
            _rigidbody.linearVelocity = _dashDirection * DashSpeed;
        }
        else
        {
            // Adjusts player velocity according to their movement input
            _rigidbody.linearVelocity = _movement * MoveSpeed;
        }
    }

    public void Damage(float damage)
    {
        damage = Mathf.Max((1 - Defence) * damage, 0);
        _health -= damage;
        
        if (_health < 0)
        {
            Debug.Log("Dead");
        }

        if (Adrenaline)
        {
            _ammo = Mathf.Min(MaxAmmo, _ammo + adrenalineRefill);
        }
    }
}
